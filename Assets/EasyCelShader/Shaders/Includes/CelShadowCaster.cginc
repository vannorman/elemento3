#include "UnityStandardShadow.cginc"


half _DitherStart;
half _DitherFadeDistance;

inline float Dither8x8Bayer( int x, int y )
{
	const float dither[ 64 ] = {
		1, 49, 13, 61,  4, 52, 16, 64,
		33, 17, 45, 29, 36, 20, 48, 32,
		9, 57,  5, 53, 12, 60,  8, 56,
		41, 25, 37, 21, 44, 28, 40, 24,
		3, 51, 15, 63,  2, 50, 14, 62,
		35, 19, 47, 31, 34, 18, 46, 30,
		11, 59,  7, 55, 10, 58,  6, 54,
		43, 27, 39, 23, 42, 26, 38, 22};
	int r = y * 8 + x;
	return dither[r] / 64;
}

struct CelVertexOutputShadowCaster
{
    V2F_SHADOW_CASTER_NOPOS
	float2 tex : TEXCOORD1;

	#if defined(_PARALLAXMAP)
		half3 viewDirForParallax : TEXCOORD2;
	#endif
	#ifdef _DITHER_FADE
		half pixelDepth                     : TEXCOORD3;
		half4 screenPos                     : TEXCOORD4;
	#endif
};

void celVertShadowCaster (VertexInput v
    , out float4 opos : SV_POSITION
    , out CelVertexOutputShadowCaster o
    #ifdef UNITY_STANDARD_USE_STEREO_SHADOW_OUTPUT_STRUCT
    , out VertexOutputStereoShadowCaster os
    #endif
)
{
    UNITY_SETUP_INSTANCE_ID(v);
    #ifdef UNITY_STANDARD_USE_STEREO_SHADOW_OUTPUT_STRUCT
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(os);
    #endif
    TRANSFER_SHADOW_CASTER_NOPOS(o,opos)
	#ifdef _DITHER_FADE
		o.pixelDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		o.screenPos = ComputeScreenPos(opos);
	#endif

	o.tex = TRANSFORM_TEX(v.uv0, _MainTex);

	#ifdef _PARALLAXMAP
		TANGENT_SPACE_ROTATION;
		o.viewDirForParallax = mul (rotation, ObjSpaceViewDir(v.vertex));
	#endif
}

half4 celFragShadowCaster (UNITY_POSITION(vpos)
    , CelVertexOutputShadowCaster i
) : SV_Target
{	

		#if defined(_ALPHATEST_ON)
			half alpha = tex2D(_MainTex, i.tex).a * _Color.a;

			#ifdef _DITHER_FADE
				float4 screenPos = i.screenPos;
				float4 screenPosNorm = screenPos / screenPos.w;
				float startDithering = _DitherStart + _ProjectionParams.y;
				screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
				float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
				float dither = Dither8x8Bayer( fmod(clipScreen.x, 8), fmod(clipScreen.y, 8) );
				clip( ( ( ( i.pixelDepth  + -_DitherStart ) / ( _DitherFadeDistance - _DitherStart ) ) - dither ) - _Cutoff );
			#endif
			clip (alpha - _Cutoff);
		#endif
		#if defined(_PARALLAXMAP) && (SHADER_TARGET >= 30)
			half3 viewDirForParallax = normalize(i.viewDirForParallax);
			fixed h = tex2D (_ParallaxMap, i.tex.xy).g;
			half2 offset = ParallaxOffset1Step (h, _Parallax, viewDirForParallax);
			i.tex.xy += offset;
		#endif

        
        #if defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)
            #if defined(_ALPHAPREMULTIPLY_ON)
                half outModifiedAlpha;
                PreMultiplyAlpha(half3(0, 0, 0), alpha, SHADOW_ONEMINUSREFLECTIVITY(i.tex), outModifiedAlpha);
                alpha = outModifiedAlpha;
            #endif
            #if defined(UNITY_STANDARD_USE_DITHER_MASK)
                // Use dither mask for alpha blended shadows, based on pixel position xy
                // and alpha level. Our dither texture is 4x4x16.
                #ifdef LOD_FADE_CROSSFADE
                    #define _LOD_FADE_ON_ALPHA
                    alpha *= unity_LODFade.y;
                #endif
                half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy*0.25,alpha*0.9375)).a;
                clip (alphaRef - 0.01);
            #else
                clip (alpha - _Cutoff);
            #endif
        #endif

    #ifdef LOD_FADE_CROSSFADE
        #ifdef _LOD_FADE_ON_ALPHA
            #undef _LOD_FADE_ON_ALPHA
        #else
            UnityApplyDitherCrossFade(vpos.xy);
        #endif
    #endif

    SHADOW_CASTER_FRAGMENT(i)
	
}