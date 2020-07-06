// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Xesin Creations/CelShader/Terrain" {
	Properties {
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Gloss ("Gloss", Range (0.03, 1)) = 0.078125

        // set by terrain engine
        [HideInInspector] _Control ("Control (RGBA)", 2D) = "red" {}
        [HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
        [HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
        [HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
        [HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
        [HideInInspector] _Normal3 ("Normal 3 (A)", 2D) = "bump" {}
        [HideInInspector] _Normal2 ("Normal 2 (B)", 2D) = "bump" {}
        [HideInInspector] _Normal1 ("Normal 1 (G)", 2D) = "bump" {}
        [HideInInspector] _Normal0 ("Normal 0 (R)", 2D) = "bump" {}
		_LightSteps("Light Steps", Range(1, 3)) = 2

		// used in fallback on old cards & base map
        [HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
        [HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader {
		Tags {
			"Queue" = "Geometry-100"
			"RenderType" = "Opaque"
		}

		CGPROGRAM
		#pragma surface surf EasyCel vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer noinstancing
		#pragma multi_compile_fog
		#pragma target 3.0
		// needs more than 8 texcoords
		#pragma exclude_renderers gles psp2

		#pragma multi_compile __ _TERRAIN_NORMAL_MAP

		#define TERRAIN_SURFACE_OUTPUT SurfaceOutput
		#include "TerrainSplatmapCommon.cginc"
		#include "../Includes/CelLight.cginc"

        half _Gloss;

		void surf (Input IN, inout SurfaceOutput o) {
			half4 splat_control;
            half weight;
            fixed4 mixedDiffuse;
            SplatmapMix(IN, splat_control, weight, mixedDiffuse, o.Normal);
            o.Albedo = mixedDiffuse.rgb;
            o.Alpha = weight;
            o.Gloss = _Gloss;
            o.Specular = mixedDiffuse.a;
		}
		ENDCG
	}

	Dependency "AddPassShader" = "Hidden/Xesin Creations/Splatmap/AddPass"
	Dependency "BaseMapShader" = "Hidden/Xesin Creations/Splatmap/Base"

	Fallback "Nature/Terrain/Diffuse"
}