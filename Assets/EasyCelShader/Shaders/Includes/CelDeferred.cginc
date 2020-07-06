#include "CelCommon.cginc"
#include "CelLight.cginc"

struct VertOutputDeferred
{
    UNITY_POSITION(pos);
    float4 tex                          : TEXCOORD0;
    half3 eyeVec                        : TEXCOORD1;
    half4 tangentToWorldAndPackedData[3]: TEXCOORD2;    // [3x3:tangentToWorld | 1x3:viewDirForParallax or worldPos]
    half4 ambientOrLightmapUV           : TEXCOORD5;    // SH or Lightmap UVs

    #if UNITY_REQUIRE_FRAG_WORLDPOS && !UNITY_PACK_WORLDPOS_WITH_TANGENT
        float3 posWorld                     : TEXCOORD6;
    #endif
	#ifdef _DITHER_FADE
		half pixelDepth                     : TEXCOORD7;
		half4 screenPos                     : TEXCOORD8;
	#endif
	#ifdef _VERTEX_COLORED
		half4 vertColor : COLOR;
	#endif

    UNITY_VERTEX_OUTPUT_STEREO
};


VertOutputDeferred vertexDeferred (MyVertexInput v)
{
    UNITY_SETUP_INSTANCE_ID(v);
    VertOutputDeferred o;
    UNITY_INITIALIZE_OUTPUT(VertOutputDeferred, o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
    #if UNITY_REQUIRE_FRAG_WORLDPOS
        #if UNITY_PACK_WORLDPOS_WITH_TANGENT
            o.tangentToWorldAndPackedData[0].w = posWorld.x;
            o.tangentToWorldAndPackedData[1].w = posWorld.y;
            o.tangentToWorldAndPackedData[2].w = posWorld.z;
        #else
            o.posWorld = posWorld.xyz;
        #endif
    #endif
    o.pos = UnityObjectToClipPos(v.vertex);

    o.tex = CalcTexCoords(v);
    o.eyeVec = NormalizePerVertexNormal(posWorld.xyz - _WorldSpaceCameraPos);
	#ifdef _DITHER_FADE
		o.pixelDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		o.screenPos = ComputeScreenPos(o.pos);
	#endif
    float3 normalWorld = UnityObjectToWorldNormal(v.normal);
    #ifdef _TANGENT_TO_WORLD
        float4 tangentWorld = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);

        float3x3 tangentToWorld = CreateTangentToWorldPerVertex(normalWorld, tangentWorld.xyz, tangentWorld.w);
        o.tangentToWorldAndPackedData[0].xyz = tangentToWorld[0];
        o.tangentToWorldAndPackedData[1].xyz = tangentToWorld[1];
        o.tangentToWorldAndPackedData[2].xyz = tangentToWorld[2];
    #else
        o.tangentToWorldAndPackedData[0].xyz = 0;
        o.tangentToWorldAndPackedData[1].xyz = 0;
        o.tangentToWorldAndPackedData[2].xyz = normalWorld;
    #endif

    o.ambientOrLightmapUV = 0;
    #ifdef LIGHTMAP_ON
        o.ambientOrLightmapUV.xy = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    #elif UNITY_SHOULD_SAMPLE_SH
        o.ambientOrLightmapUV.rgb = ShadeSHPerVertex (normalWorld, o.ambientOrLightmapUV.rgb);
    #endif
    #ifdef DYNAMICLIGHTMAP_ON
        o.ambientOrLightmapUV.zw = v.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    #endif

    #ifdef _PARALLAXMAP
        TANGENT_SPACE_ROTATION;
        half3 viewDirForParallax = mul (rotation, ObjSpaceViewDir(v.vertex));
        o.tangentToWorldAndPackedData[0].w = viewDirForParallax.x;
        o.tangentToWorldAndPackedData[1].w = viewDirForParallax.y;
        o.tangentToWorldAndPackedData[2].w = viewDirForParallax.z;
    #endif

	#ifdef _VERTEX_COLORED
		o.vertColor = v.vertColor;
	#endif

    return o;
}

void fragmentDeferred (
    VertOutputDeferred i,
    out half4 outGBuffer0 : SV_Target0,
    out half4 outGBuffer1 : SV_Target1,
    out half4 outGBuffer2 : SV_Target2,
    out half4 outEmission : SV_Target3          // RT3: emission (rgb), --unused-- (a)
#if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
    ,out half4 outShadowMask : SV_Target4       // RT4: shadowmask (rgba)
#endif
)
{
    #if (SHADER_TARGET < 30)
        outGBuffer0 = 1;
        outGBuffer1 = 1;
        outGBuffer2 = 0;
        outEmission = 0;
        #if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
            outShadowMask = 1;
        #endif
        return;
    #endif

    UNITY_APPLY_DITHER_CROSSFADE(i.pos.xy);

	#ifdef _VERTEX_COLORED
		MY_FRAGMENT_SETUP_VERT(s, i.vertColor)
	#else
    	MY_FRAGMENT_SETUP(s)
	#endif

	
	#if defined(_ALPHATEST_ON)
		#ifdef _DITHER_FADE
			float4 screenPos = i.screenPos;
			float4 screenPosNorm = screenPos / screenPos.w;
			screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
			float dither = Dither8x8Bayer( fmod(clipScreen.x, 8), fmod(clipScreen.y, 8) );
			float cameraDepthFade = ((i.pixelDepth - _ProjectionParams.y - _DitherStart) / _DitherFadeDistance);
			dither = step(dither, cameraDepthFade);
			clip(dither - _Cutoff );
		#endif
	#endif

    // no analytic lights in this pass
    UnityLight dummyLight = DummyLight ();
    half atten = 1;

#if UNITY_ENABLE_REFLECTION_BUFFERS
    bool sampleReflectionsInDeferred = false;
#else
    bool sampleReflectionsInDeferred = true;
#endif

	half steps = floor(_LightSteps);
    UnityGI gi = GI_Fragment (s, i.ambientOrLightmapUV, atten, dummyLight);
    half3 emissiveColor = CelLightningSpecular (s.diffColor, 0, 0, s.normalWorld, -s.eyeVec, gi.light, gi.indirect, steps).rgb;

    #ifdef _EMISSION
        emissiveColor += Emission (i.tex.xy);
    #endif

    #ifdef _FRESNEL_LIGHT
		emissiveColor += FresnelLightning(s.normalWorld, -s.eyeVec, _FresnelDirection).xyz;
	#endif

    #ifndef UNITY_HDR_ON
        emissiveColor.rgb = exp2(-emissiveColor.rgb);
    #endif

	half specular;
	half glossines;
	half3 specularColor = Specular(i.tex, specular, glossines);

    UnityStandardData data;
    data.diffuseColor   = s.diffColor;
    data.occlusion      = 0;
    data.specularColor  = specular * specularColor;
    data.smoothness     = glossines;

    data.normalWorld    = s.normalWorld;

    UnityStandardDataToGbuffer(data, outGBuffer0, outGBuffer1, outGBuffer2);

    // Emissive lighting buffer
    outEmission = half4(emissiveColor, 1);

	outGBuffer0.a = (floor(_LightSteps) / 17.0);

    // Baked direct lighting occlusion if any
    #if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
        outShadowMask = UnityGetRawBakedOcclusions(i.ambientOrLightmapUV.xy, IN_WORLDPOS(i));
    #endif
}