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
    static bool didWalk = false;
    public static void AddWalkForward(Vector3 fwd)
    {
        totalForward += fwd.normalized;
        didWalk = true;
    }

    public float speed = 1;
    private void Update()
    {
        if (!didWalk) return;
        var newPos = player.position + totalForward * Time.deltaTime * speed;
        {
            var stepHeight = 0.25f;
            // If hit wall, move new pt backward
            if (Physics.Raycast(player.position + Vector3.up * stepHeight, totalForward, out var hit, totalForward.magnitude))
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

            // need to account for steps.

        }
        //var angle = Utils2.FlattenVector()
        //.transform.rotation = newRot;
        player.position = newPos;
        totalForward = Vector3.zero;
    }
}
