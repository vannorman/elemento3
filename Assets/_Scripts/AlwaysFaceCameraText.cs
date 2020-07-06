using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCameraText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		var invPos = Camera.main.transform.position + (transform.position - Camera.main.transform.position) * 1.01f;
		transform.LookAt(invPos, Vector3.up);
    }
}
