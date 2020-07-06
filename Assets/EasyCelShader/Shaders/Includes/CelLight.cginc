half _LightSteps;
sampler2D _LightStepsTexture;
sampler2D _FresnelTexture;
half _FresnelDirection;
half _FresnelPower;
half _FresnelIntensity;
half _FresnelBias;

half4 CelLightningSpecular (half3 diffColor, half3 specColor, half smoothness, 
	half3 normal, half3 viewDir,
	UnityLight light, UnityIndirect gi, half steps)
{
	half3 halfDir = Unity_SafeNormalize (light.dir + viewDir);

#if UNITY_HANDLE_CORRECTLY_NEGATIVE_NDOTV
	// The amount we shift the normal toward the view vector is defined by the dot product.
	half shiftAmount = dot(normal, viewDir);
	normal = shiftAmount < 0.0f ? normal + viewDir * (-shiftAmount + 1e-5f) : normal;
#endif
	half nl = saturate(dot(normal, light.dir));
	half nh = saturate(dot(normal, halfDir));
	half clampSmooth = max(0.02, smoothness);
	half specular = pow (nh, clampSmooth*128.0);

	// Diffuse term
	half diffuseTerm = (1.0 / steps) * ceil(nl) + floor(nl * steps) / steps;
	half specularTerm = (1.0 / steps) * ceil(specular - 0.02) + floor(specular * steps) / steps;

	half3 color =   diffColor * (gi.diffuse + light.color * diffuseTerm) +
					specularTerm * light.color * specColor;

	return half4(color, 1);
}

half4 CelLightningSpecularStepsTerm (half3 diffColor, half3 specColor, half smoothness, 
	half3 normal, half3 viewDir,
	UnityLight light, UnityIndirect gi)
{
	half3 halfDir = Unity_SafeNormalize (light.dir + viewDir);

#if UNITY_HANDLE_CORRECTLY_NEGATIVE_NDOTV
	// The amount we shift the normal toward the view vector is defined by the dot product.
	half shiftAmount = dot(normal, viewDir);
	normal = shiftAmount < 0.0f ? normal + viewDir * (-shiftAmount + 1e-5f) : normal;
#endif
	half nl = saturate((1 + dot(normal, light.dir)) * 0.5);
	half nh = saturate((1 + dot(normal, halfDir)) * 0.5);
	half clampSmooth = max(0.02, smoothness);
	half specular = pow (nh, clampSmooth*128.0);

	// Diffuse term
	half diffuseTerm = tex2D( _LightStepsTexture, nl.xx).r;
	half specularTerm = tex2D( _LightStepsTexture, specular.xx).r;

	half3 color =   diffColor * (gi.diffuse + light.color * diffuseTerm) +
					specularTerm * light.color * specColor;

	return half4(color, 1);
}

half4 FresnelLightning(half3 normal, half3 viewDir, half sampleDir)
{
	float PI = 3.14159;
	half sampleDirRads = sampleDir * PI / 180;
	half VdN = 1 - saturate(dot(normal, viewDir));
	half3 Dir = normalize(half3(sin(sampleDirRads), cos(sampleDirRads), 0.0));
	half DdN = (dot(normal, Dir) + 1.0) / 2.0;
	half2 SampleUV = half2(DdN, 0.0);

	half fresnelPower = _FresnelPower;
	half fresIntensity = _FresnelIntensity;
	half LightTerm = max(0.0, _FresnelBias + fresIntensity * (pow(VdN, fresnelPower)));
	half4 AmbientColor = tex2D(_FresnelTexture, SampleUV) * LightTerm;

	return AmbientColor;
}


#ifdef TERRAIN_SURFACE_OUTPUT

		inline half4 LightingEasyCel (TERRAIN_SURFACE_OUTPUT s, half3 viewDir, UnityGI gi)
		{
			fixed4 c;
			c = CelLightningSpecular(s.Albedo, _SpecColor, s.Gloss, s.Normal, viewDir, gi.light, gi.indirect, round(_LightSteps));
			return c;
		}

		inline half4 LightingEasyCel_Deferred (TERRAIN_SURFACE_OUTPUT s, half3 viewDir, UnityGI gi, out half4 outGBuffer0, out half4 outGBuffer1, out half4 outGBuffer2)
		{
			UnityStandardData data;
			data.diffuseColor = s.Albedo;
			data.occlusion = 0;
			// PI factor come from StandardBDRF (UnityStandardBRDF.cginc:351 for explanation)
			data.specularColor = _SpecColor.rgb * s.Gloss * (1/UNITY_PI);
			data.smoothness = s.Specular;
			data.normalWorld = s.Normal;

			UnityStandardDataToGbuffer(data, outGBuffer0, outGBuffer1, outGBuffer2);

			half4 emission = half4(s.Emission, 1);

			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				emission.rgb += s.Albedo * gi.indirect.diffuse;
			#endif

			outGBuffer0.a = (floor(_LightSteps) / 17.0);

			return emission;
		}

		inline void LightingEasyCel_GI (
			TERRAIN_SURFACE_OUTPUT s,
			UnityGIInput data,
			inout UnityGI gi)
		{
			gi = UnityGlobalIllumination (data, 1.0, s.Normal);
		}

#endif