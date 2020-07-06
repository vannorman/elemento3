using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float moveSpeed = 2f;
	public float rotSpeed = .5f;
	public bool position = true;
	public bool rotation = true;
   void LateUpdate()
    {
		if (position) transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * rotSpeed);
		if (rotation) transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotSpeed/4f);
    }

	public void Init(Transform _target, float _speed)
	{
		target = _target;
		rotSpeed = _speed;
	}
}
