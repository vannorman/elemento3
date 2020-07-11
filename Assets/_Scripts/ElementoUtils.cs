using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ElementoUtils
{

#if UNITY_EDITOR
	[MenuItem("Edit/Editor Tools/Align Hand Rotation In Scene For Editing %&r")]
	public static void AlignHandRot()
	{

		GameObject.Find("LeftHandAnchor").transform.eulerAngles = new Vector3(180, 0, 0);
	}
#endif

}
