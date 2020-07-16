using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMobility : MonoBehaviour
{
    public Vector3 playerVector;

    Vector3 lastPos;
    Vector3 nowPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nowPos = transform.position;
        playerVector = nowPos - lastPos;
        lastPos = nowPos;
    }
}
