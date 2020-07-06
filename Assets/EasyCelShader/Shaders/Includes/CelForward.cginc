#include "CelCommon.cginc"
#include "CelLight.cginc"

VertOutputForwardBase forwardVertexBase (MyVertexInput v) { 
	UNITY_SETUP_INSTANCE_ID(v);
	VertOutputForwardBase o;
	UNITY_INITIALIZE_OUTPUT(VertOutputForwardBase, o);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
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

	//We need this for shadow receving
	UNITY_TRANSFER_SHADOW(o, v.uv1);

	o.ambientOrLightmapUV = CalcVertexGIForward(v, posWorld, normalWorld);

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

	UNITY_TRANSFER_FOG(o,o.pos);
	return o;
}

VertOutputForwardAdd forwardVertexAdd (MyVertexInput v) { 
	UNITY_SETUP_INSTANCE_ID(v);
	VertOutputForwardAdd o;
	UNITY_INITIALIZE_OUTPUT(VertOutputForwardAdd, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
	o.pos = UnityObjectToClipPos(v.vertex);

	o.tex = CalcTexCoords(v);
	o.eyeVec = NormalizePerVertexNormal(posWorld.xyz - _WorldSpaceCameraPos);
	#ifdef _DITHER_FADE
		o.pixelDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		o.screenPos = ComputeScreenPos(o.pos);
	#endif
	o.posWorld = posWorld.xyz;
	float3 normalWorld = UnityObjectToWorldNormal(v.normal);
	#ifdef _TANGENT_TO_WORLD
		float4 tangentWorld = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);

		float3x3 tangentToWorld = CreateTangentToWorldPerVertex(normalWorld, tangentWorld.xyz, tangentWorld.w);
		o.tangentToWorldAndLightDir[0].xyz = tangentToWorld[0];
		o.tangentToWorldAndLightDir[1].xyz = tangentToWorld[1];
		o.tangentToWorldAndLightDir[2].xyz = tangentToWorld[2];
	#else
		o.tangentToWorldAndLightDir[0].xyz = 0;
		o.tangentToWorldAndLightDir[1].xyz = 0;
		o.tangentToWorldAndLightDir[2].xyz = normalWorld;
	#endif
	//We need this for shadow receiving
	UNITY_TRANSFER_SHADOW(o, v.uv1);

	float3 lightDir = _WorldSpaceLightPos0.xyz - posWorld.xyz * _WorldSpaceLightPos0.w;
	#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = NormalizePerVertexNormal(lightDir);
	#endif
	o.tangentToWorldAndLightDir[0].w = lightDir.x;
	o.tangentToWorldAndLightDir[1].w = lightDir.y;
	o.tangentToWorldAndLightDir[2].w = lightDir.z;

	#ifdef _PARALLAXMAP
		TANGENT_SPACE_ROTATION;
		o.viewDirForParallax = mul (rotation, ObjSpaceViewDir(v.vertex));
	#endif

	#ifdef _VERTEX_COLORED
		o.vertColor = v.vertColor;
	#endif

	UNITY_TRANSFER_FOG(o,o.pos);
	return o;

}

half4 forwardFragmentBase (VertOutputForwardBase i) : SV_Target { 
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

	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	UnityLight mainLight = MainLight ();
	CEL_LIGHT_ATTENUATION(atten, i, s.posWorld);
    half shadowAtt = SHADOW_ATTENUATION(i);
	atten *= shadowAtt;
	half steps = floor(_LightSteps);
	UnityGI gi = GI_Fragment (s, i.ambientOrLightmapUV, atten, mainLight);
	half specular;
	half glossines;
	half3 specularColor = Specular(i.tex, specular, glossines);

	half4 c = CelLightningSpecular (s.diffColor, specular * specularColor, glossines, s.normalWorld, -s.eyeVec, gi.light, gi.indirect, steps);
	#ifdef _FRESNEL_LIGHT
		half3 fl = FresnelLightning(s.normalWorld, -s.eyeVec, _FresnelDirection).xyz;
	#else
		half3 fl = half3(0, 0, 0);
	#endif
	c.rgb += Emission(i.tex.xy) + fl;

	UNITY_APPLY_FOG(i.fogCoord, c.rgb);
	return OutputForward (c, s.alpha);
}

half4 forwardFragmentBaseStepsByTexture (VertOutputForwardBase i) : SV_Target { 
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
			clip( ( ( ( i.pixelDepth + -_DitherStart ) / ( _DitherFadeDistance - _DitherStart ) ) - dither ) - _Cutoff );
		#endif
	#endif

	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	UnityLight mainLight = MainLight ();
	CEL_LIGHT_ATTENUATION(atten, i, s.posWorld);
    half shadowAtt = SHADOW_ATTENUATION(i);
	atten *= shadowAtt;	
	UnityGI gi = GI_Fragment (s, i.ambientOrLightmapUV, atten, mainLight);

	half specular;
	half glossines;
	half3 specularColor = Specular(i.tex, specular, glossines);
	half4 c = CelLightningSpecularStepsTerm (s.diffColor, specular * specularColor, glossines, s.normalWorld, -s.eyeVec, gi.light, gi.indirect);
	#ifdef _FRESNEL_LIGHT
		half3 fl = FresnelLightning(s.normalWorld, -s.eyeVec, _FresnelDirection).xyz;
	#else
		half3 fl = half3(0, 0, 0);
	#endif
	c.rgb += Emission(i.tex.xy) + fl;

	UNITY_APPLY_FOG(i.fogCoord, c.rgb);
	return OutputForward (c, s.alpha);
}

half4 forwardFragmentAddStepsByTexture (VertOutputForwardAdd i) : SV_Target { 
	UNITY_APPLY_DITHER_CROSSFADE(i.pos.xy);

	#ifdef _VERTEX_COLORED
		MY_FRAGMENT_SETUP_FWDADD_VERT(s, i.vertColor)
	#else
    	MY_FRAGMENT_SETUP_FWDADD(s)
	#endif

	#if defined(_ALPHATEST_ON)
		#ifdef _DITHER_FADE
			float4 screenPos = i.screenPos;
			float4 screenPosNorm = screenPos / screenPos.w;
			screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
			float dither = Dither8x8Bayer( fmod(clipScreen.x, 8), fmod(clipScreen.y, 8) );
			clip( ( ( ( i.pixelDepth + -_DitherStart ) / ( _DitherFadeDistance - _DitherStart ) ) - dither ) - _Cutoff );
		#endif
	#endif
	
	CEL_LIGHT_ATTENUATION(atten, i, s.posWorld);

	UnityLight light = AdditiveLight (IN_LIGHTDIR_FWDADD(i), atten);
	UnityIndirect noIndirect = ZeroIndirect ();
	half3 lightDir = half3(i.tangentToWorldAndLightDir[0].w, i.tangentToWorldAndLightDir[1].w, i.tangentToWorldAndLightDir[2].w);
	half2 textureUv = half2((1 + dot(s.normalWorld, lightDir)) * 0.5, 0);
	half steps = tex2D(_LightStepsTexture, textureUv).r;
	half specular;
	half glossines;
	half3 specularColor = Specular(i.tex, specular, glossines);
	half4 c = CelLightningSpecularStepsTerm (s.diffColor, specular * specularColor, glossines, s.normalWorld, -s.eyeVec, light, noIndirect);


	UNITY_APPLY_FOG_COLOR(i.fogCoord, c.rgb, half4(0,0,0,0)); // fog towards black in additive pass
	return OutputForward (c, s.alpha);
}

half4 forwardFragmentAdd (VertOutputForwardAdd i) : SV_Target { 
	UNITY_APPLY_DITHER_CROSSFADE(i.pos.xy);

	#ifdef _VERTEX_COLORED
		MY_FRAGMENT_SETUP_FWDADD_VERT(s, i.vertColor)
	#else
    	MY_FRAGMENT_SETUP_FWDADD(s)
	#endif

	#if defined(_ALPHATEST_ON)
		#ifdef _DITHER_FADE
			float4 screenPos = i.screenPos;
			float4 screenPosNorm = screenPos / screenPos.w;
			screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
			float dither = Dither8x8Bayer( fmod(clipScreen.x, 8), fmod(clipScreen.y, 8) );
			clip( ( ( ( i.pixelDepth + -_DitherStart ) / ( _DitherFadeDistance - _DitherStart ) ) - dither ) - _Cutoff );
		#endif
	#endif
	
	CEL_LIGHT_ATTENUATION(atten, i, s.posWorld);

	UnityLight light = AdditiveLight (IN_LIGHTDIR_FWDADD(i), atten);
	UnityIndirect noIndirect = ZeroIndirect ();
	
	half steps = floor(_LightSteps);
	half specular;
	half glossines;
	half3 specularColor = Specular(i.tex, specular, glossines);
	half4 c = CelLightningSpecular (s.diffColor, specular * specularColor, glossines, s.normalWorld, -s.eyeVec, light, noIndirect, steps);

	UNITY_APPLY_FOG_COLOR(i.fogCoord, c.rgb, half4(0,0,0,0)); // fog towards black in additive pass
	return OutputForward (c, s.alpha);
}