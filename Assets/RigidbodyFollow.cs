using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 3;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        var dirToTarget = (target.transform.position - transform.position);
        var distToTarget = dirToTarget.magnitude;
        if (distToTarget > 3)
        {
            transform.position = target.position + dirToTarget * 3f;
        }
        else if (distToTarget > .2f)
        { 
            rb.MovePosition(transform.position + dirToTarget * speed * Time.deltaTime);
            //rb.velocity = Vector3.zero;
        
        }
    }
}
