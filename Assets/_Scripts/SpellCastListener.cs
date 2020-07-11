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
				foreach (var spell in spells)
				{
					// Initiate or lock a spell to begin casting
					if (PoseWithinTolerance(currentHand.hand.CurrentPose, spell.sequence[0].pose, spell.sequence[0].tolerance))
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
					// We've already achieved the first state of the spell, and will now listen for the next item in the sequence.

					if (!PoseWithinTolerance(currentHand.hand.CurrentPose, currentHand.spell.sequence[0].pose, currentHand.spell.sequence[0].tolerance))
					{
						// If the user's hand is still matching the first item, do not increase the time. Only increase the spell time if user breaks pose from the initiator pose.
						currentHand.spellTime += Time.deltaTime;
						Utils2.SpellDebug("adding time to " + currentHand.spell.name + ": "+currentHand.spellTime+" / "+currentHand.spell.sequence[currentHand.seqInd].time);
					}

					if (PoseWithinTolerance(currentHand.hand.CurrentPose, currentHand.spell.sequence[0].pose, currentHand.spell.sequence[0].tolerance))
					{
						// If the user's hand is still matching the first item, do not increase the time. Only increase the spell time if user breaks pose from the initiator pose.
						currentHand.spellTime = 0;
						//Utils2.SpellDebug("reset time to zero " + currentHand.spell.name + " because pose 0 in tolerance.");
					}

					if (currentHand.spell.sequence[currentHand.seqInd].breakPoseCancels 
						&& !PoseWithinTolerance(
							currentHand.hand.CurrentPose, currentHand.spell.sequence[currentHand.seqInd].pose, 
							currentHand.spell.sequence[currentHand.seqInd].tolerance))
					{
						CancelCurrentSpell(currentHand, "pose was broken");
						return;
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

        private void CancelCurrentSpell(CurrentHand currentHand, string reason)
        {
			Utils2.SpellDebug("Canceled " + currentHand.spell.name + ": " + reason);
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
