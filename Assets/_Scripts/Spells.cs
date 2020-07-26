using OVRTouchSample;
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
	using static PoseCollection;

	// Current issue is that one spell is easy to "start" and then it lingers around while I'd rather have a different spell? and the "move cubes" doesn't care which way my hand is flipped, too insensitve for start.

    public static class Spells
	{


		

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
			public int mana;
			public HandPoseTracker castingHand; // changes when spell is cast
			public string name = "Spell1";
			public List<PoseSequenceItem> sequence = new List<PoseSequenceItem>();
			public Action<HandPoseTracker> cancelAction;
			public Spell(string _name, List<PoseSequenceItem> _sequence, int _mana, Action<HandPoseTracker> _canceled = null)
			{
				name = _name;
				sequence = _sequence;
				mana = _mana;
				cancelAction = _canceled;
			}
		}

		public static Spell forcePush
		{
			get
			{
				Action<HandPoseTracker> forcePushBuildup = (HandPoseTracker handTracker) =>
				{
					ForcePushController.BuildUpForceStatic(handTracker);
				};

				Action<HandPoseTracker> forcePushAction = (HandPoseTracker handTracker) =>
				{
					ForcePushController.PushNowFromHand(handTracker);
				};
					
				Action<HandPoseTracker> cancelAction = (HandPoseTracker handTracker) => ForcePushController.Cancel(handTracker);
				return new Spell
				(
					"Push",
					new List<PoseSequenceItem>()
					{
						new PoseSequenceItem( 0.5f, forcePushStart, forcePushTolerance, forcePushBuildup, PoseSequenceItem.CancelOptions.None, PoseSequenceItem.InvokeOptions.WhilePoseMaintained),
						new PoseSequenceItem( 0f, forcePushEnd, forcePushTolerance, forcePushAction, PoseSequenceItem.CancelOptions.AcquirePoseCancels )
					},
					_mana: 5,
					cancelAction

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
					_mana: 5,
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
					_mana: 5,
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
					},
					_mana: 5


				);
			}
		}


		public static Spell LaserEyes
		{
			get
			{
				Action<HandPoseTracker> castLaserAction = (HandPoseTracker currentHand) =>
				{

					LaserController.OnLaserActivated(currentHand);
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
					_mana: 5,
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
					},
					_mana: 5

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
