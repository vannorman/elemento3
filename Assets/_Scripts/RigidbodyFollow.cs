using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyFollow : MonoBehaviour
{
    Transform target;
    public float speed = 3;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = Camera.main.transform;
    }
    void Update()
    {
        var dirToTarget = (target.transform.position - transform.position);
        var distToTarget = dirToTarget.magnitude;
        if (distToTarget < 0.2f)
        {
            return;
        }
        if (distToTarget > 3)
        {
            transform.position = target.position; // + dirToTarget.normalized * 3f;
        }
        else if (distToTarget > .2f)
        { 
            rb.MovePosition(transform.position + dirToTarget * speed * Time.deltaTime);
            //rb.velocity = Vector3.zero;
        
        }
    }
}
