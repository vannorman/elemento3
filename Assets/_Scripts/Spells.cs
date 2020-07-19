﻿using OVRTouchSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.Oculus;
using UnityEngine;
using static Elemento.AudioPlayer;

namespace Elemento
{
	using Pose = PoseData.Pose;
    using Random = UnityEngine.Random;

	// Current issue is that one spell is easy to "start" and then it lingers around while I'd rather have a different spell? and the "move cubes" doesn't care which way my hand is flipped, too insensitve for start.

    public static class Spells
	{


		public static Pose pointForwardWalk = new Pose(
			_thumbToIndex: 0.1f,
			_thumbToMiddle: 0.06f,
			_thumbToRing: 0.08f,
			_thumbToPinky: 0.09f,
			_betweenTipsIndexMiddle: 0.11f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.02f,
			_indexFirstJoint: -15f,
			_indexTotal: -16f,
			_middleFirstJoint: -64f,
			_middleTotal: -228f,
			_ringFirstJoint: -81f,
			_ringTotal: -238f,
			_pinkyFirstJoint: -87f,
			_pinkyTotal: -233f,
			_wristPointsForward: 57.12f,
			_wristPointsUp: 105.35f,
			_palmFaceRight: 54.87f,
			_palmPointsUp: 15.64f,
			_palmFaceCamera: 106.7f
		);

		public static Pose pointForwardWalkTolerance = new Pose(
			_thumbToIndex: 0.06f,
			_thumbToMiddle: 0.04f,
			_thumbToRing: 0.04f,
			_thumbToPinky: 0.04f,
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
			_wristPointsUp: 150.01f,
			_palmFaceRight: 150f,
			_palmPointsUp: 180.09f,
			_palmFaceCamera: 999
			);

		public static Pose laserEyeStartA = new Pose(
				_thumbToIndex: 0.06f,
				_thumbToMiddle: 0.15f,
				_thumbToRing: 0.17f,
				_thumbToPinky: 0.18f,
				_betweenTipsIndexMiddle: 0.09f,
				_betweenTipsMiddleRing: 0.04f,
				_betweenTipsRingPinky: 0.05f,
				_indexFirstJoint: -54f,
				_indexTotal: -72f,
				_middleFirstJoint: -11f,
				_middleTotal: -18f,
				_ringFirstJoint: 10f,
				_ringTotal: -3f,
				_pinkyFirstJoint: 23f,
				_pinkyTotal: 9f,
				_wristPointsForward: 117.89f,
				_wristPointsUp: 19.53f,
				_palmFaceRight: 67.58f,
				_palmPointsUp: 73.51f,
				_palmFaceCamera: 100.56f
			);
		public static Pose laserEyeStartB = new Pose(
			_thumbToIndex: 0.05f,
			_thumbToMiddle: 0.12f,
			_thumbToRing: 0.14f,
			_thumbToPinky: 0.15f,
			_betweenTipsIndexMiddle: 0.07f,
			_betweenTipsMiddleRing: 0.03f,
			_betweenTipsRingPinky: 0.05f,
			_indexFirstJoint: -57f,
			_indexTotal: -67f,
			_middleFirstJoint: -19f,
			_middleTotal: -35f,
			_ringFirstJoint: -6f,
			_ringTotal: -37f,
			_pinkyFirstJoint: 12f,
			_pinkyTotal: -20f,
			_wristPointsForward: 112.49f,
			_wristPointsUp: 27.88f,
			_palmFaceRight: 115.18f,
			_palmPointsUp: 114.73f,
			_palmFaceCamera: 95.95f

		);
		public static Pose laserEyeStartC = new Pose(
			_thumbToIndex: 0.1f,
			_thumbToMiddle: 0.17f,
			_thumbToRing: 0.18f,
			_thumbToPinky: 0.17f,
			_betweenTipsIndexMiddle: 0.08f,
			_betweenTipsMiddleRing: 0.04f,
			_betweenTipsRingPinky: 0.06f,
			_indexFirstJoint: -36f,
			_indexTotal: -39f,
			_middleFirstJoint: -4f,
			_middleTotal: -4f,
			_ringFirstJoint: 6f,
			_ringTotal: 0f,
			_pinkyFirstJoint: 15f,
			_pinkyTotal: -7f,
			_wristPointsForward: 94.96f,
			_wristPointsUp: 61.48f,
			_palmFaceRight: 35.75f,
			_palmPointsUp: 30.12f,
			_palmFaceCamera: 111.18f
		);
		public static Pose laserEyesTolerance = new Pose(
			_thumbToIndex: 0.06f,
			_thumbToMiddle: 0.04f,
			_thumbToRing: 0.04f,
			_thumbToPinky: 0.04f,
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
			_palmPointsUp: 34.09f,
			_palmFaceCamera: 999
			);
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
			_palmPointsUp: 34.09f,
			_palmFaceCamera: 999
			);
		

		public static Pose teleportStart =
			new Pose(
				_thumbToIndex: 0.13f,
				_thumbToMiddle: 0f,
				_thumbToRing: 0.01f,
				_thumbToPinky: 0.12f,
				_betweenTipsIndexMiddle: 0.10f,
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
				_palmPointsUp: 86.17f,
				_palmFaceCamera: 120
			);

					
		 

		public static Pose teleportExecute =
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
				_palmPointsUp: 96.68f,
				_palmFaceCamera: 120

			);
		public static Pose teleportTolerance = new Pose(
			_thumbToIndex			: 0.07f, 
			_thumbToMiddle			: 0.04f, 
			_thumbToRing			: 1,
			_thumbToPinky			: 1,
			_betweenTipsIndexMiddle	: 0.05f,
			_betweenTipsMiddleRing	: 1,
			_betweenTipsRingPinky	: 1,
			_indexFirstJoint		: 34f,
			_indexTotal				: 60f,
			_middleFirstJoint		: 66f,
			_middleTotal			: 75f,
			_ringFirstJoint			: 1000f,
			_ringTotal				: 1000f,
			_pinkyFirstJoint		: 1000f,
			_pinkyTotal				: 1000f,
			_wristPointsForward		: 999.36f,
			_wristPointsUp			: 45.01f,
			_palmFaceRight			: 31.90f,
			_palmPointsUp			: 34.09f,
			_palmFaceCamera			: 999

			);

		

		


		public static Pose forceMoveStart = new Pose(
			_thumbToIndex: 0.2f,
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
			_palmPointsUp: 138.08f,
			_palmFaceCamera: 75
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
			_palmPointsUp: 133.25f,
			_palmFaceCamera: 75
			);
		private static Pose forceMoveStartTolerance = new Pose(
			_thumbToIndex: 0.03f,
			_thumbToMiddle: 0.03f,
			_thumbToRing: 0.03f,
			_thumbToPinky: 0.03f,
			_betweenTipsIndexMiddle: 0.04f,
			_betweenTipsMiddleRing: 0.03f,
			_betweenTipsRingPinky: 0.03f,
			_indexFirstJoint: 45f,
			_indexTotal: 150f,
			_middleFirstJoint: 45f,
			_middleTotal: 150f,
			_ringFirstJoint: 45f,
			_ringTotal: 150f,
			_pinkyFirstJoint: 50f,
			_pinkyTotal: 160f,
			_wristPointsForward: 90.36f,
			_wristPointsUp: 90.01f,
			_palmFaceRight: 90.9f,
			_palmPointsUp: 90.09f,
			_palmFaceCamera: 999
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
			_palmPointsUp: 999,
			_palmFaceCamera: 999
			);

		public static Pose forcePushStart = new Pose(
			_thumbToIndex: 0.05f,
			_thumbToMiddle: 0.07f,
			_thumbToRing: 0.08f,
			_thumbToPinky: 0.09f,
			_betweenTipsIndexMiddle: 0.02f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.02f,
			_indexFirstJoint: -80f,
			_indexTotal: -189f,
			_middleFirstJoint: -88f,
			_middleTotal: -190f,
			_ringFirstJoint: -94f,
			_ringTotal: -202f,
			_pinkyFirstJoint: -89f,
			_pinkyTotal: -212f,
			_wristPointsForward: 84.79f,
			_wristPointsUp: 13.9f,
			_palmFaceRight: 93.28f,
			_palmPointsUp: 103.49f,
			_palmFaceCamera: 30f
			);
		public static Pose forcePushEnd = new Pose(
			_thumbToIndex: 0.07f,
			_thumbToMiddle: 0.09f,
			_thumbToRing: 0.09f,
			_thumbToPinky: 0.09f,
			_betweenTipsIndexMiddle: 0.02f,
			_betweenTipsMiddleRing: 0.02f,
			_betweenTipsRingPinky: 0.03f,
			_indexFirstJoint: 4f,
			_indexTotal: 5f,
			_middleFirstJoint: -2f,
			_middleTotal: 4f,
			_ringFirstJoint: 4f,
			_ringTotal: 2f,
			_pinkyFirstJoint: 15f,
			_pinkyTotal: -8f,
			_wristPointsForward: 86.57f,
			_wristPointsUp: 16.29f,
			_palmFaceRight: 82.54f,
			_palmPointsUp: 76.17f,
			_palmFaceCamera: 140f
			);
		public static Pose forcePushTolerance = new Pose(
			_thumbToIndex: 0.03f,
			_thumbToMiddle: 0.03f,
			_thumbToRing: 0.03f,
			_thumbToPinky: 0.03f,
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
			_wristPointsForward: 34,
			_wristPointsUp: 90,
			_palmFaceRight: 32,
			_palmPointsUp: 90,
			_palmFaceCamera: 45
			);
		
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
			public Pose pose => poses.FirstOrDefault();
			public List<Pose> poses;
			public Pose tolerance;
			public Action<HandPoseTracker> action; // need to pass "which" hand was doing this sequenec.
			public List<TonePosePair> tones;
			public bool breakPoseCancels;
			public bool finalPoseCancels;
			public HandRequirement handRequirement;
			public enum HandRequirement
			{ 
				OneHanded,
				TwoHanded // for now we will simply take the first two poses from "poses" var to be "left" and "right" requirements
			}
			public enum CancelOptions
			{ 
				None,
				BreakPoseCancels,
				AcquirePoseCancels,
				BreakOrAcquirePoseCancels
			}
			public CancelOptions poseCancelType;

			public enum InvokeOptions
			{ 
				OnceOnPoseAcquired,
				WhilePoseMaintained
			}
			public InvokeOptions invokeOptions;
			public PoseSequenceItem(float _time, List<Pose> _poses, Pose _tolerance, Action<HandPoseTracker> _action = null, CancelOptions _poseCancelType = CancelOptions.None, InvokeOptions _invokeOptions = InvokeOptions.OnceOnPoseAcquired, HandRequirement _handRequirement = HandRequirement.OneHanded, List<TonePosePair> _tones = null)
			{
				time = _time;
				poses = _poses;
				tolerance = _tolerance;
				action = _action;
				tones = _tones;
				poseCancelType = _poseCancelType;
				invokeOptions = _invokeOptions;
			}
			public PoseSequenceItem(float _time, Pose _pose, Pose _tolerance, Action<HandPoseTracker> _action = null, CancelOptions _poseCancelType = CancelOptions.None, InvokeOptions _invokeOptions = InvokeOptions.OnceOnPoseAcquired, HandRequirement _handRequirement = HandRequirement.OneHanded, List<TonePosePair> _tones = null)
			{
				time = _time;
				poses = new List<Pose>() { _pose };
				tolerance = _tolerance;
				action = _action;
				tones = _tones;
				poseCancelType = _poseCancelType;
				invokeOptions = _invokeOptions;

			}
			public PoseSequenceItem(float _time, Pose _pose)
			{
				time = _time;
				poses = new List<Pose>() { _pose };
				tolerance = tolerance_01;
			}
		}

		public class Spell
		{
			public HandPoseTracker castingHand; // changes when spell is cast
			public string name = "Spell1";
			public List<PoseSequenceItem> sequence = new List<PoseSequenceItem>();
			public Action<HandPoseTracker> cancelAction;
			public Spell(string _name, List<PoseSequenceItem> _sequence, Action<HandPoseTracker> _canceled = null)
			{
				name = _name;
				sequence = _sequence;
				cancelAction = _canceled;
			}
		}

		static Spell forcePush
		{
			get
			{
				Action<HandPoseTracker> forcePushAction = (HandPoseTracker handTracker) =>
				{
					Physics.SphereCastAll(handTracker.hand.position, 1f, handTracker.PalmForwardDirection, 15f).ToList()
						.ForEach(x =>
						{
							var rb = x.collider.GetComponent<Rigidbody>();
							if (rb)
							{
								if (rb.isKinematic && rb.GetComponent<AllowKinematicToggle>())
								{
									rb.isKinematic = false;
								}
								if (!rb.isKinematic)
                                {
									rb.AddForce(Utils2.FlattenVector(handTracker.PalmForwardDirection) * 500f);
                                }
							}
							var ai = x.collider.GetComponent<AICharacter>();
							if (ai) ai.RunAway();
						});
					
					Utils2.SpellDebug("Force push");
                    Utils2.DebugSphere(handTracker.hand.position, 0.1f, Color.blue, 0.5f);
                    Utils2.DebugSphere(handTracker.hand.position + handTracker.PalmForwardDirection * .1f, 0.1f, Color.blue, 0.5f);
					Utils2.DebugSphere(handTracker.hand.position + handTracker.PalmForwardDirection * .2f, 0.1f, Color.blue, 0.5f);
					Utils2.DebugSphere(handTracker.hand.position + handTracker.PalmForwardDirection * .3f, 0.2f, Color.green, 0.5f);

				};
				return new Spell
				(
					"Push",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem( 0.5f, forcePushStart, forcePushTolerance ),
						new PoseSequenceItem( 0f, forcePushEnd, forcePushTolerance, forcePushAction, PoseSequenceItem.CancelOptions.AcquirePoseCancels )
					}
				
				);
			}
		}


		static Spell TeleportSpell
		{
			get
			{
                Action<HandPoseTracker> a1 = (HandPoseTracker currentHand) => Teleport.SetStateStatic(global::Teleport.State.ShowLine);
                Action<HandPoseTracker> a2 = (HandPoseTracker currentHand) => Teleport.TeleportNowStatic();
                Action<HandPoseTracker> cancelAction = (HandPoseTracker currentHand) => Teleport.CancelStatic();
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
						new PoseSequenceItem( 0.3f, teleportStart, teleportTolerance, a1 ),
						new PoseSequenceItem( 0.5f, teleportExecute, teleportTolerance, a2, PoseSequenceItem.CancelOptions.AcquirePoseCancels ),
					},
					cancelAction


				);
			}
		}

		public static Spell ForceMove
		{
			get
			{
				Action<HandPoseTracker> selectAndHighlightCubes = (HandPoseTracker currentHand) => { SpellForceMove.Instance.SetState(SpellForceMove.State.SelectAndHighlight, currentHand); };
				Action<HandPoseTracker> beginMoveingCubes = (HandPoseTracker currentHand) => SpellForceMove.Instance.SetState(SpellForceMove.State.Moving, currentHand);
				Action<HandPoseTracker> cancelAction = (HandPoseTracker currentHand) => SpellForceMove.Instance.Cancel();
				return new Spell
				(
					"Force Move",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem(1, forceMoveStart, forceMoveStartTolerance, selectAndHighlightCubes),
						new PoseSequenceItem(7, forceMoveGrab, forceMoveGrabTolerance, beginMoveingCubes, PoseSequenceItem.CancelOptions.BreakPoseCancels), // if you break this pose, it cancels.
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
				Action<HandPoseTracker> cubeFountain = (HandPoseTracker currentHand) => 
				{ 
					for (var i = 0; i < 20; i++) 
					{ 
						var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
						c.transform.localScale *= 0.3f;
						c.AddComponent<Rigidbody>(); 
						c.transform.position = Camera.main.transform.position + (Vector3.Normalize(currentHand.hand.position - Camera.main.transform.position) + Vector3.up) * 4f + Random.insideUnitSphere * 1; 
						c.GetComponent<Rigidbody>().AddForce(Vector3.up + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * 40f);
						c.GetComponent<Renderer>().material.color = Utils2.RandomColor;
					} 
				};
				return new Spell
				(
                    "Cube Fountain",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem(0.2f, pullMatterFromGroundStart, tolerance_01),
						new PoseSequenceItem(2, pullMatterFromGroundTwo, tolerance_01),
						new PoseSequenceItem(2, pullMatterFromGroundEnd, tolerance_01, cubeFountain, PoseSequenceItem.CancelOptions.AcquirePoseCancels)
					}
					
				);
			}
		}


		public static Spell LaserEyes
		{
			get
			{
				Action<HandPoseTracker> castLaserAction = (HandPoseTracker currentHand) =>
				{
					var pinchPosition = (currentHand._indexTip.position + currentHand._thumbTip.position) / 2f;
					var widthBetweenFingers = (currentHand._indexTip.position - currentHand._thumbTip.position).magnitude;
					LaserController.DrawLaser(pinchPosition, widthBetweenFingers);
				};
				Action<HandPoseTracker> cancelAction = (HandPoseTracker currentHand) =>
				{
					LaserController.Cancel();
				};
				return new Spell
				(
					"Laser Eyes",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem(.22f, new List<Pose>(){laserEyeStartA,laserEyeStartB,laserEyeStartC }, laserEyesTolerance, castLaserAction, PoseSequenceItem.CancelOptions.BreakPoseCancels, PoseSequenceItem.InvokeOptions.WhilePoseMaintained)
						//new PoseSequenceItem(2, pullMatterFromGroundEnd, tolerance_01, castLaser, PoseSequenceItem.CancelOptions.AcquirePoseCancels)
					},
					cancelAction

				);
			}
		}

		public static Spell PointToWalk
		{
			get
			{
				Action<HandPoseTracker> walkForwardAction = (HandPoseTracker currentHand) =>
				{
					WalkController.AddWalkForward(currentHand._indexTip.position - currentHand.GetJoint(Finger.Index,1).joint.position); // Direction pointer finger points.
				};
				
				return new Spell
				(
					"Walk Forward",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem(
							_time:.22f, 
							_poses: new List<Pose>(){pointForwardWalk,pointForwardWalk }, 
							_tolerance: pointForwardWalkTolerance, 
							_action: walkForwardAction, 
							_poseCancelType: PoseSequenceItem.CancelOptions.BreakPoseCancels, 
							_invokeOptions: PoseSequenceItem.InvokeOptions.WhilePoseMaintained, 
							_handRequirement: PoseSequenceItem.HandRequirement.TwoHanded)
						//new PoseSequenceItem(2, pullMatterFromGroundEnd, tolerance_01, castLaser, PoseSequenceItem.CancelOptions.AcquirePoseCancels)
					}

				);
			}
		}


		public static List<Spell> availableSpells = new List<Spell> { PointToWalk, forcePush,  TeleportSpell, ForceMove, CubeFountain, LaserEyes };


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
