Shader "Xesin Creations/CelShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}

		_Glossines("Glossines", Range(0,1)) = 0.4
		_SpecularColor("Specular Color", Color) = (1,1,1)
		_SpecGlossMap("Specular", 2D) = "white" {}

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02
        _ParallaxMap ("Height Map", 2D) = "black" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        _DetailMask("Detail Mask", 2D) = "white" {}

        _DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
        _DetailNormalMapScale("Scale", Float) = 1.0
        _DetailNormalMap("Normal Map", 2D) = "bump" {}

		_LightSteps("Light Steps", Range(1, 16)) = 2

		_DitherStart("Dither Start", Float) = 20
		_DitherFadeDistance("Fade distance", Float) = 5

        _FresnelTexture("Fresnel Texture", 2D) = "black" {}
        _FresnelDirection("Fresnel Direction", Range(0, 360)) = 0
        _FresnelPower("Fresnel Exponent", Float) = 0.64
        _FresnelIntensity("Fresnel Intensity", Float) = 3.7
        _FresnelBias("Fresnel Bias", Float) = -3.34

        [Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", Float) = 0
        // Blending state
        [HideInInspector] _Mode ("__mode", Float) = 0.0
        [HideInInspector] _SrcBlend ("__src", Float) = 1.0
        [HideInInspector] _DstBlend ("__dst", Float) = 0.0
        [HideInInspector] _ZWrite ("__zw", Float) = 1.0
		[HideInInspector] _CullMode ("__cll", Float) = 2.0

    }

	SubShader
    {
        Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
        LOD 300
        // ------------------------------------------------------------------
        //  Base forward pass (directional light, emission, lightmaps, ...)
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
			Cull [_CullMode]

            CGPROGRAM
            #pragma target 3.0


            // -------------------------------------

            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature _PARALLAXMAP
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _DITHER_FADE
			#pragma shader_feature _VERTEX_COLORED
            #pragma shader_feature _FRESNEL_LIGHT

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #pragma multi_compile _ LOD_FADE_CROSSFADE
			#include "UnityCG.cginc"
			#include "Includes/CelForward.cginc"

            #pragma vertex forwardVertexBase
            #pragma fragment forwardFragmentBase            

            ENDCG
        }
        // ------------------------------------------------------------------
        //  Additive forward pass (one light per pass)
        Pass
        {
            Name "FORWARD_DELTA"
            
            Tags { "LightMode" = "ForwardAdd" }
            Blend [_SrcBlend] One
            Fog { Color (0,0,0,0) } // in additive pass fog should be black
            ZWrite Off
            ZTest LEqual
			Cull [_CullMode]


            CGPROGRAM
            #pragma target 3.0


            // -------------------------------------
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature _PARALLAXMAP
			 #pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _DITHER_FADE
			#pragma shader_feature _VERTEX_COLORED
            #pragma shader_feature _FRESNEL_LIGHT

            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma multi_compile _ LOD_FADE_CROSSFADE

			#include "UnityCG.cginc"
			#include "Includes/CelForward.cginc"

            #pragma vertex forwardVertexAdd
            #pragma fragment forwardFragmentAdd

            ENDCG

        }
        // ------------------------------------------------------------------
        //  Shadow rendering pass
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual

            CGPROGRAM
            #pragma target 3.0

            // -------------------------------------


            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _METALLICGLOSSMAP
            #pragma shader_feature _PARALLAXMAP
			#pragma shader_feature _DITHER_FADE

            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing
            #pragma multi_compile _ LOD_FADE_CROSSFADE
			

            #pragma vertex celVertShadowCaster
            #pragma fragment celFragShadowCaster

            #include "Includes/CelShadowCaster.cginc"

            ENDCG
        }
        // ------------------------------------------------------------------
        //  Deferred pass
        Pass
        {
            Name "DEFERRED"
            Tags { "LightMode" = "Deferred" }
			Cull [_CullMode]

            CGPROGRAM
            #pragma target 3.0
            #pragma exclude_renderers nomrt


            // -------------------------------------

            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICGLOSSMAP
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature _PARALLAXMAP
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _DITHER_FADE
			#pragma shader_feature _VERTEX_COLORED
            #pragma shader_feature _FRESNEL_LIGHT

            #pragma multi_compile_prepassfinal
            #pragma multi_compile_instancing
            #pragma multi_compile _ LOD_FADE_CROSSFADE

			#include "UnityCG.cginc"
            #include "Includes/CelDeferred.cginc"

            #pragma vertex vertexDeferred
            #pragma fragment fragmentDeferred

            ENDCG
        }

        // ------------------------------------------------------------------
        // Extracts information for lightmapping, GI (emission, albedo, ...)
        // This pass it not used during regular rendering.
        Pass
        {
            Name "META"
            Tags { "LightMode"="Meta" }

            Cull Off

            CGPROGRAM
            #pragma vertex vert_meta
            #pragma fragment frag_meta

            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICGLOSSMAP
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature EDITOR_VISUALIZATION

            #include "UnityStandardMeta.cginc"
            ENDCG
        }
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
        LOD 150
        // ------------------------------------------------------------------
        //  Base forward pass (directional light, emission, lightmaps, ...)
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
			Cull [_CullMode]

            CGPROGRAM
            #pragma target 3.0


            // -------------------------------------
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _DITHER_FADE
			#pragma shader_feature _VERTEX_COLORED
            #pragma shader_feature _FRESNEL_LIGHT

			#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

			#include "UnityCG.cginc"
			#include "Includes/CelForward.cginc"

            #pragma vertex forwardVertexBase
            #pragma fragment forwardFragmentBase            

            ENDCG
        }
        // ------------------------------------------------------------------
        //  Additive forward pass (one light per pass)
        Pass
        {
            Name "FORWARD_DELTA"
            Tags { "LightMode" = "ForwardAdd" }
            Blend [_SrcBlend] One
            Fog { Color (0,0,0,0) } // in additive pass fog should be black
            ZWrite Off
            ZTest LEqual
			Cull [_CullMode]

            CGPROGRAM
            #pragma target 3.0

            // -------------------------------------


            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _DITHER_FADE
			#pragma shader_feature _SPECGLOSSMAP
			#pragma skip_variants SHADOWS_SOFT
			#pragma shader_feature _VERTEX_COLORED
            #pragma shader_feature _FRESNEL_LIGHT

            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "Includes/CelForward.cginc"

            #pragma vertex forwardVertexAdd
            #pragma fragment forwardFragmentAdd

            ENDCG

        }
        // ------------------------------------------------------------------
        //  Shadow rendering pass
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual

            CGPROGRAM
            #pragma target 3.0

            // -------------------------------------


            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _METALLICGLOSSMAP
            #pragma shader_feature _PARALLAXMAP
			#pragma shader_feature _DITHER_FADE

            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing

            #pragma vertex celVertShadowCaster
            #pragma fragment celFragShadowCaster

            #include "Includes/CelShadowCaster.cginc"

            ENDCG
        }
        // ------------------------------------------------------------------
        //  Deferred pass
        Pass
        {
            Name "DEFERRED"
            Tags { "LightMode" = "Deferred" }
			Cull [_CullMode]

            CGPROGRAM
            #pragma target 3.0
            #pragma exclude_renderers nomrt

            // -------------------------------------

            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICGLOSSMAP
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature _PARALLAXMAP
			#pragma shader_feature _DITHER_FADE
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _VERTEX_COLORED
            #pragma shader_feature _FRESNEL_LIGHT

            #pragma multi_compile_prepassfinal
            #pragma multi_compile_instancing

			#include "UnityCG.cginc"
            #include "Includes/CelDeferred.cginc"

            #pragma vertex vertexDeferred
            #pragma fragment fragmentDeferred

            ENDCG
        }

        // ------------------------------------------------------------------
        // Extracts information for lightmapping, GI (emission, albedo, ...)
        // This pass it not used during regular rendering.
        Pass
        {
            Name "META"
            Tags { "LightMode"="Meta" }

            Cull Off

            CGPROGRAM
            #pragma vertex vert_meta
            #pragma fragment frag_meta

            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICGLOSSMAP
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature EDITOR_VISUALIZATION


            #include "UnityStandardMeta.cginc"
            ENDCG
        }
    }

    Fallback "Transparent/Cutout/Diffuse"
    CustomEditor "XesinCreations.CelShaderEditor"
}
