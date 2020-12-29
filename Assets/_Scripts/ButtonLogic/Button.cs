using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elemento
{ 
    public class Button : MonoBehaviour
    {
        public GameObject buttonGfx;
        public GameObject highlightFx;
        private GameObject entryPoint;

        [Serializable]
        public class InteractableStateArgsEvent : UnityEvent<InteractableStateArgs>
        {
        }

        public class InteractableStateArgs : EventArgs
        {
            public readonly InteractableState OldInteractableState;
            public readonly InteractableState NewInteractableState;

            public InteractableStateArgs(InteractableState newInteractableState, InteractableState oldState)
            {
                NewInteractableState = newInteractableState;
                OldInteractableState = oldState;
            }
        }

        float touchTime = 0;
        internal void TouchWithFingerTip(FingerTipCollider finger)
        {
            currentFinger = finger.gameObject;
            Debug.Log("Touch");
            touchTime = .1f;
        }

        public enum InteractableState
        {
            Ready,
            Pressed
        }


        public InteractableStateArgsEvent InteractableStateChanged;

        private void Start()
        {
            entryPoint = new GameObject();
            entryPoint.transform.SetParent(transform);
        }





        bool fingerTouching = false;
        GameObject currentFinger;
        private float triggerDepth = 0.006f;

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<FingerTipCollider>() is FingerTipCollider finger || other.gameObject.name == "Hand_Index3_CapsuleCollider")
            {
                Debug.Log("trigger:" + other.name);
                entryPoint.transform.position = other.transform.position;
                fingerTouching = true;
                currentFinger = other.gameObject;
            }
            else
            {
                //if (other.GetComponent<CapsuleCollider>())
                //    Debug.Log("Another capsule without.");
                //else
                //    Debug.Log("Not a fingertip collider.");
            }
            // if triggered by a fingertip, enable button pressing
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "Hand_Index3_CapsuleCollider")
            {
                Debug.Log("Exit");
                fingerTouching = false;
                currentState = InteractableState.Ready;
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (fingerTouching)
            {
                highlightFx.SetActive(true);
                float depth = (currentFinger.transform.position - entryPoint.transform.position).magnitude;
                if (depth >= triggerDepth)
                {
                    ExecuteButtonPress();
                }
                else 
                {
                    Debug.Log("press depth fail;" + depth);
                }
            }
            else 
            {
                highlightFx.SetActive(false);
            }
        }

        InteractableState currentState = InteractableState.Ready;

        private void ExecuteButtonPress()
        {
            Debug.Log("PRESS. curstate:" + currentState);
            if (currentState == InteractableState.Ready)
            { 
                InteractableStateChanged?.Invoke(new InteractableStateArgs(InteractableState.Pressed, currentState));
                currentState = InteractableState.Pressed;
            }
        }
    }
}
