// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/Xesin Creations/Splatmap/Base" {
	Properties {
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Gloss ("Gloss", Range (0.03, 1)) = 0.078125
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        // used in fallback on old cards
        _Color ("Main Color", Color) = (1,1,1,1)
    }

	SubShader {
		LOD 200

        CGPROGRAM
        #pragma surface surf EasyCel

        sampler2D _MainTex;
        half _Gloss;
		#define TERRAIN_SURFACE_OUTPUT SurfaceOutput
		#include "../Includes/CelLight.cginc"

        struct Input {
            float2 uv_MainTex;
        };

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = tex.rgb;
            o.Specular = tex.a;
            o.Alpha = 1.0f;
            o.Gloss = _Gloss;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
