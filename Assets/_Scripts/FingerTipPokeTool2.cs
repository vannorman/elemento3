/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace OculusSampleFramework
{
	/// <summary>
	/// Poke tool used for near-field (touching) interactions. Assumes that it will be placed on
	/// finger tips.
	/// </summary>
	public class FingerTipPokeTool2 : InteractableTool
	{
		private const int NUM_VELOCITY_FRAMES = 10;

		[SerializeField] private FingerTipPokeToolView _fingerTipPokeToolView = null;
		[SerializeField] private OVRPlugin.HandFinger _fingerToFollow = OVRPlugin.HandFinger.Index;

		public override InteractableToolTags ToolTags
		{
			get
			{
				return InteractableToolTags.Poke;
			}
		}
		public override ToolInputState ToolInputState
		{
			get
			{
				return ToolInputState.Inactive;
			}
		}
		public override bool IsFarFieldTool
		{
			get
			{
				return false;
			}
		}
		public override bool EnableState
		{
			get
			{
				return _fingerTipPokeToolView.gameObject.activeSelf;
			}
			set
			{
				_fingerTipPokeToolView.gameObject.SetActive(value);
			}
		}

		private Vector3[] _velocityFrames;
		private int _currVelocityFrame = 0;
		private bool _sampledMaxFramesAlready;
		private Vector3 _position;

		private BoneCapsuleTriggerLogic[] _boneCapsuleTriggerLogic;

		private float _lastScale = 1.0f;
		private bool _isInitialized = false;
		private OVRBoneCapsule _capsuleToTrack;
		private Transform _tipToTrack;

		public override void Initialize()
		{
			Assert.IsNotNull(_fingerTipPokeToolView);

			InteractableToolsInputRouter.Instance.RegisterInteractableTool(this);
			_fingerTipPokeToolView.InteractableTool = this;

			_velocityFrames = new Vector3[NUM_VELOCITY_FRAMES];
			Array.Clear(_velocityFrames, 0, NUM_VELOCITY_FRAMES);

			StartCoroutine(AttachTriggerLogic());
		}

		private IEnumerator AttachTriggerLogic()
		{
			while (!HandsManager.Instance || !HandsManager.Instance.IsInitialized())
			{
				yield return null;
			}

			//OVRSkeleton.BoneId boneToTestCollisions = OVRSkeleton.BoneId.Hand_Pinky3;
			//switch (_fingerToFollow)
			//{
			//	case OVRPlugin.HandFinger.Thumb:
			//		boneToTestCollisions = OVRSkeleton.BoneId.Hand_Index3;
			//		break;
			//	case OVRPlugin.HandFinger.Index:
			//		boneToTestCollisions = OVRSkeleton.BoneId.Hand_Index3;
			//		break;
			//	case OVRPlugin.HandFinger.Middle:
			//		boneToTestCollisions = OVRSkeleton.BoneId.Hand_Middle3;
			//		break;
			//	case OVRPlugin.HandFinger.Ring:
			//		boneToTestCollisions = OVRSkeleton.BoneId.Hand_Ring3;
			//		break;
			//	default:
			//		boneToTestCollisions = OVRSkeleton.BoneId.Hand_Pinky3;
			//		break;
			//}
			List<BoneCapsuleTriggerLogic> boneCapsuleTriggerLogic = new List<BoneCapsuleTriggerLogic>();
			_tipToTrack = IsRightHandedTool ? HandsManager.Instance.LeftHand.transform.FindInChildren("Hand_IndexTip") : HandsManager.Instance.RightHand.transform.FindInChildren("Hand_IndexTip");

			var boneCapsuleTrigger = _tipToTrack.gameObject.AddComponent<BoneCapsuleTriggerLogic>();
			//var ovrCapsuleInfo = _tipToTrack.gameObject.AddComponent<OVRBoneCapsule>();
			//ovrCapsuleInfo.CapsuleCollider.isTrigger = true;
			//boneCapsuleTrigger.ToolTags = ToolTags;
			//boneCapsuleTriggerLogic.Add(boneCapsuleTrigger);
			

			_boneCapsuleTriggerLogic = boneCapsuleTriggerLogic.ToArray();
			_isInitialized = true;
		}

		private void Update()
		{
			if (!HandsManager.Instance || !HandsManager.Instance.IsInitialized() || !_isInitialized || _tipToTrack == null)
			{
				return;
			}

			OVRHand hand = IsRightHandedTool ? HandsManager.Instance.RightHand : HandsManager.Instance.LeftHand;
			float currentScale = hand.HandScale;
			// push tool into the tip based on how wide it is. so negate the direction
			Vector3 trackedPosition = _tipToTrack.position;

			// push tool back so that it's centered on transform/bone
			Vector3 toolPosition = trackedPosition;
			transform.position = toolPosition;
			InteractionPosition = trackedPosition;

			UpdateAverageVelocity();

			CheckAndUpdateScale();
		}

		private void UpdateAverageVelocity()
		{
			var prevPosition = _position;
			var currPosition = transform.position;
			var currentVelocity = (currPosition - prevPosition) / Time.deltaTime;
			_position = currPosition;
			_velocityFrames[_currVelocityFrame] = currentVelocity;
			// if sampled more than allowed, loop back toward the beginning
			_currVelocityFrame = (_currVelocityFrame + 1) % NUM_VELOCITY_FRAMES;

			Velocity = Vector3.zero;
			// edge case; when we first start up, we will have only sampled less than the
			// max frames. so only compute the average over that subset. After that, the
			// frame samples will act like an array that loops back toward to the beginning
			if (!_sampledMaxFramesAlready && _currVelocityFrame == NUM_VELOCITY_FRAMES - 1)
			{
				_sampledMaxFramesAlready = true;
			}

			int numFramesToSamples = _sampledMaxFramesAlready ? NUM_VELOCITY_FRAMES : _currVelocityFrame + 1;
			for (int frameIndex = 0; frameIndex < numFramesToSamples; frameIndex++)
			{
				Velocity += _velocityFrames[frameIndex];
			}

			Velocity /= numFramesToSamples;
		}

		private void CheckAndUpdateScale()
		{
			float currentScale = IsRightHandedTool ? HandsManager.Instance.RightHand.HandScale
				: HandsManager.Instance.LeftHand.HandScale;
			if (Mathf.Abs(currentScale - _lastScale) > Mathf.Epsilon)
			{
				transform.localScale = new Vector3(currentScale, currentScale, currentScale);
				_lastScale = currentScale;
			}
		}

		public override List<InteractableCollisionInfo> GetNextIntersectingObjects()
		{
			_currentIntersectingObjects.Clear();

			foreach (var boneCapsuleTriggerLogic in _boneCapsuleTriggerLogic)
			{
				var collidersTouching = boneCapsuleTriggerLogic.CollidersTouchingUs;
				foreach (ColliderZone colliderTouching in collidersTouching)
				{
					_currentIntersectingObjects.Add(new InteractableCollisionInfo(colliderTouching,
						colliderTouching.CollisionDepth, this));
				}
			}

			return _currentIntersectingObjects;
		}

		public override void FocusOnInteractable(Interactable focusedInteractable,
		  ColliderZone colliderZone)
		{
			// no need for focus
		}

		public override void DeFocus()
		{
			// no need for focus
		}
	}
}
