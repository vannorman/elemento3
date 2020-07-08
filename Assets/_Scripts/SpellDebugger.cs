using OculusSampleFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Elemento.Spells;

namespace Elemento
{
    using OVRTouchSample;
    using System;
	using static Elemento.SpellCastListener;
	using Pose = PoseData.Pose;
	public class SpellDebugger : MonoBehaviour
	{
		public SpellCastListener spellcastListener;
		public HandPoseTracker handPoseTracker;
		public Text spellName;
		public Text totalPoses;
		public Text spellIndex;
		public Text spellTime;

		public Text timeMode;

		public Transform current;

		public ButtonController prevSpell;
		public ButtonController nextSpell;
		public ButtonController timeModeInfiniteButton;

		public Spell targetSpell;
		public Text targetSpellText;
		int targetIndex = 0;

		public GameObject checkMarkPrefab;
		public GameObject checkMarkUncheckedPrefab;
		List<GameObject> checkmarks = new List<GameObject>();

		public GameObject poseTextPrefab;
		public Transform poseMonitorParent;

		// Start is called before the first frame update
		void Start()
		{
			nextSpell.InteractableStateChanged.AddListener(NextSpell);
			prevSpell.InteractableStateChanged.AddListener(PreviousSpell);
			timeModeInfiniteButton.InteractableStateChanged.AddListener(ToggleTimeMode);

			handPoseTracker.onInitialized += UpdateTargetSpell;

			spellcastListener.onSpellCastCompleted += SpellCastCompleted;

		}

		private void SpellCastCompleted()
		{
			UpdateSpellProgressIndicator(true);
		}

		private void UpdateSpellProgressIndicator(bool spellCompleted = false, int newIndex = -1)
		{

			completedCutoff = spellCompleted ? int.MaxValue : newIndex;
			Debug.Log("<color=#090><b>New seq:</b></color>:" + newIndex);
			// update checkmark graphics.
			checkmarks.ForEach(x => Destroy(x));
			checkmarks.Clear();
			for (var i = 0; i < targetSpell.sequence.Count; i++)
			{
				if (completedCutoff > i)
				{
					// add a checked checkmark for every sequence achieved in this spell so far
					checkmarks.Add(Instantiate(checkMarkPrefab));
				}
				else
				{
					// add an empty one for unachieved sequences
					checkmarks.Add(Instantiate(checkMarkUncheckedPrefab));
				}
			}
			ParentCheckmarks();
		}

		public Transform checkmarksParent;
		List<GameObject> poseMeters = new List<GameObject>();
		List<GameObject> deltasMeters = new List<GameObject> ();
		void UpdateTargetSpell()
		{
			targetSpell = spells[targetIndex];
			targetSpellText.text = targetSpell.name;
			
			// Clear old chekcmark progress, create new empty checkboxes for sequence progress
			checkmarks.ForEach(x => Destroy(x));
			checkmarks.Clear();
			targetSpell.sequence.ForEach(x => checkmarks.Add(Instantiate(checkMarkUncheckedPrefab)));
			ParentCheckmarks();

			// Destroy and clear old pose metrics
			foreach (var go in poseMeters)
			{
				Destroy(go);
			}
			foreach (var go in deltasMeters)
			{
				Destroy(go);
			}
			poseMeters.Clear();
			deltasMeters.Clear();

			// for each pose in sequence, add a target and a delta
			for(var i=0;i<targetSpell.sequence.Count;i++)
			{
				var meter = Instantiate(poseTextPrefab);
				poseMeters.Add(meter);
				meter.transform.SetParent(poseMonitorParent);
				meter.transform.localPosition = Vector3.zero;
				meter.transform.localRotation = Quaternion.identity;
				meter.transform.localScale = Vector3.one * 0.1661068f;
				meter.name = "meter " + i;
				UpdatePoseText(meter.transform, targetSpell.sequence[i].pose); // show in the visible debug window ALL the poses you need to achieve.

				var delta = Instantiate(poseTextPrefab);
				deltasMeters.Add(delta);
				delta.transform.SetParent(poseMonitorParent);
				delta.transform.localPosition = Vector3.zero;
				delta.transform.localRotation = Quaternion.identity;
				delta.transform.localScale = Vector3.one * 0.1661068f;
				delta.name = "delta " + i;

			}
		}

		private void ParentCheckmarks()
		{
			foreach (var cm in checkmarks)
			{
				cm.transform.SetParent(checkmarksParent);
				cm.transform.localScale = Vector3.one;
				cm.transform.localPosition = Vector3.zero;
				cm.transform.localRotation = Quaternion.identity;
			}

		}

		public void NextSpell(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{

				targetIndex++;
				if (targetIndex >= spells.Count)
				{
					targetIndex = 0;
				}
				UpdateTargetSpell();
			}
		}

		public void PreviousSpell(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				targetIndex--;
				if (targetIndex < 0)
				{
					targetIndex = spells.Count - 1;
				}
				UpdateTargetSpell();
			}
		}

		int completedCutoff = -1;
		SpellData spell;

		// Update is called once per frame
		void Update()
		{
			UpdatePoseText(current, handPoseTracker.CurrentPose);
			Debug.Log("curpose index first;" + handPoseTracker.CurrentPose.thumbToIndex);
			UpdateDeltaPoseTexts();


			//// progress checkbarks?
			//spell = spellcastListener.GetSpellData();
			//if (spell.currentSpell != null)
			//{
				
			//	spellTime.text = (Mathf.Round(spell.timeRemaining * 100) / 100f).ToString();

				
			//	if (spell.currentSpell == targetSpell && completedCutoff != spell.index)
			//	{
			//		UpdateSpellProgressIndicator(false, spell.index);
			//	}
			//}
			//else
			//{
			//	if (completedCutoff != -1)
			//	{
			//		// horrible logic to remove checkmark spell indication
			//		UpdateSpellProgressIndicator(false, -1);  // no spell currently
			//	}
			//}
		}

		private void UpdateDeltaPoseTexts()
		{
			if (targetSpell == null) return;
			for (var i=0; i<targetSpell.sequence.Count; i++)
			{
				if (UpdateDeltaPoseText(deltasMeters[i].transform, targetSpell.sequence[i].pose, targetSpell.sequence[i].tolerance))
				{
					// all values green for this sequence!
					UpdateSpellProgressIndicator(false, i);
				}
			}
		}

		private void UpdatePoseText(Transform parent, Pose p)
		{
			var i = 0;
			parent.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = p.Values[i++].ToString());
		}


		private bool UpdateDeltaPoseText(Transform parent, Pose target, Pose tolerance)
		{
			var i = 0;
			var red = false;
			parent.GetComponentsInChildren<Text>().ToList().ForEach(x =>
			{
				var d = Mathf.Abs(handPoseTracker.CurrentPose.Values[i] - target.Values[i]);
				x.color = d < tolerance.Values[i] ? Color.green : Color.red;
				x.text = d.ToString();
				if (d < tolerance.Values[i]) red = true;
				i++;
			});
			return !red; // returns true only if all values were green (within tolerance)
		}

		bool timeModeInfinite = false;
		public void ToggleTimeMode(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				timeModeInfinite = !timeModeInfinite;
				timeMode.text = timeModeInfinite ? "Time mode: infinite" : "Time mode: regular"; // allow player to have infinite time between poses
				spellcastListener.SetTimeMode(timeModeInfinite);
			}
		}
	}
}
