using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//using static Elemento.HandPoseTracker

namespace Elemento
{
	using static PoseCollection;
	using static PoseData;
	using Pose = PoseData.Pose;
	public class HandDebug : MonoBehaviour
	{
		public HandPoseTracker hand;
		public Text output;
		public Material mat;
		public GameObject textLabelPrefab;
		public Transform target;
		public Transform current;
		public Transform delta;
		public Transform tolerance;
		public Transform lastSaved;
		public Transform current2;
		public Transform delta2;
		public Text spellIdentifier;

	


		public class TipDistanceLabel
		{
			//public PoseMetric metric;
			public Text label;
			public LineRenderer A;
			public LineRenderer B;
			public Finger start;
			public Finger end;
		}





		Transform WorldSpaceCanvas => GameObject.Find("WorldSpaceCanvas").transform;

		TipDistanceLabel thumbToIndex;
		TipDistanceLabel thumbToMiddle;
		TipDistanceLabel thumbToRing;
		TipDistanceLabel thumbToPinky;
		TipDistanceLabel betweenTipsIndexMiddle;
		TipDistanceLabel betweenTipsMiddleRing;
		TipDistanceLabel betweenTipsRingPinky;

		Text textLabelIndexFirstJoint;
		Text textLabelIndexTotal;
		Text textLabelMiddleFirstJoint;
		Text textLabelMiddleTotal;
		Text textLabelRingFirstJoint;
		Text textLabelRingTotal;
		Text textLabelPinkyFirstJoint;
		Text textLabelPinkyTotal;



		Text handRotation;
		public GameObject metricLinePrefab;
		public Transform metricLinesParent;


		public class MetricLine
		{
			public LineRenderer lr;
			public Func<float> GetMetricData { get; set; }
			public float yScale => 1f / (yMax - yMin);
			public float yMin = 0;
			public float yMax = 1.3f;
			public static int dataCount = 300;
			public static float xSpeed = 0.1f;

			public MetricLine(Func<float> action, LineRenderer _lr, Transform p, Color c, float _yMin, float _yMax)
			{
				// make sure the line renderer is intialized properly, and set to the right parent (the parent should be the UI vertical layout group)
				GetMetricData = action;
				lr = _lr;
				lr.positionCount = MetricLine.dataCount;
				lr.transform.parent.SetParent(p);
				lr.startColor = c;
				lr.endColor = c;
				lr.transform.parent.localPosition = new Vector3(0,0,-.01f);
				yMin = _yMin;
				yMax = _yMax;
				//yScale = _yScale;
			}
		}

		List<MetricLine> metricLines = new List<MetricLine>();
		// Start is called before the first frame update
		Pose targetDebug;
		Pose toleranceDebug = teleportTolerance;

		public bool debug = true;
		void Start()
		{
		
		
			InitializeLabels();
			var i = 0;
			//target.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = brandonTeleportStartP.Values[i++].ToString());
			i = 0;
			tolerance.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = teleportTolerance.Values[i++].ToString());
		}

		void Update()

		{

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				var i = 0;
				target.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = teleportStart.Values[i++].ToString());
				targetDebug = teleportStart;
			}


			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				var i = 0;
				target.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = teleportExecute.Values[i++].ToString());
				targetDebug = teleportExecute;
			}
			DrawLinesAndDistancesBetweenFingertips();
			DrawAngleAmountsOnKnuckles();
			DrawMetricLinesToTrackHandPoses();

			if (debug)
			{
				UpdateCurrentPoseText();
				UpdateDeltaPoseText(targetDebug,teleportTolerance);
				UpdatecurrentPose2Text();
				UpdateDeltaPoseText2();
			}

			if (Input.GetKeyDown(KeyCode.T))
			{
				var i = 0;
				var s = "";
				tolerance_01.Values.ToList().ForEach(x => s += Pose.ValueLabels[i++] + ": " + x.ToString() + "f,\n");
				s.TrimEnd(',');
				Debug.Log("<color=#008>TOLERANCE:</color>: " + Time.time);
				Debug.Log(s);
			}

			if (Input.GetKeyDown(KeyCode.R))
			{
				SavePose(hand.CurrentPose, "Recorded");
				var i = 0;
				var s = "";
				hand.CurrentPose.Values.ToList().ForEach(x => s += Pose.ValueLabels[i++] + ": " + x.ToString() + "f,\n");
				s.TrimEnd(',');
				Debug.Log("<color=#008>RECORDED:</color>: " + Time.time);
				Debug.Log(s);
				UpdateSavedPoseText();

			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				var PrevPose = LoadPose("Recorded");
				var deltaPose = ComparePose(hand.CurrentPose, PrevPose);


				var d = "";
				deltaPose.ToList().ForEach(x => d += x.ToString() + "f,");

				Debug.Log("TOLERANCE:");
				Debug.Log(d);
			}
		}


	


		LineRenderer InitiLR => Instantiate(metricLinePrefab).GetComponentInChildren<LineRenderer>();
		private void InitializeMetricLines()
		{
			var tipDistMin = 0;
			var tipDistMax = .13f;
			var rotSingleMin = 0;
			var rotSingleMax = 90;
			var rotCumMin = 0;
			var rotCumMax = 360;
			var rotWristMin = 0;
			var rotWristMax = 90;
			metricLines = new List<MetricLine>{
				// MetricLine args: "assign func to allow perpetual current getting of value for this metric" , "Create and assign line renderer", "set parent",  "set color,  "min", "max" 

				// Between tips

				// TODO: Replace this with CurrentPose.value instead of float parse the text
				new MetricLine(() => { return float.Parse(thumbToIndex.label.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, tipDistMin, tipDistMax),
				new MetricLine(() => { return float.Parse(thumbToMiddle.label.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, tipDistMin, tipDistMax),
				new MetricLine(() => { return float.Parse(thumbToRing.label.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, tipDistMin, tipDistMax),
				new MetricLine(() => { return float.Parse(thumbToPinky.label.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, tipDistMin, tipDistMax),
				new MetricLine(() => { return float.Parse(betweenTipsIndexMiddle.label.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, tipDistMin, tipDistMax),
				new MetricLine(() => { return float.Parse(betweenTipsMiddleRing.label.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, tipDistMin, tipDistMax),
				new MetricLine(() => { return float.Parse(betweenTipsRingPinky.label.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, tipDistMin, tipDistMax),

				// rotation of first joint for each finger
				new MetricLine(() => { return float.Parse(textLabelIndexFirstJoint.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, rotSingleMin, rotSingleMax),
				new MetricLine(() => { return float.Parse(textLabelMiddleFirstJoint.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, rotSingleMin, rotSingleMax),
				new MetricLine(() => { return float.Parse(textLabelRingFirstJoint.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, rotSingleMin, rotSingleMax),
				new MetricLine(() => { return float.Parse(textLabelPinkyFirstJoint.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, rotSingleMin, rotSingleMax),

				// total rotation of each finger
				new MetricLine(() => { return float.Parse(textLabelIndexTotal.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, rotCumMin, rotCumMax),
				new MetricLine(() => { return float.Parse(textLabelMiddleTotal.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, rotCumMin, rotCumMax),
				new MetricLine(() => { return float.Parse(textLabelRingTotal.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, rotCumMin, rotCumMax),
				new MetricLine(() => { return float.Parse(textLabelPinkyTotal.text); }, InitiLR, metricLinesParent, Utils2.RandomColor, rotCumMin, rotCumMax),

				
				// wrist data
				new MetricLine(() => { return hand.CurrentPose.wristPointsForward; }, InitiLR, metricLinesParent, Utils2.RandomColor, rotWristMin, rotWristMax),
				new MetricLine(() => { return hand.CurrentPose.wristPointsUp; }, InitiLR, metricLinesParent, Utils2.RandomColor, rotWristMin, rotWristMax),
				new MetricLine(() => { return hand.CurrentPose.palmFaceRight; }, InitiLR, metricLinesParent, Utils2.RandomColor, rotWristMin, rotWristMax),
				new MetricLine(() => { return hand.CurrentPose.palmPointsUp; }, InitiLR, metricLinesParent, Utils2.RandomColor, rotWristMin, rotWristMax),
			};

		}

		void SavePose(Pose p, string pref)
		{
			LastSavedPose = p;
			var poseAsString = string.Join(",", p.Values);
			PlayerPrefs.SetString(pref, poseAsString);
		}
	

		public Pose LoadPose(string pref)
		{
			return new Pose(PlayerPrefs.GetString(pref).Split(',').ToList().Select(x => float.Parse(x)).ToArray());
		}

		void InitializeLabels()
		{
			// init middle to thumb text
			thumbToIndex = new TipDistanceLabel();
			thumbToMiddle = new TipDistanceLabel();
			thumbToRing = new TipDistanceLabel();
			thumbToPinky = new TipDistanceLabel();

			InitializeLineRenderer(thumbToIndex, Finger.Thumb, Finger.Index, Utils2.Purple);
			InitializeLineRenderer(thumbToMiddle, Finger.Thumb, Finger.Middle,  Utils2.Purple);
			InitializeLineRenderer(thumbToRing, Finger.Thumb, Finger.Ring,  Utils2.Purple);
			InitializeLineRenderer(thumbToPinky, Finger.Thumb, Finger.Pinky, Utils2.Purple);

			betweenTipsIndexMiddle = new TipDistanceLabel();
			betweenTipsMiddleRing = new TipDistanceLabel();
			betweenTipsRingPinky = new TipDistanceLabel();

			InitializeLineRenderer(betweenTipsIndexMiddle, Finger.Index, Finger.Middle, Color.cyan);
			InitializeLineRenderer(betweenTipsMiddleRing, Finger.Middle, Finger.Ring, Color.cyan);
			InitializeLineRenderer(betweenTipsRingPinky, Finger.Ring, Finger.Pinky, Color.cyan);

			// init rotation texts
			var textLabels = new List<Text>() {
				textLabelIndexFirstJoint,
				textLabelIndexTotal,
				textLabelMiddleFirstJoint,
				textLabelMiddleTotal,
				textLabelRingFirstJoint,
				textLabelRingTotal,
				textLabelPinkyFirstJoint,
				textLabelPinkyTotal
			};

			// todo move to KnuckleRotationLabel class
			textLabelIndexFirstJoint = Instantiate(textLabelPrefab).GetComponent<Text>();
			textLabelIndexTotal = Instantiate(textLabelPrefab).GetComponent<Text>();
			textLabelMiddleFirstJoint = Instantiate(textLabelPrefab).GetComponent<Text>();
			textLabelMiddleTotal = Instantiate(textLabelPrefab).GetComponent<Text>();
			textLabelRingFirstJoint = Instantiate(textLabelPrefab).GetComponent<Text>();
			textLabelRingTotal = Instantiate(textLabelPrefab).GetComponent<Text>();
			textLabelPinkyFirstJoint = Instantiate(textLabelPrefab).GetComponent<Text>();
			textLabelPinkyTotal = Instantiate(textLabelPrefab).GetComponent<Text>();

			textLabelIndexFirstJoint.transform.SetParent(WorldSpaceCanvas);
			textLabelIndexTotal.transform.SetParent(WorldSpaceCanvas);
			textLabelMiddleFirstJoint.transform.SetParent(WorldSpaceCanvas);
			textLabelMiddleTotal.transform.SetParent(WorldSpaceCanvas);
			textLabelRingFirstJoint.transform.SetParent(WorldSpaceCanvas);
			textLabelRingTotal.transform.SetParent(WorldSpaceCanvas);
			textLabelPinkyFirstJoint.transform.SetParent(WorldSpaceCanvas);
			textLabelPinkyTotal.transform.SetParent(WorldSpaceCanvas);

			handRotation = Instantiate(textLabelPrefab).GetComponent<Text>();
			handRotation.transform.SetParent(WorldSpaceCanvas);
			handRotation.gameObject.name = "Hand Rotation";

		}




		private void InitializeLineRenderer(TipDistanceLabel LL, Finger start, Finger end, Color c)
		{
			
			LL.start = start;
			LL.end = end;
			var mtt = (GameObject)Instantiate(textLabelPrefab);  
			mtt.transform.SetParent(WorldSpaceCanvas);
			LL.label = mtt.GetComponent<Text>();
			LL.label.alignment = TextAnchor.MiddleCenter;
			//LL.distLabel.color = Color.cyan;

			GameObject lr1 = new GameObject("lr1");
			lr1.transform.SetParent(mtt.transform);
			lr1.transform.localPosition = Vector3.zero;
			LL.A = lr1.AddComponent<LineRenderer>();
			LL.A.material = mat;
			LL.A.material.color = c;
			LL.A.startWidth = 0.001f;
			LL.A.endWidth = 0.001f;

			GameObject lr2 = new GameObject("lr2");
			lr2.transform.SetParent(mtt.transform);
			lr2.transform.localPosition = Vector3.zero;
			LL.B = lr2.AddComponent<LineRenderer>();
			LL.B.material = mat;
			LL.B.material.color = c;
			LL.B.startWidth = 0.001f;
			LL.B.endWidth = 0.001f;
			
		}
		/*
		thumb - Index
		thumb - Middle
		thumb - Ring
		thumb - Pinky
		index - middle
		middle - ring
		ring - pinky
		index first rot
		index total rot
		mid first rot
		mid total rot
		ring first rot
		ring total rot
		pinky first rot 
		pinky total rot
		wrist points forward
		palm  points up 
		palm face right
		wrist face up
		
		 
			 */

		Pose LastSavedPose = new Pose();


		// Update is called once per frame


		private void UpdatecurrentPose2Text()
		{
			var i = 0;
			current2.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = hand.CurrentPose.Values[i++].ToString());
		}

		private void UpdateDeltaPoseText(Pose p, Pose t)
		{
			var i = 0;
			delta.GetComponentsInChildren<Text>().ToList().ForEach(x => 
			{
				var d = Mathf.Abs(hand.CurrentPose.Values[i] - p.Values[i]);
				x.color = d < t.Values[i] ? Color.green : Color.red;
				x.text = d.ToString();
				i++;
			});
		}

		private void UpdateDeltaPoseText2()
		{
			var i = 0;
			delta2.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = Mathf.Abs(hand.CurrentPose.Values[i] - LastSavedPose.Values[i++]).ToString());
		}


		private void UpdateCurrentPoseText()
		{
			var i = 0;
			current.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = hand.CurrentPose.Values[i++].ToString());
		}

		private void UpdateSavedPoseText()
		{
			var i = 0;
			lastSaved.GetComponentsInChildren<Text>().ToList().ForEach(x => x.text = LastSavedPose.Values[i++].ToString());
		}
	   
		void DrawLinesAndDistancesBetweenFingertips()
		{
			// Draw lines and distances
			hand.CurrentPose.thumbToIndex = GetTipDistance(thumbToIndex);
			hand.CurrentPose.thumbToMiddle = GetTipDistance(thumbToMiddle); 
			hand.CurrentPose.thumbToRing = GetTipDistance(thumbToRing);
			hand.CurrentPose.thumbToPinky = GetTipDistance(thumbToPinky);
			hand.CurrentPose.betweenTipsIndexMiddle = GetTipDistance(betweenTipsIndexMiddle); 
			hand.CurrentPose.betweenTipsMiddleRing = GetTipDistance(betweenTipsMiddleRing); 
			hand.CurrentPose.betweenTipsRingPinky = GetTipDistance(betweenTipsRingPinky);
		}

		float GetTipDistance(TipDistanceLabel LL)
		{
			// Returns tip distance AND draws a handy dandy line to show you
			var start = hand.GetFingerTipPosition(LL.start);
			var end = hand.GetFingerTipPosition(LL.end);
			var mid = (start + end) / 2f;
			var spacing = .01f;
			var midA = mid + (start - end).normalized * spacing;
			var midB = mid + (end - start).normalized * spacing;
			var val = (Mathf.RoundToInt((start - end).magnitude * 100) / 100f);


			LL.A.SetPositions(new Vector3[] { start, midA });
			LL.B.SetPositions(new Vector3[] { midB, end });
			LL.label.transform.position = mid;
			LL.label.text = val.ToString();

			
			return val;
		}

		void DrawAngleAmountsOnKnuckles()
		{


			// then set labels text above joints
			textLabelIndexFirstJoint.text = hand.CurrentPose.indexFirstJoint.ToString();
			textLabelIndexTotal.text = hand.CurrentPose.indexTotal.ToString();
			textLabelMiddleFirstJoint.text = hand.CurrentPose.middleFirstJoint.ToString();
			textLabelMiddleTotal.text = hand.CurrentPose.middleTotal.ToString();
			textLabelRingFirstJoint.text = hand.CurrentPose.ringFirstJoint.ToString();
			textLabelRingTotal.text = hand.CurrentPose.ringTotal.ToString();
			textLabelPinkyFirstJoint.text = hand.CurrentPose.pinkyFirstJoint.ToString();
			textLabelPinkyTotal.text = hand.CurrentPose.pinkyTotal.ToString();

			// Position the text "outside" the hand to hover over the joint

			textLabelIndexFirstJoint.transform.position = GetKnuckleTextPositionForJoint(hand.GetJoint(Finger.Index,1).joint);
			textLabelIndexTotal.transform.position = GetKnuckleTextPositionForJoint(hand.GetJoint(Finger.Index, 2).joint);
			textLabelMiddleFirstJoint.transform.position = GetKnuckleTextPositionForJoint(hand.GetJoint(Finger.Middle, 1).joint);
			textLabelMiddleTotal.transform.position = GetKnuckleTextPositionForJoint(hand.GetJoint(Finger.Middle, 2).joint);
			textLabelRingFirstJoint.transform.position = GetKnuckleTextPositionForJoint(hand.GetJoint(Finger.Ring, 1).joint);
			textLabelRingTotal.transform.position = GetKnuckleTextPositionForJoint(hand.GetJoint(Finger.Ring, 2).joint);
			textLabelPinkyFirstJoint.transform.position = GetKnuckleTextPositionForJoint(hand.GetJoint(Finger.Pinky, 1).joint);
			textLabelPinkyTotal.transform.position = GetKnuckleTextPositionForJoint(hand.GetJoint(Finger.Pinky, 2).joint);

			handRotation.text = "Fwd:" + hand.CurrentPose.wristPointsForward + "\nPalmUp:" + hand.CurrentPose.palmPointsUp + "\npointRight:" + hand.CurrentPose.palmFaceRight + "\nwrist point Up:" + hand.CurrentPose.wristPointsUp;
			handRotation.transform.position = hand._bones.transform.position - hand.hand.transform.right * .1f;

		}

		void DrawMetricLinesToTrackHandPoses()
		{
			metricLines.ForEach(x => {
				Vector3[] arr = new Vector3[x.lr.positionCount];
				x.lr.GetPositions(arr);
				var currentDataPoint = new Vector3(-MetricLine.xSpeed, x.GetMetricData() * x.yScale, 0);
				arr = arr.Reverse().Skip(1).Append(currentDataPoint).Reverse().ToArray(); // add oe element to beginning, delete last element, still 100
				for (int i = 0; i < arr.Length; i++)
					arr[i].x += MetricLine.xSpeed; // nudge to the right this frame. note frst element moves to x=0, because above we set its x to -nudgeAmount
				x.lr.SetPositions(arr);

			});
	
		}



		public Vector3 GetKnuckleTextPositionForJoint(Transform j, float dist=.015f)
		{
			// the bigger the negative Z, from 0 to -90, the more to the X direction we position it
			float x = (1f / (90 - -j.localRotation.z) / 90f) * 2;
			return j.position + (-j.up + j.right * x) * dist;
		}

	
	

	}
}
