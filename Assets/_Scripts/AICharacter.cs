using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
namespace Elemento
{

    public class AICharacter : MonoBehaviour
    {
        public GameObject ragdollPrefab;
        Animator anim => GetComponent<Animator>();
        float runSpeed = 3;

        public enum State
        {
            Idle = 0,
            Walking = 1,
            Running = 2,
            Attacking = 3,
            Ragdoll = 4,
            GetUpFrommFaceDown = 5,
            GetUpFromFaceUp = 6,
            Dead = 7
        }
        State state;
        State stateAfterGetUp;

        void ResumeSetStateAfterGetUp()
        {
            SetState(stateAfterGetUp);
        }
        public void SetState(State newState)
        {
            //if (state == newState) return;

            if (state == State.Ragdoll && newState != State.Ragdoll) // old state was ragdoll, so get up
            {
                transform.position = ragdoll.transform.position;
                transform.rotation = Quaternion.identity;
                /// ragdoll.transform.rotation;
                Destroy(ragdoll);
                visibleMesh.SetActive(true);
                if (Vector3.Angle(ragdoll.transform.GetChild(0).forward, Vector3.up) < 90)
                {
                    state = State.GetUpFromFaceUp;
                    anim.SetInteger("LocomotionState",6);
                    anim.SetTrigger("GetUpFromFaceUp");
                    returnToIdle = true;
                    returnToIdleAfterSeconds = 2f;


                }
                else
                {
                    state = State.GetUpFrommFaceDown;
                    anim.SetInteger("LocomotionState", 6);
                    anim.SetTrigger("GetUpFromFaceDown");
                    returnToIdle = true;
                    returnToIdleAfterSeconds = 2f;
                }
                return;
            }

            state = newState;
            switch (state)
            {
                case State.Ragdoll:
                    ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
                    visibleMesh.SetActive(false);
                    break;
                case State.Idle:
                    anim.SetInteger("LocomotionState", 0);
                    break;
                case State.Walking:
                    anim.SetInteger("LocomotionState", 1);
                    break;
                case State.Running:
                    anim.SetInteger("LocomotionState", 2);
                    break;

            }
        }



        GameObject ragdoll;
        public GameObject visibleMesh;

        // Start is called before the first frame update
        void Start()
        {
            
        }

 
        public void RunAway()
        {
            SetState(State.Running);
        }



        bool returnToIdle = false;
        float returnToIdleAfterSeconds = 0f;

        // Update is called once per frame
        void Update()
        {
            returnToIdleAfterSeconds -= Time.deltaTime;
            if (returnToIdle && returnToIdleAfterSeconds < 0)
            {
                returnToIdle = false;
                SetState(State.Idle);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SetState(State.Ragdoll);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SetState(State.Idle);
            }
            
        }
    }
}
