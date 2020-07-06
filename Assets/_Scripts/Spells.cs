using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Elemento.AudioPlayer;

namespace Elemento
{
	using Pose = PoseData.Pose;
	public static class Spells
	{
		public static Pose fireStartP = new Pose(0.05f, 0.05f, 0.06f, 0.07f, 0.02f, 0.02f, 0.02f, -68f, -250f, -77f, -245f, -81f, -249f, -76f, -242f, 79.33f, 22.28f, 89.5f, 109.91f);
		//public static Pose fireEndP = new Pose(0.09f, 0.11f, 0.11f, 0.11f, 0.02f, 0.02f, 0.03f, 8f, 10f, 3f, 10f, 6f, 8f, 5f, -1f, 85.09f, 13.75f, 77.07f, 78.37f);
		public static Pose fireEndP = new Pose(0.09f, 0.11f, 0.11f, 0.1f, 0.02f, 0.02f, 0.03f, 5f, 8f, 3f, 6f, 5f, 5f, 3f, -1f, 74.99f, 15.1f, 73.74f, 82.18f);
		public static Pose tolerance_01 = new Pose(
			_thumbToIndex: 0.02f,
			_thumbToMiddle: 0.02f,
			_thumbToRing: 0.03f,
			_thumbToPinky: 0.02f,
			_betweenTipsIndexMiddle: 0.02f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.02f,
			_indexFirstJoint: 34f,
			_indexTotal: 128f,
			_middleFirstJoint: 36f,
			_middleTotal: 125f,
			_ringFirstJoint: 45f,
			_ringTotal: 121f,
			_pinkyFirstJoint: 50f,
			_pinkyTotal: 129f,
			_wristPointsForward: 34.36f,
			_wristPointsUp: 45.01f,
			_palmFaceRight: 31.9f,
			_palmPointsUp: 34.09f
			);
		public static Pose teleportStartP = new Pose(0.11f, 0.01f, 0.07f, 0.08f, 0.11f, 0.07f, 0.01f, -10f, -10f, -67f, -117f, -77f, -223f, -75f, -219f, 87.78f, 11.59f, 97.96f, 94.14f);
		public static Pose teleportEndP = new Pose(0.12f, 0.08f, 0.09f, 0.1f, 0.12f, 0.02f, 0.01f, -14f, -7f, -78f, -247f, -87f, -235f, -90f, -227f, 81.48f, 13.98f, 91.7f, 92.43f);

		//public static Pose brandonTeleportStartP = new Pose(, , ,, 0.01f, 0.01f, 0f, -1f, -50f, -128f, -53f, -138f, -44f, -123f, 110.84f, 8.72f, 83.5f, 84.06f);
		public static Pose brandonTeleportStartP =
			new Pose(
				_thumbToIndex: 0.13f,
				_thumbToMiddle: 0f,
				_thumbToRing: 0.01f,
				_thumbToPinky: 0.12f,
				_betweenTipsIndexMiddle: 0.13f,
				_betweenTipsMiddleRing: 0.01f,
				_betweenTipsRingPinky: 0.11f,
				_indexFirstJoint: -7f,
				_indexTotal: 1f,
				_middleFirstJoint: -73f,
				_middleTotal: -119f,
				_ringFirstJoint: -68f,
				_ringTotal: -123f,
				_pinkyFirstJoint: -3f,
				_pinkyTotal: -8f,
				_wristPointsForward: 83.34f,
				_wristPointsUp: 10.73f,
				_palmFaceRight: 86.22f,
				_palmPointsUp: 86.17f


			);

					
		 

		public static Pose brandonTeleportEndP =
			new Pose(
				_thumbToIndex: 0.11f,
				_thumbToMiddle: 0.08f,
				_thumbToRing: 0.09f,
				_thumbToPinky: 0.1f,
				_betweenTipsIndexMiddle: 0.12f,
				_betweenTipsMiddleRing: 0.02f,
				_betweenTipsRingPinky: 0.01f,
				_indexFirstJoint: -23f,
				_indexTotal: -15f,
				_middleFirstJoint: -82f,
				_middleTotal: -250f,
				_ringFirstJoint: -92f,
				_ringTotal: -240f,
				_pinkyFirstJoint: -97f,
				_pinkyTotal: -229f,
				_wristPointsForward: 78.72f,
				_wristPointsUp: 16.11f,
				_palmFaceRight: 94.61f,
				_palmPointsUp: 96.68f
			);
		public static Pose teleportTolerance = new Pose(
			_thumbToIndex			: 0.05f, 
			_thumbToMiddle			: 0.04f, 
			_thumbToRing			: 1,
			_thumbToPinky			: 1,
			_betweenTipsIndexMiddle	: 0.04f,
			_betweenTipsMiddleRing	: 1,
			_betweenTipsRingPinky	: 1,
			_indexFirstJoint		: 34f,
			_indexTotal				: 28f,
			_middleFirstJoint		: 66f,
			_middleTotal			: 75f,
			_ringFirstJoint			: 1000f,
			_ringTotal				: 1000f,
			_pinkyFirstJoint		: 1000f,
			_pinkyTotal				: 1000f,
			_wristPointsForward		: 34.36f,
			_wristPointsUp			: 45.01f,
			_palmFaceRight			: 31.90f,
			_palmPointsUp			: 34.09f
			);

		public static Pose forcePushStart = new Pose(
			_thumbToIndex: 0.05f,
			_thumbToMiddle: 0.05f,
			_thumbToRing: 0.07f,
			_thumbToPinky: 0.07f,
			_betweenTipsIndexMiddle: 0.02f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.01f,
			_indexFirstJoint: -72f,
			_indexTotal: -234f,
			_middleFirstJoint: -77f,
			_middleTotal: -228f,
			_ringFirstJoint: -83f,
			_ringTotal: -238f,
			_pinkyFirstJoint: -81f,
			_pinkyTotal: -240f,
			_wristPointsForward: 80.22f,
			_wristPointsUp: 14.8f,
			_palmFaceRight: 93.19f,
			_palmPointsUp: 104.42f
		);

		public static Pose forcePushEnd = new Pose(
			_thumbToIndex: 0.1f,
			_thumbToMiddle: 0.13f,
			_thumbToRing: 0.13f,
			_thumbToPinky: 0.13f,
			_betweenTipsIndexMiddle: 0.03f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.03f,
			_indexFirstJoint: 12f,
			_indexTotal: 19f,
			_middleFirstJoint: 8f,
			_middleTotal: 16f,
			_ringFirstJoint: 10f,
			_ringTotal: 12f,
			_pinkyFirstJoint: 13f,
			_pinkyTotal: 4f,
			_wristPointsForward: 84.28f,
			_wristPointsUp: 16.48f,
			_palmFaceRight: 74.22f,
			_palmPointsUp: 78.38f
		);


		public static Pose forceMoveStart = new Pose(
			_thumbToIndex: 0.1f,
			_thumbToMiddle: 0.14f,
			_thumbToRing: 0.14f,
			_thumbToPinky: 0.15f,
			_betweenTipsIndexMiddle: 0.06f,
			_betweenTipsMiddleRing: 0.04f,
			_betweenTipsRingPinky: 0.04f,
			_indexFirstJoint: -23f,
			_indexTotal: -47f,
			_middleFirstJoint: -15f,
			_middleTotal: -47f,
			_ringFirstJoint: -19f,
			_ringTotal: -67f,
			_pinkyFirstJoint: -16f,
			_pinkyTotal: -56f,
			_wristPointsForward: 48.48f,
			_wristPointsUp: 48.4f,
			_palmFaceRight: 78.61f,
			_palmPointsUp: 138.08f
			);
		public static Pose forceMoveGrab = new Pose(
			_thumbToIndex: 0.05f,
			_thumbToMiddle: 0.05f,
			_thumbToRing: 0.06f,
			_thumbToPinky: 0.06f,
			_betweenTipsIndexMiddle: 0.02f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.01f,
			_indexFirstJoint: -64f,
			_indexTotal: -245f,
			_middleFirstJoint: -65f,
			_middleTotal: -232f,
			_ringFirstJoint: -70f,
			_ringTotal: -249f,
			_pinkyFirstJoint: -58f,
			_pinkyTotal: -239f,
			_wristPointsForward: 50.43f,
			_wristPointsUp: 44.91f,
			_palmFaceRight: 80.66f,
			_palmPointsUp: 133.25f
			);
		private static Pose forceMoveTolerance = new Pose(
			_thumbToIndex: 0.03f,
			_thumbToMiddle: 0.03f,
			_thumbToRing: 0.03f,
			_thumbToPinky: 0.03f,
			_betweenTipsIndexMiddle: 0.03f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.02f,
			_indexFirstJoint: 45f,
			_indexTotal: 150f,
			_middleFirstJoint: 45f,
			_middleTotal: 150f,
			_ringFirstJoint: 45f,
			_ringTotal: 150f,
			_pinkyFirstJoint: 50f,
			_pinkyTotal: 160f,
			_wristPointsForward: 50.36f,
			_wristPointsUp: 50.01f,
			_palmFaceRight: 50.9f,
			_palmPointsUp: 50.09f
			);

		public static Pose brandonFireStart = new Pose(0.05f, 0.07f, 0.08f, 0.09f, 0.02f, 0.02f, 0.02f, -80f, -189f, -88f, -190f, -94f, -202f, -89f, -212f, 84.79f, 13.9f, 93.28f, 103.49f);
		public static Pose brandonFireEnd = new Pose(0.07f, 0.09f, 0.09f, 0.09f, 0.02f, 0.02f, 0.03f, 4f, 5f, -2f, 4f, 4f, 2f, 15f, -8f, 86.57f, 16.29f, 82.54f, 76.17f);
		public static Pose brandonFireEnd2 = new Pose(0.07f, 0.08f, 0.09f, 0.09f, 0.02f, 0.02f, 0.03f, 3f, 3f, -4f, 0f, 2f, 0f, 17f, -10f, 91.29f, 13.22f, 82.39f, 79.49f);


		public class PoseSequenceItem
		{
			public float time;
			public Pose pose;
			public Pose tolerance;
			public Action action;
			public List<TonePosePair> tones;
			public PoseSequenceItem(float _time, Pose _pose, Pose _tolerance, Action _action = null, List<TonePosePair> _tones = null)
			{
				time = _time;
				pose = _pose;
				tolerance = _tolerance;
				action = _action;
				tones = _tones;
			}
			public PoseSequenceItem(float _time, Pose _pose)
			{
				time = _time;
				pose = _pose;
				tolerance = tolerance_01;
			}
		}

		public class Spell
		{
			public HandPoseTracker castingHand; // changes when spell is cast
			public string name = "Spell1";
			public List<PoseSequenceItem> sequence = new List<PoseSequenceItem>();
			public Action canceled;
			public Spell(string _name, List<PoseSequenceItem> _sequence, Action _canceled = null)
			{
				name = _name;
				sequence = _sequence;
				canceled = _canceled;
			}
		}

		static Spell forcePush
		{
			get
			{
				Action forcePushAction = () =>
				{
					Physics.SphereCastAll(Camera.main.transform.position, 1f, Camera.main.transform.forward, 15f).ToList()
						.Where(x => x.collider.GetComponent<Rigidbody>() != null).ToList()
						.ForEach(x => x.collider.GetComponent<Rigidbody>()?.AddForce(Camera.main.transform.forward * 500f));
				};
				return new Spell
				(
					"Push",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem( 0, brandonFireStart ),
						new PoseSequenceItem( .5f, brandonFireEnd, tolerance_01, forcePushAction )
					}
				
				);
			}
		}

		static Spell teleport
		{
			get
			{
				Action activateTeleportFn = () => Teleport.SetStateStatic(Teleport.State.ShowLine);
				Action executeTeleportFn = () => Teleport.TeleportNowStatic();
				Action cancelAction = () => Teleport.CancelStatic();

				var poseTones = new List<TonePosePair>
				{
					new TonePosePair(Tone.A, 0, .01f), // thumb to index
					new TonePosePair(Tone.B, 1, .01f), // thumb to middle
				};
				return new Spell
				(
					"Teleport",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem( 0, teleportStartP, teleportTolerance, activateTeleportFn, poseTones ),
						new PoseSequenceItem( .5f, teleportEndP, teleportTolerance, executeTeleportFn ),
					},
					cancelAction


				);
			}
		}

		static Spell BrandonTeleport
		{
			get
			{
				Action a1 = () => Teleport.SetStateStatic(Teleport.State.ShowLine);
				Action a2 = () => Teleport.TeleportNowStatic();
				Action cancelAction = () => Teleport.CancelStatic();
				var poseTones = new List<TonePosePair>
				{
					new TonePosePair(Tone.A, 0, .05f), // thumb to index
					new TonePosePair(Tone.B, 1, .05f), // thumb to middle
				};
				return new Spell
				(
					"Teleport",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem( 0, brandonTeleportStartP, teleportTolerance, a1, poseTones ),
						new PoseSequenceItem( .5f, brandonTeleportEndP, teleportTolerance, a2 ),
					},
					cancelAction


				);
			}
		}

		public static Spell ForceMove
		{
			get
			{
				Action selectAndHighlightCubes = () => { SpellForceMove.Instance.SetState(SpellForceMove.State.SelectAndHighlight); };
				Action beingMovingCubes = () => SpellForceMove.Instance.SetState(SpellForceMove.State.Moving);
				Action cancelAction = () => SpellForceMove.Instance.SetState(SpellForceMove.State.Ready);
				return new Spell
				(
					"Force Move",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem(0, forceMoveStart, forceMoveTolerance, selectAndHighlightCubes),
						new PoseSequenceItem(30, forceMoveGrab, forceMoveTolerance, beingMovingCubes),

					},
					cancelAction

				);
			}
		}



		public static List<Spell> spells = new List<Spell> { forcePush,  BrandonTeleport, ForceMove };


		// This tone pose pair indicators allow tones to manifest when certain pose metrics get within certain bounds.
		public class TonePosePair
		{
			public Tone tone;
			public int index; // corresponds to Pose.Values index, for example index = 0 is ThumbToIndex
			public float threshold;
			public TonePosePair(Tone _tone, int _index, float _threshold)
			{
				tone = _tone;
				index = _index;
				threshold = _threshold;
			}
		}



	}



}
