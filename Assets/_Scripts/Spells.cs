using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Elemento.AudioPlayer;

namespace Elemento
{
	using Pose = PoseData.Pose;
    using Random = UnityEngine.Random;

    public static class Spells
	{
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
		private static Pose forceMoveGrabTolerance = new Pose(
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
			_wristPointsForward: 999,
			_wristPointsUp: 999,
			_palmFaceRight: 999,
			_palmPointsUp: 999
			);

		public static Pose forcePushStart = new Pose(0.05f, 0.07f, 0.08f, 0.09f, 0.02f, 0.02f, 0.02f, -80f, -189f, -88f, -190f, -94f, -202f, -89f, -212f, 84.79f, 13.9f, 93.28f, 103.49f);
		public static Pose forcePushEnd = new Pose(0.07f, 0.09f, 0.09f, 0.09f, 0.02f, 0.02f, 0.03f, 4f, 5f, -2f, 4f, 4f, 2f, 15f, -8f, 86.57f, 16.29f, 82.54f, 76.17f);

		
		public static Pose pullMatterFromGroundStart = new Pose(
			_thumbToIndex: 0.1f,
			_thumbToMiddle: 0.13f,
			_thumbToRing: 0.14f,
			_thumbToPinky: 0.16f,
			_betweenTipsIndexMiddle: 0.03f,
			_betweenTipsMiddleRing: 0.03f,
			_betweenTipsRingPinky: 0.05f,
			_indexFirstJoint: -3f,
			_indexTotal: -7f,
			_middleFirstJoint: -12f,
			_middleTotal: -16f,
			_ringFirstJoint: -4f,
			_ringTotal: -14f,
			_pinkyFirstJoint: 12f,
			_pinkyTotal: -7f,
			_wristPointsForward: 41.76f,
			_wristPointsUp: 63.68f,
			_palmFaceRight: 71.26f,
			_palmPointsUp: 27.88f);

		public static Pose pullMatterFromGroundTwo = new Pose(
			_thumbToIndex: 0.04f,
			_thumbToMiddle: 0.05f,
			_thumbToRing: 0.06f,
			_thumbToPinky: 0.07f,
			_betweenTipsIndexMiddle: 0.02f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.02f,
			_indexFirstJoint: -49f,
			_indexTotal: -213f,
			_middleFirstJoint: -62f,
			_middleTotal: -220f,
			_ringFirstJoint: -76f,
			_ringTotal: -231f,
			_pinkyFirstJoint: -77f,
			_pinkyTotal: -227f,
			_wristPointsForward: 51.96f,
			_wristPointsUp: 58.65f,
			_palmFaceRight: 62.42f,
			_palmPointsUp: 36.01f
			);

		public static Pose pullMatterFromGroundEnd = new Pose(
			_thumbToIndex: 0.05f,
			_thumbToMiddle: 0.05f,
			_thumbToRing: 0.06f,
			_thumbToPinky: 0.07f,
			_betweenTipsIndexMiddle: 0.02f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.02f,
			_indexFirstJoint: -64f,
			_indexTotal: -245f,
			_middleFirstJoint: -73f,
			_middleTotal: -245f,
			_ringFirstJoint: -77f,
			_ringTotal: -244f,
			_pinkyFirstJoint: -73f,
			_pinkyTotal: -243f,
			_wristPointsForward: 110.7f,
			_wristPointsUp: 16.59f,
			_palmFaceRight: 71.17f,
			_palmPointsUp: 93.01f);

		public class PoseSequenceItem
		{
			public float time;
			public Pose pose;
			public Pose tolerance;
			public Action action;
			public List<TonePosePair> tones;
			public bool breakPoseCancels;
			public PoseSequenceItem(float _time, Pose _pose, Pose _tolerance, Action _action = null, bool _breakPoseCancels = false, List<TonePosePair> _tones = null)
			{
				time = _time;
				pose = _pose;
				tolerance = _tolerance;
				action = _action;
				tones = _tones;
				breakPoseCancels = _breakPoseCancels;
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
						new PoseSequenceItem( 0, forcePushStart ),
						new PoseSequenceItem( .5f, forcePushEnd, tolerance_01, forcePushAction )
					}
				
				);
			}
		}


		static Spell Teleport
		{
			get
			{
                Action a1 = () => global::Teleport.SetStateStatic(global::Teleport.State.ShowLine);
                Action a2 = () => global::Teleport.TeleportNowStatic();
                Action cancelAction = () => global::Teleport.CancelStatic();
				//var poseTones = new List<TonePosePair>
				//{
				//	new TonePosePair(Tone.A, 0, .05f), // thumb to index
				//	new TonePosePair(Tone.B, 1, .05f), // thumb to middle
				//};
				return new Spell
				(
					"Teleport",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem( 3, brandonTeleportStartP, teleportTolerance, a1 ),
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
				Action beginMoveingCubes = () => SpellForceMove.Instance.SetState(SpellForceMove.State.Moving);
				Action cancelAction = () => SpellForceMove.Instance.SetState(SpellForceMove.State.Ready);
				return new Spell
				(
					"Force Move",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem(3, forceMoveStart, forceMoveTolerance, selectAndHighlightCubes),
						new PoseSequenceItem(30, forceMoveGrab, forceMoveGrabTolerance, beginMoveingCubes, true), // if you break this pose, it cancels.
						//new PoseSequenceItem(0, forceMoveStart, forceMoveTolerance, cancelAction),
					},
					cancelAction

				);
			}
		}

		public static Spell CubeFountain
		{
			get
			{
				Action cubeFountain = () => 
				{ 
					for (var i = 0; i < 20; i++) 
					{ 
						var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
						c.transform.localScale *= 0.4f;
						c.AddComponent<Rigidbody>(); 
						c.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 4f + Random.insideUnitSphere * 3.5f; 
						c.GetComponent<Rigidbody>().AddForce(Vector3.up + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * 100f);
						c.GetComponent<Renderer>().material.color = Utils2.RandomColor;
					} 
				};
				return new Spell
				(
                    "Cube Fountain",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem(2, pullMatterFromGroundStart, tolerance_01),
						new PoseSequenceItem(2, pullMatterFromGroundTwo, tolerance_01),
						new PoseSequenceItem(2, pullMatterFromGroundEnd, tolerance_01, cubeFountain)
					}
					
				);
			}
		}



		public static List<Spell> spells = new List<Spell> { forcePush,  Teleport, ForceMove, CubeFountain };


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
