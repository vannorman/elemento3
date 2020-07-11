using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Elemento
{ 
	public static class PoseData
	{
		// Start is called before the first frame update
		public static bool PoseWithinTolerance(Pose a, Pose b, Pose tol)
		{
			var i = 0;
			return !a.Values.Select(x => Mathf.Abs(x - b.Values[i]) < Mathf.Abs(tol.Values[i++])).Contains(false); // check if a - b is inside tolerance for every case
		}



		public static float[] ComparePose(Pose a, Pose b)
		{
			var i = 0;
			return a.Values.Select(x => x - b.Values[i++]).ToArray();
		}

		public struct Pose
		{
			public float thumbToIndex;
			public float thumbToMiddle;
			public float thumbToRing;
			public float thumbToPinky;
			public float betweenTipsIndexMiddle;
			public float betweenTipsMiddleRing;
			public float betweenTipsRingPinky;
			public float indexFirstJoint;
			public float indexTotal;
			public float middleFirstJoint;
			public float middleTotal;
			public float ringFirstJoint;
			public float ringTotal;
			public float pinkyFirstJoint;
			public float pinkyTotal;
			public float wristPointsForward;
			public float wristPointsUp;
			public float palmFaceRight;
			public float palmPointsUp;
            public float palmFaceCamera;

			public Pose(float[] values)
			{
				thumbToIndex = values[0];
				thumbToMiddle = values[1];
				thumbToRing = values[2];
				thumbToPinky = values[3];
				betweenTipsIndexMiddle = values[4];
				betweenTipsMiddleRing = values[5];
				betweenTipsRingPinky = values[6];
				indexFirstJoint = values[7];
				indexTotal = values[8];
				middleFirstJoint = values[9];
				middleTotal = values[10];
				ringFirstJoint = values[11];
				ringTotal = values[12];
				pinkyFirstJoint = values[13];
				pinkyTotal = values[14];
				wristPointsForward = values[15];
				wristPointsUp = values[16];
				palmFaceRight = values[17];
				palmPointsUp = values[18];
				palmFaceCamera = values[19];
			}

			public static string[] ValueLabels = new string[] { 
				"_thumbToIndex", 
				"_thumbToMiddle",
				"_thumbToRing",
				"_thumbToPinky",
				"_betweenTipsIndexMiddle",
				"_betweenTipsMiddleRing",
				"_betweenTipsRingPinky",
				"_indexFirstJoint",
				"_indexTotal",
				"_middleFirstJoint",
				"_middleTotal",
				"_ringFirstJoint",
				"_ringTotal",
				"_pinkyFirstJoint",
				"_pinkyTotal",
				"_wristPointsForward",
				"_wristPointsUp",
				"_palmFaceRight",
				"_palmPointsUp",
				"_palmFaceCamera"
			};

			public Pose(float _thumbToIndex = 0, float _thumbToMiddle = 0, float _thumbToRing = 0, float _thumbToPinky = 0, float _betweenTipsIndexMiddle = 0,
				float _betweenTipsMiddleRing = 0, float _betweenTipsRingPinky = 0, float _indexFirstJoint = 0, float _indexTotal = 0,
				float _middleFirstJoint = 0, float _middleTotal = 0, float _ringFirstJoint = 0, float _ringTotal = 0,
				float _pinkyFirstJoint = 0, float _pinkyTotal = 0, float _wristPointsForward = 0, float _wristPointsUp = 0,
				float _palmFaceRight = 0, float _palmPointsUp = 0, float _palmFaceCamera = 0)
			{
				thumbToIndex = _thumbToIndex;
				thumbToMiddle = _thumbToMiddle;
				thumbToRing = _thumbToRing;
				thumbToPinky = _thumbToPinky;
				betweenTipsIndexMiddle = _betweenTipsIndexMiddle;
				betweenTipsMiddleRing = _betweenTipsMiddleRing;
				betweenTipsRingPinky = _betweenTipsRingPinky;
				indexFirstJoint = _indexFirstJoint;
				indexTotal = _indexTotal;
				middleFirstJoint = _middleFirstJoint;
				middleTotal = _middleTotal;
				ringFirstJoint = _ringFirstJoint;
				ringTotal = _ringTotal;
				pinkyFirstJoint = _pinkyFirstJoint;
				pinkyTotal = _pinkyTotal;
				wristPointsForward = _wristPointsForward;
				wristPointsUp = _wristPointsUp;
				palmFaceRight = _palmFaceRight;
				palmPointsUp = _palmPointsUp;
				palmFaceCamera = _palmFaceCamera;
			}



			public float[] Values => new float[] {
				thumbToIndex,
				thumbToMiddle,
				thumbToRing,
				thumbToPinky,
				betweenTipsIndexMiddle,
				betweenTipsMiddleRing,
				betweenTipsRingPinky,
				indexFirstJoint,
				indexTotal,
				middleFirstJoint,
				middleTotal,
				ringFirstJoint,
				ringTotal,
				pinkyFirstJoint,
				pinkyTotal,
				wristPointsForward,
				wristPointsUp,
				palmFaceRight,
				palmPointsUp,
				palmFaceCamera
			};
		}

	}
}
