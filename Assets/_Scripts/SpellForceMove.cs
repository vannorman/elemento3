using OVRTouchSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Elemento
{

	public class SpellForceMove : MonoBehaviour
	{


		public enum State
		{
			Ready,
			SelectAndHighlight,
			Moving
		}
		private State state;

		private List<GameObject> selected = new List<GameObject>();
		private GameObject target;
		public float force = 500f;
		public HandPoseTracker castingHandTracker;
		public float radius = 2.1f;
		public float range = 15f;
		public float coneAngle = 30;
		public float wristAngleOffset = -30;
		GameObject overlapArea;
		public Material grabEffectMaterial;

		public static SpellForceMove Instance { get; private set; }

		private void Start()
		{
			Instance = this;
			target = new GameObject("target");
            overlapArea = GameObject.CreatePrimitive(PrimitiveType.Cube);
            overlapArea.GetComponent<Collider>().isTrigger = true;
            overlapArea.transform.localScale = new Vector3(4, 4, 4);
            var td = overlapArea.AddComponent<TriggerDetector>();
            td.onTriggerEntered += TriggerEntered;
            td.onTriggerExited += TriggerExited;
			overlapArea.AddComponent<Rigidbody>();
			overlapArea.GetComponent<Rigidbody>().useGravity = false;
			//overlapArea.GetComponent<Renderer>().enabled = false;
			overlapArea.GetComponent<Renderer>().material = grabEffectMaterial;
			var follow = overlapArea.AddComponent<SmoothFollow>();
			follow.Init(_target: target.transform, _speed: 50);
		}

		void TriggerEntered(Collider c)
		{
			if (state == State.SelectAndHighlight)
			{
				AddObjectToSelected(c.gameObject);
			}
		}

		void TriggerExited(Collider c)
		{
			if (state == State.SelectAndHighlight)
			{
				if (selected.Contains(c.gameObject))
				{
					RemoveObjectFromSelected(c.gameObject);
				}
			}
		}

        private void RemoveObjectFromSelected(GameObject go)
        {
			go.GetComponent<QuickOutline.Outline>().enabled = false;
			selected.Remove(go);
		}



		public void SetState(State newState, HandPoseTracker hand)
		{
			castingHandTracker = hand;

			state = newState;
			switch (state)
			{
				case State.SelectAndHighlight:
					overlapArea.SetActive(true);
					break;
				case State.Moving:
					overlapArea.SetActive(true);
					break;
				case State.Ready:
					overlapArea.SetActive(false);
					ClearSelected();
					break;
			}
		}

		float t = 0;
		float interval = 0.5f;
		private void Update()
		{
			switch (state)
			{
				case State.SelectAndHighlight:
					target.transform.position = castingHandTracker.hand.position + castingHandTracker.WristForwardDirection * range / 2f;
					//t -= Time.deltaTime;
					//if (t < 0)
					//{
					//    t = interval;
					//    ClearSelected();
					//    foreach (var c in Physics.SphereCastAll(new Ray(castingHand.position, castingHand.right), radius, range))
					//    {
					//        if (c.collider.gameObject.GetComponent<Rigidbody>() && Vector3.Angle(castingHand.right, c.point - castingHand.position) < coneAngle)
					//        {
					//            AddObjectToSelected(c.collider.gameObject);
					//        }
					//    }
					//}
					break;
				case State.Moving:
					target.transform.position = castingHandTracker.hand.position + castingHandTracker.WristForwardDirection * range / 2f;
					selected.ForEach(x => x.GetComponent<Rigidbody>().AddForce((target.transform.position - x.transform.position).normalized * force));
					break;
				default: break;

			}
		}

		private void ClearSelected()
		{
			foreach (var x in selected)
			{
				var qo = x.GetComponent<QuickOutline.Outline>();
				if (qo) Destroy(qo);
			}
			selected.Clear();
		}

		private void AddObjectToSelected(GameObject g)
		{
			if (!g.GetComponent<Rigidbody>() || g.GetComponent<Rigidbody>().isKinematic) return;
			selected.Add(g);
			var qo = g.GetComponent<QuickOutline.Outline>() ? g.GetComponent<QuickOutline.Outline>() : g.AddComponent<QuickOutline.Outline>();
			qo.enabled = true;
			qo.OutlineMode = QuickOutline.Outline.Mode.OutlineAll;
			qo.OutlineColor = Color.green;
		}

        public void Cancel()
        {
			
			SetState(SpellForceMove.State.Ready, null);
		}
    }
}