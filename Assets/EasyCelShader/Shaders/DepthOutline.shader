Shader "Hidden/Xesin Creations/DepthOutline"
{
    Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv[5] : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;

			sampler2D _CameraDepthNormalsTexture;
			sampler2D_float _CameraDepthTexture;

			uniform half _Sensitivity;
			uniform half _SampleDistance;
			uniform half4 _OutlineColor;

			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				float2 uv = v.texcoord.xy;


                #if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
						uv.y = 1-uv.y;
                #endif

				o.uv[0] = uv;

				// offsets for two additional samples
				o.uv[1] = uv + float2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _SampleDistance;
				o.uv[2] = uv + float2(+_MainTex_TexelSize.x, +_MainTex_TexelSize.y) * _SampleDistance;
				o.uv[3] = uv + float2(+_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * _SampleDistance;
				o.uv[4] = uv + float2(-_MainTex_TexelSize.x, +_MainTex_TexelSize.y) * _SampleDistance;

				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				fixed2 uv = i.uv[0];

				half4 original = tex2D(_MainTex, uv);
				float center = tex2D(_CameraDepthTexture, i.uv[0]).x;


				float sample1 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv[1]));
				float sample2 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv[2]));
				float sample3 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv[3]));
				float sample4 = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv[4]));

                half edge = (center - sample1) + (center - sample2) + (center - sample3) + (center - sample4);
                edge = saturate(round(edge * _Sensitivity.x));

				half4 color = lerp(original,_OutlineColor,edge);

				return color;
				//return sample1;
			}

			ENDCG
		}
	}
}
