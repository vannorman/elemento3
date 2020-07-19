using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elemento
{
	using static Spells;
	using static PoseData;
	using static HandPoseTracker;
	using UnityEngine.UI;
	using System;
    using OVRTouchSample;
    using System.Linq;

    public class SpellCastListener : MonoBehaviour
	{
		public Text spellIdentifier;
		public HandPoseTracker leftHand;
		public HandPoseTracker rightHand;
		public delegate void OnSpellCastCompleted();
		public OnSpellCastCompleted onSpellCastCompleted;

		// Start is called before the first frame update
		void Start()
		{
			hands = new List<CurrentHand>{ new CurrentHand(leftHand), new CurrentHand(rightHand) };
		}

		// Update is called once per frame
		void Update()
		{
			DetectSpellcast();

		}

		class CurrentHand
		{
			public HandPoseTracker hand;
			public bool castingSpellState = false;
			public Spell spell;
			public float spellTime = 0f;
			public int seqInd = 0;
			public CurrentHand(HandPoseTracker _hand)
			{
				hand = _hand;
			}
		}

		List<CurrentHand> hands = new List<CurrentHand>();


		private void DetectSpellcast()
		{

			foreach (var currentHand in hands)
			{
				foreach (var spell in availableSpells)
				{
					// Initiate or lock a spell to begin casting
					if (AnyPoseWithinTolerance(currentHand.hand.CurrentPose, spell.sequence[0].poses, spell.sequence[0].tolerance))
						//if (PoseWithinTolerance(currentHand.hand.CurrentPose, spell.sequence[0].pose, spell.sequence[0].tolerance))
					{
						if (currentHand.spell == null || (currentHand.spell != null && currentHand.spell != spell))
						{
							if (currentHand.spell != null)
							{
								CancelCurrentSpell(currentHand, "for a new spell");
							}
							Utils2.SpellDebug("Started " + spell.name);
							currentHand.castingSpellState = true;
							currentHand.spell = spell;
							currentHand.spellTime = 0;
							currentHand.seqInd = 0;
							currentHand.spell.sequence[currentHand.seqInd].action?.Invoke(currentHand.hand);
						}
						//spellIdentifier.text = "SPELL IDENTIFIED! NAME: " + currentHand.currentSpell.name + "STAGE: 0 .. ";
						break;
					}
				}
				
				if (currentHand.castingSpellState && currentHand.spell != null)
				{


					var curSeq = currentHand.spell.sequence[currentHand.seqInd];

					// Invoke spell every frame if desired
					if (curSeq.invokeOptions == PoseSequenceItem.InvokeOptions.WhilePoseMaintained && AnyPoseWithinTolerance(currentHand.hand.CurrentPose, curSeq.poses, curSeq.tolerance))
					{
						curSeq.action.Invoke(currentHand.hand);
					}

					// If starting pose is not held, advance spell time.

					if (!PoseWithinTolerance(currentHand.hand.CurrentPose, currentHand.spell.sequence[0].pose, currentHand.spell.sequence[0].tolerance))
					{
						currentHand.spellTime += Time.deltaTime;
						Utils2.SpellDebug("adding time to " + currentHand.spell.name + ": "+currentHand.spellTime+" / "+currentHand.spell.sequence[currentHand.seqInd].time);
					}

					// If starting pose is (re)acquired, set spell time to zero.
					if (PoseWithinTolerance(currentHand.hand.CurrentPose, currentHand.spell.sequence[0].pose, currentHand.spell.sequence[0].tolerance))
					{
						currentHand.spellTime = 0;
					}

					// if pose was broken, cancel spell if desired
					//if (curSeq.poseCancelType == PoseSequenceItem.CancelOptions.BreakPoseCancels || curSeq.poseCancelType == PoseSequenceItem.CancelOptions.BreakOrAcquirePoseCancels 
					//	&& !AnyPoseWithinTolerance(
					//		currentHand.hand.CurrentPose, curSeq.poses, 
					//		curSeq.tolerance, true))
					//{
					//	CancelCurrentSpell(currentHand, "pose was broken for seq ind:"+currentHand.seqInd);
					//	return;
					//}
					if (curSeq.poseCancelType == PoseSequenceItem.CancelOptions.BreakPoseCancels || curSeq.poseCancelType == PoseSequenceItem.CancelOptions.BreakOrAcquirePoseCancels)
					{
						//if (!PoseWithinTolerance(currentHand.hand.CurrentPose, curSeq.pose, curSeq.tolerance))
						//{
						//	Utils2.SpellDebug("not in tolerance! "+currentHand.seqInd);
						//}
						if (!AnyPoseWithinTolerance(currentHand.hand.CurrentPose, curSeq.poses, curSeq.tolerance))
						{ 
							Utils2.SpellDebug("any pose not in tolerance! "+currentHand.seqInd);
							CancelCurrentSpell(currentHand, "broke tolerance");
							return;
						}
					}


					if (currentHand.spellTime > currentHand.spell.sequence[currentHand.seqInd].time && !timeModeInfinite)
					{
						CancelCurrentSpell(currentHand, "out of time");

						return;
					}
					if (currentHand.seqInd < currentHand.spell.sequence.Count - 1 && 
						PoseWithinTolerance(
							currentHand.hand.CurrentPose, currentHand.spell.sequence[currentHand.seqInd + 1].pose, 
							currentHand.spell.sequence[currentHand.seqInd + 1].tolerance))
					{
						// you invoked the next stage of the spell.
						currentHand.seqInd++;
						Utils2.SpellDebug("Advacned spell " + currentHand.spell.name + " to "+ currentHand.seqInd);
						currentHand.spellTime = 0;
						currentHand.spell.sequence[currentHand.seqInd].action?.Invoke(currentHand.hand);
						if (currentHand.spell.sequence[currentHand.seqInd].poseCancelType == PoseSequenceItem.CancelOptions.AcquirePoseCancels)
						{
							CancelCurrentSpell(currentHand, "Finish");
						}
					}
				}
			}
		}

        private bool AnyPoseWithinTolerance(Pose currentPose, List<Pose> poses, Pose tolerance)
        {
			foreach (var pose in poses)
			{
				if (PoseWithinTolerance(currentPose, pose, tolerance))
				{
					return true;
				}
			}

			return false;
        }

        private void CancelCurrentSpell(CurrentHand currentHand, string reason)
        {
			Utils2.SpellDebug("Canceled " + currentHand.spell.name + ": " + reason);
			Debug.Log("Canceled;" + reason);
			//spellIdentifier.text = "Lost spell (time):" + currentSpell.name;
			currentHand.spell.cancelAction?.Invoke(currentHand.hand);
			currentHand.spell = null;
			currentHand.castingSpellState = false;
		}

  //      private void DetectPoseAndPlayTone(Spell spell, HandPoseTracker currentHand)
		//{
		//	// detect a spell that is "closest" wrt tones available and make those tones appear if close by
		//	// for each spell compare current pose distances for tones
		//	if (spell.sequence[0].tones != null)
		//	{
		//		//Debug.Log("tone not null for;" + spell.name);
		//		foreach (var tonePair in spell.sequence[0].tones)
		//		// Or, alternatively allow player to "lock" into only one spell for practice so tones only activate for that one spell.
		//		{
		//			float distToDesiredPosePart = Mathf.Abs(currentHand.CurrentPose.Values[tonePair.index] - spell.sequence[0].pose.Values[tonePair.index]);
		//			//Debug.Log("for tone " + tonePair + ", dist was:" + distToDesiredPosePart + ", while threshold was:" + tonePair.threshold);
		//			if (distToDesiredPosePart < tonePair.threshold)
		//			{
		//				AudioPlayer.inst.tones[tonePair.tone].volume = 1 - distToDesiredPosePart / tonePair.threshold;

		//				// also draw line FX for these digits that fades in/out to let player know

		//				// play a tone at a volume approaching 1 as dist approaches zero
		//				// possibly flex tune up or down in pitch inversely from 0 as well
		//			}
		//			else
		//			{
		//				AudioPlayer.inst.tones[tonePair.tone].volume = 0;
		//			}
		//		}
		//	}
		//}

		//public SpellData GetSpellData()
		//{
		//	// data construct explicitly used for spell debugging to see what spell we're on and what stage
		//	var timeLeft = currentSpell != null ? currentSpell.sequence[seqInd].time - spellTime : -1;
		//	return new SpellData(currentSpell, seqInd,  timeLeft );
		//}

		public class SpellData
		{
			public Spell currentSpell;
			public int index;
			public float timeRemaining;
			public SpellData(Spell _cur, int _ind, float _t)
			{
				currentSpell = _cur;
				index = _ind;
				timeRemaining = _t;
			}
		}

		bool timeModeInfinite = false;
		public void SetTimeMode(bool _timeModeInfinite)
		{
			timeModeInfinite = _timeModeInfinite;
		}
	}
}
