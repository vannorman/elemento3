using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elemento
{
	using Pose = PoseData.Pose;
	public class HandPoseTracker : MonoBehaviour
	{
		
		public Transform hand;
		public GameObject _smoothFollowHand;
		public Transform _bones;

		public bool initialized { get; private set; }
		public delegate void OnInitialized();
		public OnInitialized onInitialized;

		public Transform _indexTip;
		public Transform _middleTip;
		public Transform _thumbTip;
		public Transform _ringTip;
		public Transform _pinkyTip;

		public float wristPointsForward => Mathf.Round(Vector3.Angle(hand.right, Camera.main.transform.forward) * 100) / 100f; // more zero is more forward
		public float palmPointsUp => Mathf.Round(Vector3.Angle(Vector3.up, -hand.up) * 100) / 100f; // more zero is more palm up toward sky
		public float palmFaceRight => Mathf.Round(Vector3.Angle(hand.right, Camera.main.transform.right) * 100) / 100f; // more zero means LEFT hand faces right
		public float wristPointsUp => Mathf.Round(Vector3.Angle(Vector3.up, hand.right) * 100) / 100f; // more zero is forearm point toward sky

		public Pose CurrentPose = new Pose();

		public Joint GetJoint(Finger f, int index)
		{
			return joints.Where(x => x.finger == f && x.index == index).First();
		}
		List<Joint> joints = new List<Joint>();


		public Transform Bones
		{
			get
			{
				if (!_bones) _bones = hand.FindInChildren("Bones");
				return _bones;
			}
		}

		

		private void InitializeJoints()
		{
			Bones.GetComponentsInChildren<Transform>().ToList()
				.Where(x => !x.name.Contains("Tip")).ToList()
				.Where(x => Joint.GetFingerIndexFromName(x.name) != -1).ToList() // only joints with numbers, e.g. not forearm stub
				.ForEach(x => joints.Add(
					 new Joint(Joint.GetFingerTypeFromName(x.name),
					 Joint.GetFingerIndexFromName(x.name),
					 x)));
		}

		private void Update()
		{
			
			if (!initialized)
			{
				if (Bones)
				{
					InitializeJoints();
					InitializeTips();
					if (_indexTip != null)
					{
						initialized = true;
						onInitialized?.Invoke();

					}
				}
			}
			if (!initialized) return;

			// save values to current pose
			CurrentPose.indexFirstJoint = Utils2.FriendlyAngle(GetJoint(Finger.Index, 1).joint.localEulerAngles.z);
			CurrentPose.indexTotal = joints.Where(x => x.finger == Finger.Index).Select(x => Utils2.FriendlyAngle(x.joint.localEulerAngles.z)).Sum();
			CurrentPose.middleFirstJoint = Utils2.FriendlyAngle(joints.Where(x => x.finger == Finger.Middle && x.index == 1).FirstOrDefault().joint.localEulerAngles.z);
			CurrentPose.middleTotal = joints.Where(x => x.finger == Finger.Middle).Select(x => Utils2.FriendlyAngle(x.joint.localEulerAngles.z)).Sum();
			CurrentPose.ringFirstJoint = Utils2.FriendlyAngle(joints.Where(x => x.finger == Finger.Ring && x.index == 1).FirstOrDefault().joint.localEulerAngles.z);
			CurrentPose.ringTotal = joints.Where(x => x.finger == Finger.Ring).Select(x => Utils2.FriendlyAngle(x.joint.localEulerAngles.z)).Sum();
			CurrentPose.pinkyFirstJoint = Utils2.FriendlyAngle(joints.Where(x => x.finger == Finger.Pinky && x.index == 1).FirstOrDefault().joint.localEulerAngles.z);
			CurrentPose.pinkyTotal = joints.Where(x => x.finger == Finger.Pinky).Select(x => Utils2.FriendlyAngle(x.joint.localEulerAngles.z)).Sum();

			CurrentPose.wristPointsForward = wristPointsForward;
			CurrentPose.palmPointsUp = palmPointsUp;
			CurrentPose.palmFaceRight = palmFaceRight;
			CurrentPose.wristPointsUp = wristPointsUp;
		}

		float GetTipDistance(Finger a, Finger b)
		{
			var val = (Mathf.RoundToInt((GetFingerTipPosition(a) - GetFingerTipPosition(b)).magnitude * 100) / 100f);
			return val;
		}

		public Vector3 GetFingerTipPosition(Finger finger)
		{
			if (!_thumbTip) return Vector3.zero;
			switch (finger)
			{
				case Finger.Thumb: return _thumbTip.position;
				case Finger.Index: return _indexTip.position;
				case Finger.Middle: return _middleTip.position;
				case Finger.Ring: return _ringTip.position;
				case Finger.Pinky: return _pinkyTip.position;
				default: return Vector3.zero;
			}
		}

		private void InitializeTips()
		{
			// another second order initialization
			_thumbTip = Bones.FindInChildren("Hand_ThumbTip").transform;
			_indexTip = Bones.FindInChildren("Hand_IndexTip").transform;
			_middleTip = Bones.FindInChildren("Hand_MiddleTip").transform;
			_ringTip = Bones.FindInChildren("Hand_RingTip").transform;
			_pinkyTip = Bones.FindInChildren("Hand_PinkyTip").transform;
		}

	

		public void Start()
		{
			_smoothFollowHand = new GameObject("smooth follow hand");
			var sf = _smoothFollowHand.AddComponent<SmoothFollow>();
			sf.Init(hand, 6f);
		}
	}

	public class Joint
	{
		public Finger finger;
		public int index;
		public Transform joint;
		public Joint(Finger _finger, int _index, Transform _joint)
		{
			finger = _finger;
			index = _index;
			joint = _joint;
		}
		public static int GetFingerIndexFromName(string s)
		{
			try { return Utils2.IntParse(s); }
			catch { return -1; }
		}
		public static Finger GetFingerTypeFromName(string s)
		{
			if (s.Contains("Index")) return Finger.Index;
			if (s.Contains("Middle")) return Finger.Middle;
			if (s.Contains("Ring")) return Finger.Ring;
			if (s.Contains("Pinky")) return Finger.Pinky;
			return Finger.Thumb;
		}

	}

	public enum Finger { Index, Middle, Ring, Pinky, Thumb }

}


