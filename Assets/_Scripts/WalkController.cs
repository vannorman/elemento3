using UnityEngine;
namespace Elemento
{

    public class WalkController : MonoBehaviour
    {

        public static WalkController Instance;
        Transform player;
        public bool ghostMode = true;
        private void Start()
        {
            Instance = this;
            player = FindObjectOfType<OVRCameraRig>()?.transform;
        }

        static bool leftHandPoint = false;
        static bool rightHandPoint = false;
        static Vector3 leftHandFwd = Vector3.zero;
        static Vector3 rightHandFwd = Vector3.zero;

        public static void AddWalkForward(Vector3 fwd, HandPoseTracker hand)
        {
            if (hand.handType == HandType.Left)
            {
                leftHandPoint = true;
                leftHandFwd = fwd;
            }
            else if (hand.handType == HandType.Right)
            {
                rightHandPoint = true;
                rightHandFwd = fwd;
            }

        }

        public float speed = 1;
        private void Update()
        {
            if (!leftHandPoint || !rightHandPoint)
            {
                // both hands were not pointing, so reset and return
                leftHandPoint = false;
                rightHandPoint = false;
                leftHandFwd = Vector3.zero;
                rightHandFwd = Vector2.zero;
            }
            var flatFwd = Utils2.FlattenVector(leftHandFwd + rightHandFwd);
            var flatFwdSelf = Utils2.FlattenVector(player.forward);
            var totalForward = leftHandFwd + rightHandFwd;
            if (Vector3.Angle(flatFwd, flatFwdSelf) > 45)
            {
                // turn
                var rotSpeed = 1;
                var newDir = Vector3.RotateTowards(player.forward, totalForward, Time.deltaTime * rotSpeed, 1f);
                var newRot = Quaternion.LookRotation(newDir, Vector3.up);
                var e = player.transform.rotation.eulerAngles;
                player.transform.rotation = Quaternion.Euler(e.x,newRot.y,e.y);
            }
       
            // walk fwd
            var newPos = player.position + totalForward * Time.deltaTime * speed;
            
            var stepHeight = 0.25f;
            if (!ghostMode)
            { 
                // If hit wall, move new pt backward
                if (Physics.Raycast(player.position + Vector3.up * stepHeight, totalForward, out var hit, totalForward.magnitude, LayerMask.NameToLayer("Trigger")))
                {
                    var dirTowardsWall = hit.point - player.position;
                    newPos = hit.point - dirTowardsWall * totalForward.magnitude;
                    return; // just don't
                }
                if (Physics.Raycast(newPos + Vector3.up * .2f, Vector3.down, out var hit2, LayerMask.NameToLayer("Trigger")))
                {
                    newPos.y = hit2.point.y;
                }
           
            }
            //var angle = Utils2.FlattenVector()
            //.transform.rotation = newRot;
            player.position = newPos;
            leftHandPoint = false;
            rightHandPoint = false;
            leftHandFwd = Vector3.zero;
            rightHandFwd = Vector2.zero;

        }
    }
}
