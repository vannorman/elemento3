// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

using System;
using UnityEngine;
using UnityEditor;

namespace XesinCreations
{
	internal class CelShaderEditorBase : ShaderGUI
	{
		public enum BlendMode
		{
			Opaque,
			Cutout,
			Fade,
			Transparent
		}

		public enum CullMode
		{
			Off,
			Front,
			Back
		}

		public enum SmoothnessMapChannel
		{
			SpecularMetallicAlpha,
			AlbedoAlpha,
		}

		protected static class Styles
		{
			public static GUIContent uvSetLabel = new GUIContent("UV Set");
			public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Transparency (A)");
			public static GUIContent specularText = new GUIContent("Specular", "Specular color (RGB) and Specular (A)");
			public static GUIContent alphaCutoffText = new GUIContent("Alpha Cutoff", "Threshold for alpha cutoff");
			public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map");
			public static GUIContent heightMapText = new GUIContent("Height Map", "Height Map (G)");
			public static GUIContent emissionText = new GUIContent("Color", "Emission (RGB)");
			public static GUIContent detailMaskText = new GUIContent("Detail Mask", "Mask for Secondary Maps (A)");
			public static GUIContent detailAlbedoText = new GUIContent("Detail Albedo x2", "Albedo (RGB) multiplied by 2");
			public static GUIContent detailNormalMapText = new GUIContent("Normal Map", "Normal Map");
			public static GUIContent emissiveWarning = new GUIContent ("Emissive value is animated but the material has not been configured to support emissive. Please make sure the material itself has some amount of emissive.");
			public static GUIContent emissiveColorWarning = new GUIContent ("Ensure emissive color is non-black for emission to have effect.");
			public static GUIContent fresnelLightTexture = new GUIContent ("Texture Lookup");

			public static string primaryMapsText = "Main Maps";
			public static string secondaryMapsText = "Secondary Maps";
			public static string forwardText = "Forward Rendering Options";
			public static string renderingMode = "Rendering Mode";
			public static string cullMode = "Cull Mode";
			public static string advancedText = "Advanced Options";
			public static readonly string[] blendNames = Enum.GetNames(typeof(BlendMode));
			public static readonly string[] cullNames = Enum.GetNames(typeof(CullMode));
		}

		protected MaterialProperty blendMode = null;
		protected MaterialProperty cullMode = null;
		protected MaterialProperty albedoMap = null;
		protected MaterialProperty albedoColor = null;
		protected MaterialProperty alphaCutoff = null;
		protected MaterialProperty specularGlossMap = null;
		protected MaterialProperty specularColor = null;
		protected MaterialProperty glossines = null;
		protected MaterialProperty bumpScale = null;
		protected MaterialProperty bumpMap = null;
		protected MaterialProperty heigtMapScale = null;
		protected MaterialProperty heightMap = null;
		protected MaterialProperty emissionColorForRendering = null;
		protected MaterialProperty emissionMap = null;
		protected MaterialProperty detailMask = null;
		protected MaterialProperty detailAlbedoMap = null;
		protected MaterialProperty detailNormalMapScale = null;
		protected MaterialProperty detailNormalMap = null;
		protected MaterialProperty uvSetSecondary = null;
		protected MaterialProperty ditherStart = null;
		protected MaterialProperty ditherFade = null;
		protected MaterialProperty fresnelLightTexture = null;
		protected MaterialProperty fresnelLightDirection = null;
		protected MaterialProperty fresnelLightPower = null;
		protected MaterialProperty fresnelLightBias = null;
		protected MaterialProperty fresnelLightIntensity = null;

		protected MaterialEditor m_MaterialEditor;
		protected bool m_FirstTimeApply = true;
		protected bool ditherEnabled = false;
		protected bool isVertexTinted = false;
		protected bool fresnelLightEnabled = false;

		public virtual void FindProperties(MaterialProperty[] props)
		{
			blendMode = FindProperty("_Mode", props);
			cullMode = FindProperty("_CullMode", props);
			albedoMap = FindProperty("_MainTex", props);
			specularGlossMap = FindProperty("_SpecGlossMap", props);
			specularColor = FindProperty("_SpecularColor", props);
			glossines = FindProperty("_Glossines", props);
			albedoColor = FindProperty("_Color", props);
			alphaCutoff = FindProperty("_Cutoff", props);
			bumpScale = FindProperty("_BumpScale", props);
			bumpMap = FindProperty("_BumpMap", props);
			heigtMapScale = FindProperty("_Parallax", props);
			heightMap = FindProperty("_ParallaxMap", props);
			emissionColorForRendering = FindProperty("_EmissionColor", props);
			emissionMap = FindProperty("_EmissionMap", props);
			detailMask = FindProperty("_DetailMask", props);
			detailAlbedoMap = FindProperty("_DetailAlbedoMap", props);
			detailNormalMapScale = FindProperty("_DetailNormalMapScale", props);
			detailNormalMap = FindProperty("_DetailNormalMap", props);
			uvSetSecondary = FindProperty("_UVSec", props);
			ditherStart = FindProperty("_DitherStart", props);
			ditherFade = FindProperty("_DitherFadeDistance", props);
			fresnelLightTexture = FindProperty("_FresnelTexture", props);
			fresnelLightDirection = FindProperty("_FresnelDirection", props);
			fresnelLightPower = FindProperty("_FresnelPower", props);
			fresnelLightBias = FindProperty("_FresnelBias", props);
			fresnelLightIntensity = FindProperty("_FresnelIntensity", props);
			
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
			m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;
			ditherEnabled = material.IsKeywordEnabled("_DITHER_FADE");
			isVertexTinted = material.IsKeywordEnabled("_VERTEX_COLORED");
			fresnelLightEnabled = material.IsKeywordEnabled("_FRESNEL_LIGHT");
			// Make sure that needed setup (ie keywords/renderqueue) are set up if we're switching some existing
			// material to a standard shader.
			// Do this before any GUI code has been issued to prevent layout issues in subsequent GUILayout statements (case 780071)
			if (m_FirstTimeApply)
			{
				MaterialChanged(material, isVertexTinted, fresnelLightEnabled);
				m_FirstTimeApply = false;
			}

			ShaderPropertiesGUI(material);
		}

		public virtual void ShaderPropertiesGUI(Material material)
		{
			// Use default labelWidth
			EditorGUIUtility.labelWidth = 0f;

			// Detect any changes to the material
			EditorGUI.BeginChangeCheck();
			{
				BlendModePopup();
				CullModePopup();

				// Primary properties
				GUILayout.Label(Styles.primaryMapsText, EditorStyles.boldLabel);
				DoAlbedoArea(material);
				DoSpecularArea(material);
				DoNormalArea();
				m_MaterialEditor.TexturePropertySingleLine(Styles.heightMapText, heightMap, heightMap.textureValue != null ? heigtMapScale : null);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailMaskText, detailMask);
				DoEmissionArea(material);
				EditorGUI.BeginChangeCheck();
				m_MaterialEditor.TextureScaleOffsetProperty(albedoMap);
				if (EditorGUI.EndChangeCheck())
					emissionMap.textureScaleAndOffset = albedoMap.textureScaleAndOffset; // Apply the main texture scale and offset to the emission texture as well, for Enlighten's sake
				DoFresnelArea();
				DoDitherArea(material);

				isVertexTinted = EditorGUILayout.Toggle("Vertex color tint", isVertexTinted);

				EditorGUILayout.Space();

				// Secondary properties
				GUILayout.Label(Styles.secondaryMapsText, EditorStyles.boldLabel);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailAlbedoText, detailAlbedoMap);
				m_MaterialEditor.TexturePropertySingleLine(Styles.detailNormalMapText, detailNormalMap, detailNormalMapScale);
				m_MaterialEditor.TextureScaleOffsetProperty(detailAlbedoMap);
				m_MaterialEditor.ShaderProperty(uvSetSecondary, Styles.uvSetLabel.text);

				// Third properties
				GUILayout.Label(Styles.forwardText, EditorStyles.boldLabel);
			}
			if (EditorGUI.EndChangeCheck())
			{
				foreach (var obj in blendMode.targets)
					MaterialChanged((Material)obj, isVertexTinted, fresnelLightEnabled);
				foreach (var obj in cullMode.targets)
					MaterialChanged((Material)obj, isVertexTinted, fresnelLightEnabled);
			}

			EditorGUILayout.Space();

			// NB renderqueue editor is not shown on purpose: we want to override it based on blend mode
			GUILayout.Label(Styles.advancedText, EditorStyles.boldLabel);
			m_MaterialEditor.EnableInstancingField();
			m_MaterialEditor.DoubleSidedGIField();
		}

		public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
		{
			// _Emission property is lost after assigning Standard shader to the material
			// thus transfer it before assigning the new shader
			if (material.HasProperty("_Emission"))
			{
				material.SetColor("_EmissionColor", material.GetColor("_Emission"));
			}

			base.AssignNewShaderToMaterial(material, oldShader, newShader);

			if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
			{
				SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"));
				return;
			}

			BlendMode blendMode = BlendMode.Opaque;
			if (oldShader.name.Contains("/Transparent/Cutout/"))
			{
				blendMode = BlendMode.Cutout;
			}
			else if (oldShader.name.Contains("/Transparent/"))
			{
				// NOTE: legacy shaders did not provide physically based transparency
				// therefore Fade mode
				blendMode = BlendMode.Fade;
			}
			material.SetFloat("_Mode", (float)blendMode);
			MaterialChanged(material, isVertexTinted, fresnelLightEnabled);
		}

		void BlendModePopup()
		{
			EditorGUI.showMixedValue = blendMode.hasMixedValue;
			var mode = (BlendMode)blendMode.floatValue;

			EditorGUI.BeginChangeCheck();
			mode = (BlendMode)EditorGUILayout.Popup(Styles.renderingMode, (int)mode, Styles.blendNames);
			if (EditorGUI.EndChangeCheck())
			{
				m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
				blendMode.floatValue = (float)mode;
			}

			EditorGUI.showMixedValue = false;
		}

		void CullModePopup()
		{
			EditorGUI.showMixedValue = cullMode.hasMixedValue;
			var mode = (CullMode)cullMode.floatValue;

			EditorGUI.BeginChangeCheck();
			mode = (CullMode)EditorGUILayout.Popup(Styles.cullMode, (int)mode, Styles.cullNames);
			if (EditorGUI.EndChangeCheck())
			{
				m_MaterialEditor.RegisterPropertyChangeUndo("Cull Mode");
				cullMode.floatValue = (float)mode;
			}

			EditorGUI.showMixedValue = false;
		}

		void DoNormalArea()
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMap, bumpMap.textureValue != null ? bumpScale : null);
		}

		void DoAlbedoArea(Material material)
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
			if (((BlendMode)material.GetFloat("_Mode") == BlendMode.Cutout))
			{
				m_MaterialEditor.ShaderProperty(alphaCutoff, Styles.alphaCutoffText.text, MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
			}
		}

		void DoSpecularArea(Material material)
		{
			m_MaterialEditor.TexturePropertySingleLine(Styles.specularText, specularGlossMap, specularColor);
			m_MaterialEditor.ShaderProperty(glossines, "Glossines", MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
		}

		void DoDitherArea(Material material)
		{
			EditorGUI.BeginChangeCheck();
				ditherEnabled = EditorGUILayout.Toggle("Dither", ditherEnabled);
			if(EditorGUI.EndChangeCheck())
				SetKeyword(material, "_DITHER_FADE", ditherEnabled);
			if(ditherEnabled){
				EditorGUI.indentLevel++;
				m_MaterialEditor.FloatProperty(ditherStart, "Start");
				m_MaterialEditor.FloatProperty(ditherFade, "Fade Distance");
				EditorGUI.indentLevel--;
			}
		}

		static bool ShouldEmissionBeEnabled(Material mat, Color color)
		{
			var realtimeEmission = (mat.globalIlluminationFlags & MaterialGlobalIlluminationFlags.RealtimeEmissive) > 0;
			return color.maxColorComponent > 0.1f / 255.0f || realtimeEmission;
		}

		bool HasValidEmissiveKeyword (Material material)
		{
			// Material animation might be out of sync with the material keyword.
			// So if the emission support is disabled on the material, but the property blocks have a value that requires it, then we need to show a warning.
			// (note: (Renderer MaterialPropertyBlock applies its values to emissionColorForRendering))
			bool hasEmissionKeyword = material.IsKeywordEnabled ("_EMISSION");
			if (!hasEmissionKeyword && ShouldEmissionBeEnabled (material, emissionColorForRendering.colorValue))
				return false;
			else
				return true;
		}

		void DoEmissionArea(Material material)
		{
			// Emission for GI?
			if (m_MaterialEditor.EmissionEnabledProperty())
			{
				bool hadEmissionTexture = emissionMap.textureValue != null;

				// Texture and HDR color controls
				m_MaterialEditor.TexturePropertyWithHDRColor(Styles.emissionText, emissionMap, emissionColorForRendering, false);

				// If texture was assigned and color was black set color to white
				float brightness = emissionColorForRendering.colorValue.maxColorComponent;
				if (emissionMap.textureValue != null && !hadEmissionTexture && brightness <= 0f)
					emissionColorForRendering.colorValue = Color.white;

				// change the GI flag and fix it up with emissive as black if necessary
				m_MaterialEditor.LightmapEmissionFlagsProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel, true);
			}
		}

		void DoFresnelArea()
		{
			fresnelLightEnabled = EditorGUILayout.Toggle("Fresnel Light", fresnelLightEnabled);
			if (fresnelLightEnabled)
			{
				EditorGUI.indentLevel++;
				m_MaterialEditor.TexturePropertySingleLine(Styles.fresnelLightTexture, fresnelLightTexture);
				m_MaterialEditor.RangeProperty(fresnelLightDirection, "Sample Direction");
				m_MaterialEditor.FloatProperty(fresnelLightPower, "Power");
				m_MaterialEditor.FloatProperty(fresnelLightBias, "Bias");
				m_MaterialEditor.FloatProperty(fresnelLightIntensity, "Intensity");
				EditorGUI.indentLevel--;
			}
		}
		public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
		{
			switch (blendMode)
			{
				case BlendMode.Opaque:
					material.SetOverrideTag("RenderType", "");
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
					material.SetInt("_ZWrite", 1);
					material.DisableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = -1;
					break;
				case BlendMode.Cutout:
					material.SetOverrideTag("RenderType", "TransparentCutout");
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
					material.SetInt("_ZWrite", 1);
					material.EnableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
					break;
				case BlendMode.Fade:
					material.SetOverrideTag("RenderType", "Transparent");
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					material.SetInt("_ZWrite", 0);
					material.DisableKeyword("_ALPHATEST_ON");
					material.EnableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
					break;
				case BlendMode.Transparent:
					material.SetOverrideTag("RenderType", "Transparent");
					material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					material.SetInt("_ZWrite", 0);
					material.DisableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
					break;
			}
		}

		static SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
		{
			int ch = (int)material.GetFloat("_SmoothnessTextureChannel");
			if (ch == (int)SmoothnessMapChannel.AlbedoAlpha)
				return SmoothnessMapChannel.AlbedoAlpha;
			else
				return SmoothnessMapChannel.SpecularMetallicAlpha;
		}

		static void SetMaterialKeywords(Material material, bool vertexTinted, bool fresnelLightEnabled)
		{
			// Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
			// (MaterialProperty value might come from renderer material property block)
			SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap") || material.GetTexture("_DetailNormalMap"));
			SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));
			SetKeyword(material, "_DETAIL_MULX2", material.GetTexture("_DetailAlbedoMap") || material.GetTexture("_DetailNormalMap"));
			SetKeyword(material, "_SPECGLOSSMAP", material.GetTexture("_SpecGlossMap"));
			SetKeyword(material, "_VERTEX_COLORED", vertexTinted);
			SetKeyword(material, "_FRESNEL_LIGHT", fresnelLightEnabled);

			bool shouldEmissionBeEnabled = (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
			SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);

			if (material.HasProperty("_SmoothnessTextureChannel"))
			{
				SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A", GetSmoothnessMapChannel(material) == SmoothnessMapChannel.AlbedoAlpha);
			}
		}

		protected static void MaterialChanged(Material material, bool vertexTinted, bool fresnelLightEnabled)
		{
			SetupMaterialWithBlendMode(material, (BlendMode)material.GetFloat("_Mode"));

			SetMaterialKeywords(material, vertexTinted, fresnelLightEnabled);
		}

		protected static void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
				m.EnableKeyword(keyword);
			else
				m.DisableKeyword(keyword);
		}
	}
}
