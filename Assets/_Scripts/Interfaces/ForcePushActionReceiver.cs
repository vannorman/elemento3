using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePushActionReceiver : MonoBehaviour, IForcePushActionHandler
{
    // Duplicated in AICharacter, but with different effects. Override would work but no longer an interface. #mattquestions
    public void IOnForcePushAction(Vector3 dir, float force)
    {
        foreach (var rb in transform.GetComponentsInChildren<Rigidbody>())
        { 
            rb.AddForce(dir*force);
            
        }
    }
}
