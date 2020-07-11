using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class Utils2
{
	public static Transform FindInChildren(this Transform t, string name)
	{
		return (from x in t.GetComponentsInChildren<Transform>()
				where x.gameObject.name == name
				select x.gameObject).FirstOrDefault()?.transform;
	}

#if UNITY_EDITOR
	[MenuItem("Edit/Editor Tools/Set Local Position To Zero %#y")]
	public static void SetLocalPositionToZero()
	{
		foreach (GameObject o in Selection.gameObjects)
		{
			if (o.GetComponent<RectTransform>())
			{
				o.GetComponent<RectTransform>().localPosition = Vector3.zero;

			}
			else
			{
				o.transform.localPosition = Vector3.zero;
			}
			o.transform.localRotation = Quaternion.identity;
		}
	}

	[MenuItem("Edit/Editor Tools/Set Local Rotation To Zero %y")]
	public static void SetLocalRotationToZero()
	{
		foreach (GameObject o in Selection.gameObjects)
		{
			o.transform.localRotation = Quaternion.identity;
		}
	}

	[MenuItem("Edit/Editor Tools/Group Items %e")]
	public static void GroupItems()
	{
		var avg = Vector3.zero;
		var p = Selection.activeGameObject.transform.parent;
		Selection.gameObjects.ToList().ForEach(x => { avg += x.transform.position; });
		avg /= Selection.gameObjects.Length;
		var g = new GameObject(Selection.gameObjects[0].name+ " group");
		Selection.gameObjects.ToList().ForEach((x => x.transform.SetParent(g.transform)));
		Selection.activeGameObject = g;
		Selection.activeGameObject.transform.parent = p;
		/// Selection.gameObjects.Count;
	}

	[MenuItem("Edit/Editor Tools/Clump Items %w")]
	public static void ClumpItems()
	{
		var avg = Vector3.zero;
		Selection.gameObjects.ToList().ForEach(x => { avg += x.transform.position; });
		avg /= Selection.gameObjects.Length;
		Selection.gameObjects.ToList().ForEach(x => { x.transform.position += (avg - x.transform.position).normalized; });
		/// Selection.gameObjects.Count;
	}




    [MenuItem("Edit/Editor Tools/DeClump Items #%w")]
	public static void DeClumpItems()
	{
		var avg = Vector3.zero;
		Selection.gameObjects.ToList().ForEach(x => { avg += x.transform.position; });
		avg /= Selection.gameObjects.Length;
		Selection.gameObjects.ToList().ForEach(x => { x.transform.position -= (avg - x.transform.position).normalized; });
		/// Selection.gameObjects.Count;
	}
#endif

	static Text debugOut;
	internal static void SpellDebug(string v)
	{
		if (!debugOut) debugOut = GameObject.FindObjectOfType<DebugOut>()?.GetComponent<Text>();
		if (!debugOut) return;
		debugOut.text += "\n" + v;
		var lines = debugOut.text.Split('\n');
		var maxLines = 20;
		if (lines.Length > maxLines)
		{
			// trim top if excess
			var lines2 = lines.ToList().Skip(1).ToArray();
			var lines2string = string.Join("\n", lines2);
			debugOut.text = lines2string;
		}
	}

	public static Color Purple

	{
		get { return new Color(1, 0, 1); }
	}

	public static Color RandomColor
	{
		get {
			var i = Random.Range(0, 9);
			switch (i)
			{
				case 0: return Color.red;
				case 1: return new Color(1, 1, 0.25f);
				case 2: return new Color(1, 1, 0);
				case 3: return new Color(0, 1, 0);
				case 4: return new Color(0, 0, 1);
				case 5: return new Color(0, 1, 1);
				case 6: return Color.magenta;
				case 7: return Color.cyan;
				case 8: return Color.yellow;
				default: return Color.black;
			}

		}
	}


	#region MathUtils
	internal static int FriendlyAngle(float v)
	{
		return FriendlyAngle(Mathf.RoundToInt(v));
	}

	internal static int FriendlyAngle(int v)
	{
		if (v > 180)
			return v - 360;
		else return v;
	}

	public static int IntParse(float f)
	{
		return IntParse(f.ToString());
	}
	public static int IntParse(string s)
	{
		s = s.Replace(" ", "");
		if (s == "") return 1;
		int result = int.Parse(Regex.Replace(s, "[^0-9]", ""));
		bool neg = s.ToCharArray()[0] == '-';
		return neg ? -result : result;
	}

	public static Vector3 FlattenVector(Vector3 a)
	{
		return new Vector3(a.x, 0, a.z);
	}

	internal static void DebugSphere(Vector3 p, float scale, Color color, float destroyAfterSeconds = 4f)
    {
		var s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		s.transform.position = p;
		s.transform.localScale = Vector3.one * scale;
		s.GetComponent<Renderer>().material.color = color;
		GameObject.Destroy(s.GetComponent<Collider>());
		var tod = s.AddComponent<TimedObjectDestructor>();
		tod.DestroyAfterSeconds(destroyAfterSeconds);

    }
    #endregion
}
