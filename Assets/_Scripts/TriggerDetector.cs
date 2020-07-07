using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public delegate void OnTriggerEntered(Collider c);
    public OnTriggerEntered onTriggerEntered;

    public delegate void OnTriggerExited(Collider c);
    public OnTriggerExited onTriggerExited;

    public void OnTriggerEnter(Collider other)
    {
        onTriggerEntered?.Invoke(other);
    }

    public void OnTriggerExit(Collider other)
    {
        onTriggerExited?.Invoke(other);
    }
}
