﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elemento
{
	public class HandCollider : MonoBehaviour
	{
		public HandPoseTracker hand;
		public GameObject fingerTipTool;
		bool initialized = false;
		// Start is called before the first frame update
		void Initialize()
		{
			initialized = true;
			var tipCollider = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			tipCollider.transform.localScale *= .015f;
			tipCollider.transform.SetParent(hand._indexTip);
			tipCollider.transform.localPosition = Vector3.zero;
			tipCollider.tag = hand.handType == HandType.Left ? TagManager.TipColliderOverride_Left : TagManager.TipColliderOverride_Right;
			tipCollider.transform.forward = -tipCollider.transform.parent.right;
			tipCollider.name = tipCollider.tag;
			tipCollider.transform.parent.gameObject.AddComponent<FingerTipCollider>();
		}

		// Update is called once per frame
		void Update()
		{
			if (hand.initialized && !initialized)
			{
				Initialize();
				return;
			}

		}
	}
}
