using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
	// forget parabolas for a minute
	// snap once to start teleporting, project straight line from camera fowrad
	// snap again to confirm (or wait and it will dissipate)
	
	Transform arrow;
	public static Teleport inst;
	public GameObject teleportTargetPrefab;
	GameObject target;
	Text targetText;
	public Transform player;


    // Start is called before the first frame update
    void Start()
    {
		inst = this;
		arrow = new GameObject("arrow").transform;
		target = Instantiate(teleportTargetPrefab);
		targetText = target.GetComponentInChildren<Text>();
		target.SetActive(false);
	}

	public enum State
	{
		Ready,
		ShowLine,
	}

	public State state = State.Ready;

	bool teleporting = true;
	bool canTeleportHere = false;
	public float maxVert = 10;
	// Update is called once per frame
    void Update()
    {
		switch (state)
		{
			case State.Ready:
				break;
			case State.ShowLine:
				Transform cameraFwd = new GameObject("camfwd").transform;
				cameraFwd.transform.position = Camera.main.transform.position;
				cameraFwd.transform.rotation = Camera.main.transform.rotation;
				cameraFwd.Rotate(Camera.main.transform.right, 5); // look down a smidge
				var lookDir = cameraFwd.forward;
				Destroy(cameraFwd.gameObject);
				maxVert = 2f;
				if (Physics.Raycast(new Ray(Camera.main.transform.position, lookDir), out var hit))
				{
					target.transform.position = hit.point;
					if (Mathf.Abs(hit.point.y - player.position.y) > maxVert)
					{
						canTeleportHere = false;
						targetText.color = Color.red;
						targetText.text = "TOO TALL!";
						return;
					}
					var groundNormal = Mathf.Round(Vector3.Angle(hit.normal, Vector3.up)*100)/100f;
					var dist = Mathf.Round(hit.distance*100)/100f;
					if (dist > maxDist)
					{
						canTeleportHere = false;
						targetText.color = Color.red;
						targetText.text = "TOO FAR!";
						return;
					}
					if (groundNormal < 30)
					{
						canTeleportHere = true;
						targetText.color = Color.green;
						targetText.text = "Dist:" + dist.ToString() + "\nNorm:" + groundNormal.ToString();
					}
					else
					{
						// close enough, but hit a wall (not the ground).
						// if we hit a wall, try to move the teleport target back by 1 meter and down to ground.
						if (Vector3.Angle(hit.normal, lookDir) > 90)
						{
							if (Physics.Raycast(new Ray(hit.point - lookDir, Vector3.down), out var hit2))
							{
								var groundNormal2 = Mathf.Round(Vector3.Angle(hit2.normal, Vector3.up) * 100) / 100f;
								if (groundNormal2 < 30)
								{
									target.transform.position = hit2.point;
									if (Mathf.Abs(hit2.point.y - player.position.y) < maxVert)
									{
										canTeleportHere = true;
										targetText.color = Color.green;
										targetText.text = "Dist:" + dist.ToString() + "\n moved back";

									}
								}
							}
							else
							{
								canTeleportHere = false;
								targetText.color = Color.red;
								targetText.text = "Not ground, not wall?";
							}
						}


						

					}
				}
				else
				{
					//Debug.Log("Showing but no hp");
				}
				break;
		}
    }
	public static void TeleportNowStatic() { inst.TeleportNow(); }
	public void TeleportNow()
	{
		if (canTeleportHere)
		{
			if (state == State.ShowLine)
			{
				player.position = target.transform.position;
				SetState(State.Ready);
			}

		}
	}

	public void SetState(State newState)
	{
		state = newState;
		switch (state)
		{
			case State.Ready:
				target.SetActive(false);
				break;
			case State.ShowLine:
				Debug.Log("state swapped showline");
				target.SetActive(true);

				break;
		}
	}

	public static  void SetStateStatic(State newState)
	{
		inst.SetState(newState);
	}

	public float angleIncreaseA = 0.1f;
	public float stepAmount = .2f;
	public float zOffsetDeg = 10;
	public float maxDist = 100;

	public static void CancelStatic() { inst.Cancel();  }
	internal void Cancel()
	{
		SetState(State.Ready);
	}

	//private void DrawArc()
	//{
	//	var startP = HandDebug._smoothFollowHand.transform.position;
	//	var points = new List<Vector3>();
	//	points.Add(startP);
	//	arrow.position = startP;
	//	arrow.forward = HandDebug._smoothFollowHand.transform.right;
	//	arrow.Rotate(HandDebug._smoothFollowHand.transform.forward, zOffsetDeg, Space.World);
	//	var lastP = startP;
	//	var angleD = 0f;
	//	for (var i = 0; i < 40; i++)
	//	{
	//		points.Add(arrow.position);
	//		var axis = -Vector3.Cross(arrow.forward, Vector3.up);
	//		arrow.Rotate(axis, angleD, Space.World);
	//		angleD += angleIncreaseA;
	//		arrow.position += arrow.forward * stepAmount;
	//		if (Physics.Raycast(arrow.position, arrow.forward, out var hit))
	//		{
	//			if (hit.distance < stepAmount)
	//			{
	//				points.Add(hit.point);
	//				break;
	//			}
	//		}

	//	}

	//	var lr = GetComponent<LineRenderer>();
	//	lr.positionCount = points.Count;
	//	lr.SetPositions(points.ToArray());
	//}
}
