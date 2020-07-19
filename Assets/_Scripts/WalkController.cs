using Oculus.Platform.Samples.VrHoops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkController : MonoBehaviour
{

    public static WalkController Instance;
    Transform player;
    private void Start()
    {
        Instance = this;
        player = FindObjectOfType<OVRCameraRig>().transform;
    }
    public static Vector3 totalForward = Vector3.zero;
    public static void AddWalkForward(Vector3 fwd)
    {
        totalForward += fwd;    
    }

    public float speed = 1;
    private void Update()
    {
        totalForward = totalForward.normalized;
        var newPos = player.position + totalForward * Time.deltaTime * speed;
        {
            // If hit wall, move new pt backward
            if (Physics.Raycast(player.position, totalForward, out var hit, totalForward.magnitude))
            {
                var dirTowardsWall = hit.point - player.position;
                newPos = hit.point - dirTowardsWall * totalForward.magnitude;
                return; // just don't
            }
        }
        {
            // if float/underground, snap to ground
            if (Physics.Raycast(newPos + Vector3.up * .2f, Vector3.down, out var hit))
            {
                newPos.y = hit.point.y;
            }

        }
        //var angle = Utils2.FlattenVector()
        //.transform.rotation = newRot;
        player.position = newPos;
        totalForward = Vector3.zero;
    }
}
