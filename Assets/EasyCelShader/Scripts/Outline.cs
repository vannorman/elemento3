using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ImageEffectAllowedInSceneView, ExecuteInEditMode]
public class Outline : MonoBehaviour {

	private Material _mat = null;
	private Camera ownerCamera = null;

	public float outlineWidth = 1;
	public Color outlineColor = Color.black;
	public float sensitivity = 500f;

	private Material material {
		get{
			if(_mat == null){
				Shader shader = Shader.Find("Hidden/Xesin Creations/DepthOutline");
				_mat = new Material(shader);
			}

			return _mat;
		}
	}

	void OnEnable()
	{
		ownerCamera = GetComponent<Camera>();
		ownerCamera.depthTextureMode = DepthTextureMode.Depth;
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		material.SetFloat("_SampleDistance", outlineWidth);
		material.SetColor("_OutlineColor", outlineColor);
		material.SetFloat("_Sensitivity", sensitivity);
		Graphics.Blit(src, dest, material);
	}
}
