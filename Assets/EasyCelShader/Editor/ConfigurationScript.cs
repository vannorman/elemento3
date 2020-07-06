using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConfigurationScript {

 	[MenuItem("XesinCreations/Easy CelShading/Set Custom Deferred")]
	static void SetDeferred(){
		 const string GraphicsSettingsAssetPath = "ProjectSettings/GraphicsSettings.asset";
        SerializedObject graphicsManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(GraphicsSettingsAssetPath)[0]);   
        SerializedProperty mode = graphicsManager.FindProperty ("m_Deferred" + ".m_Mode");
        SerializedProperty shader = graphicsManager.FindProperty("m_Deferred" + ".m_Shader");

		SerializedProperty RefMode = graphicsManager.FindProperty ("m_DeferredReflections" + ".m_Mode");
        SerializedProperty RefShader = graphicsManager.FindProperty("m_DeferredReflections" + ".m_Shader");

        //None = 0,           Builtin = 1,            Custom = 2
 		mode.intValue = 2;
		RefMode.intValue = 2;

        shader.objectReferenceValue = Shader.Find("Hidden/Internal-DeferredCelShading");
        RefShader.objectReferenceValue = Shader.Find("Hidden/Internal-DeferredReflectionsCelshading");

        graphicsManager.ApplyModifiedProperties();
	}

	[MenuItem("XesinCreations/Easy CelShading/Set Default Deferred")]
	static void SetDefaultDeferred(){
		 const string GraphicsSettingsAssetPath = "ProjectSettings/GraphicsSettings.asset";
        SerializedObject graphicsManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(GraphicsSettingsAssetPath)[0]);   
        SerializedProperty mode = graphicsManager.FindProperty ("m_Deferred" + ".m_Mode");

		SerializedProperty RefMode = graphicsManager.FindProperty ("m_DeferredReflections" + ".m_Mode");

        //None = 0,           Builtin = 1,            Custom = 2
 		mode.intValue = 1;
 		RefMode.intValue = 1;

        graphicsManager.ApplyModifiedProperties();
	}
}
