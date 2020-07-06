// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

using System;
using UnityEngine;
using UnityEditor;

namespace XesinCreations
{
    internal class CelShaderEditorByTexture : CelShaderEditorBase
    {

        public static GUIContent lighStepsText = new GUIContent("Light steps", "Grayscale gradient (R)");
        MaterialProperty lightSteps = null;

        public override void FindProperties(MaterialProperty[] props)
        {
            base.FindProperties(props);
			lightSteps = FindProperty("_LightStepsTexture", props);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            base.OnGUI(materialEditor, props);
        }

        public override void ShaderPropertiesGUI(Material material)
        {
            base.ShaderPropertiesGUI(material);

            GUILayout.Label("Extra properties", EditorStyles.boldLabel);
            m_MaterialEditor.TexturePropertySingleLine(lighStepsText, lightSteps);
        }
    }
}
