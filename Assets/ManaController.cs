using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour
{
	public Obi.ObiEmitter emitter;

	// Update is called once per frame
	void Update()
	{

		

		if (Input.GetKey(KeyCode.R))
		{
			emitter.KillAll();
		}

	}
}
