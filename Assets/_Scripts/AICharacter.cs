using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Unity.XR.Oculus;
using UnityEngine;
using static Elemento.Spells;

namespace Elemento
{

    public class AICharacter : MonoBehaviour, IForcePushActionHandler
    {
        public GameObject ragdollPrefab;
        Animator anim => GetComponent<Animator>();

        public static AICharacter Instance { get; private set; }

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
        State stateAfterReturn;



        void SetStateAfterSeconds(State st, float s)
        {
            setStateAfterSecondsTimer = s;
            setStateAfterSeconds = true;
            stateAfterReturn = st;
        }


        public void SetState(State newState)
        {
            //if (state == newState) return;
            if (state == State.Ragdoll && newState == State.Ragdoll)
            {
                return;
            }
            if (state == State.Ragdoll && newState != State.Ragdoll) // old state was ragdoll, so get up
            {
                ragdoll.SetActive(false);
                var rdi = ragdoll.GetComponent<RagdollInfo>();
                var trySpawnHere = ragdoll.GetComponent<RagdollInfo>().toe.position + Vector3.up * 0.2f;
                Utils2.DebugSphere(trySpawnHere, 0.3f, Color.red);
                //var maxTries = 10f;
                //var dirFromRagdollToSelf = (transform.position - ragdoll.GetComponent<RagdollInfo>().realCenter.position).normalized;
                //while (Physics.CheckSphere(trySpawnHere, 0.5f, ~LayerMask.NameToLayer("Ragdoll")) && maxTries-- > 0)
                //{
                //    Utils2.DebugSphere(trySpawnHere, 0.3f,Color.red); 
                //    var step = 0.2f;
                //    trySpawnHere -= dirFromRagdollToSelf * step;
                //}

                //Utils2.DebugSphere(trySpawnHere, 0.3f, Color.red);

                transform.position = trySpawnHere;

                //transform.rotation = Utils2.FlattenRotation(Quaternion.LookRotation(ragdoll.GetComponent<RagdollInfo>().realCenter.up,Vector3.up));


                // If the skeleton is on its belly, we want the forward to be head - toe
                // if skel is on back, we want fwd to be toe - head

                //transform.forward = ragdoll.GetComponent<RagdollInfo>().realCenter.up;
                /// ragdoll.transform.rotation;
                Destroy(ragdoll);
                visibleMesh.SetActive(true);
                GetComponent<Collider>().enabled = true;
                if (Vector3.Angle(rdi.toe.up, Vector3.up) < 90)
                {
                    // face up
                    state = State.GetUpFromFaceUp;
                    anim.SetInteger("LocomotionState",6);
                    anim.SetTrigger("GetUpFromFaceUp");
                    SetStateAfterSeconds(State.Idle, 2);
                    var dirFromHeadTotoe = Utils2.FlattenVector(rdi.toe.position - rdi.head.position).normalized;
                    transform.forward = dirFromHeadTotoe;
                }
                else
                {

                    state = State.GetUpFrommFaceDown;
                    anim.SetInteger("LocomotionState", 6);
                    anim.SetTrigger("GetUpFromFaceDown");
                    SetStateAfterSeconds(State.Idle, 4);
                    var dirFromToeToHead = Utils2.FlattenVector( rdi.head.position - rdi.toe.position).normalized;
                    transform.forward = dirFromToeToHead;
                }

                return;
            }

            state = newState;
            switch (state)
            {
                case State.Ragdoll:
                    ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
                    visibleMesh.SetActive(false);
                    GetComponent<Collider>().enabled = false;
                    SetStateAfterSeconds(State.Idle, 4);

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
            Instance = this;   
        }

 
        public void RunAway()
        {
            SetState(State.Running);
        }



        bool setStateAfterSeconds = false;
        float setStateAfterSecondsTimer = 0f;

        // Update is called once per frame
        void Update()
        {
            setStateAfterSecondsTimer -= Time.deltaTime;
            if (setStateAfterSeconds && setStateAfterSecondsTimer < 0)
            {
                setStateAfterSeconds = false;
                SetState(stateAfterReturn);
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

        public void IOnForcePushAction(Vector3 dir, float forceAmount = 50)
        {

            SetState(State.Ragdoll);
            foreach (var rb in ragdoll.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddForce(dir * forceAmount);
            }
        }
    }
}
