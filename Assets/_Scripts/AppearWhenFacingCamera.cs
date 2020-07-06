using OculusSampleFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearWhenFacingCamera : MonoBehaviour
{
	public Vector3 localForward = Vector3.up;
	public Vector3 localUp = Vector3.right;
	public GameObject spellDebuggerPanel;
	public ButtonController detachButton;

	bool detachedState = false;
	Vector3 startPos;
	private float maxDistBeforeReattach = 2.5f;

	private void Start()
	{
		// editor hack - we flipped it 180 to view in editor because default Oculus pose makes it upside down for editing
		// so, flip it back during start lol
		this.transform.localRotation = Quaternion.identity;

		detachButton.InteractableStateChanged.AddListener(Detach);
		startPos = spellDebuggerPanel.transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		var distToCamera = (spellDebuggerPanel.transform.position - Camera.main.transform.position).magnitude;
		if (detachedState && distToCamera > maxDistBeforeReattach)
		{
			// player moved far away after detaching the gui
			Reattach();
		}
		if (!detachedState)
		{
			CheckIfShouldBeActive();
		}
	}

	private void CheckIfShouldBeActive()
	{
		var faceCameraDelta = Vector3.Angle(transform.TransformVector(localForward), -Camera.main.transform.forward);
		var faceUpwardDelta = Vector3.Angle(transform.TransformVector(localUp), Vector3.up);
		if (faceUpwardDelta > 55 && faceCameraDelta < 35 && !spellDebuggerPanel.activeSelf)
		{
			spellDebuggerPanel.SetActive(true);
		}
		else if ((faceUpwardDelta < 55 || faceCameraDelta > 35) && spellDebuggerPanel.activeSelf)
		{
			spellDebuggerPanel.SetActive(false);
		}

	}

	public void Detach(InteractableStateArgs o )
	{
		if (o.NewInteractableState == InteractableState.ActionState)
		{
			detachedState = !detachedState;
			if (detachedState)
			{
				Detach();
			}
			else
			{
				Reattach();
				
			}
		}
	}

	private void Reattach()
	{
		detachedState = false;
		spellDebuggerPanel.transform.SetParent(this.transform);
		spellDebuggerPanel.transform.localPosition = startPos;
		spellDebuggerPanel.transform.localRotation = Quaternion.identity;
	}

	private void Detach()
	{
		spellDebuggerPanel.transform.parent = null;
	}
}
