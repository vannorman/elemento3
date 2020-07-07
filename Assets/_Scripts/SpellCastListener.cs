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

	public class SpellCastListener : MonoBehaviour
	{
		public Text spellIdentifier;
		public HandPoseTracker hand;
		public delegate void OnSpellCastCompleted();
		public OnSpellCastCompleted onSpellCastCompleted;

		// Start is called before the first frame update
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
			DetectSpellcast();

		}

		bool castingSpellState = false;
		Spell currentSpell;
		float spellTime = 0f;
		int seqInd = 0;



		private void DetectSpellcast()
		{


			if (!castingSpellState)
			{
				// detect spell start
				// note, tone changes may abrubptly abort when castingspellstate advances ..
				foreach (var spell in spells)
				{
					//DetectPoseAndPlayTone(spell);
					



					// Initiate or lock a spell to begin casting
					if (PoseWithinTolerance(hand.CurrentPose, spell.sequence[0].pose, spell.sequence[0].tolerance))
					{
						castingSpellState = true;
						currentSpell = spell;
						spellTime = 0;
						seqInd = 0;
						currentSpell.sequence[seqInd].action?.Invoke();
						spellIdentifier.text = "SPELL IDENTIFIED! NAME: " + currentSpell.name + "STAGE: 0 .. ";
						break;
					}
				}
			}
			if (castingSpellState && currentSpell != null)
			{
				//(*
// So the issue is that spell sequence index should be the LAST gesture you achieved, not the CURRENT DESIRED gesture -- it's one index ahead.
// That way we can test if the CURRENTLY ACHIEVED gesture should be maintained / cancel spell.

				// Current issue is that for FORCE MOVE, I can ENTER the state of the grab, but when I release the grab gesture, it's not detected by breakPoseCancels below.
				// So, I cannot cancel my spell by releaseing the gesture.

				// We've already achieved the first state of the spell, and will now listen for the next item in the sequence.

				if (!PoseWithinTolerance(hand.CurrentPose, currentSpell.sequence[0].pose, currentSpell.sequence[0].tolerance))
				{
					// If the user's hand is still matching the first item, do not increase the time. Only increase the spell time if user breaks pose from the initiator pose.
					spellTime += Time.deltaTime;
				}

				if (currentSpell.sequence[seqInd].breakPoseCancels && !PoseWithinTolerance(hand.CurrentPose, currentSpell.sequence[seqInd].pose, currentSpell.sequence[seqInd].tolerance))
				{
					// We were supposed to keep this pose -- breaking it cancels the spell.
					CancelCurrentSpell();
					return;
				}

				if (spellTime > currentSpell.sequence[seqInd].time && !timeModeInfinite)
				{
					// We exceeded the maximum time for this spell.
					CancelCurrentSpell();

					return;
				}
				if (seqInd < currentSpell.sequence.Count - 1 && PoseWithinTolerance(hand.CurrentPose, currentSpell.sequence[seqInd+1].pose, currentSpell.sequence[seqInd+1].tolerance))
				{
					// you invoked the next stage of the spell.
					seqInd++;
					spellTime = 0;
					currentSpell.sequence[seqInd].action?.Invoke();
				}
			}
		}

        private void CancelCurrentSpell()
        {
			spellIdentifier.text = "Lost spell (time):" + currentSpell.name;
			currentSpell.canceled?.Invoke();
			currentSpell = null;
			castingSpellState = false;
		}

        private void DetectPoseAndPlayTone(Spell spell)
		{
			// detect a spell that is "closest" wrt tones available and make those tones appear if close by
			// for each spell compare current pose distances for tones
			if (spell.sequence[0].tones != null)
			{
				//Debug.Log("tone not null for;" + spell.name);
				foreach (var tonePair in spell.sequence[0].tones)
				// Or, alternatively allow player to "lock" into only one spell for practice so tones only activate for that one spell.
				{
					float distToDesiredPosePart = Mathf.Abs(hand.CurrentPose.Values[tonePair.index] - spell.sequence[0].pose.Values[tonePair.index]);
					//Debug.Log("for tone " + tonePair + ", dist was:" + distToDesiredPosePart + ", while threshold was:" + tonePair.threshold);
					if (distToDesiredPosePart < tonePair.threshold)
					{
						AudioPlayer.inst.tones[tonePair.tone].volume = 1 - distToDesiredPosePart / tonePair.threshold;

						// also draw line FX for these digits that fades in/out to let player know

						// play a tone at a volume approaching 1 as dist approaches zero
						// possibly flex tune up or down in pitch inversely from 0 as well
					}
					else
					{
						AudioPlayer.inst.tones[tonePair.tone].volume = 0;
					}
				}
			}
		}

		public SpellData GetSpellData()
		{
			// data construct explicitly used for spell debugging to see what spell we're on and what stage
			var timeLeft = currentSpell != null ? currentSpell.sequence[seqInd].time - spellTime : -1;
			return new SpellData(currentSpell, seqInd,  timeLeft );
		}

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
