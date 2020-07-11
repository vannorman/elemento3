using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectDestructor : MonoBehaviour
{
    public bool autoDestruct = false;
    public float autoDestructSeconds = 2f;
    private void Start()
    {
        if (autoDestruct)
        {
            DestroyAfterSeconds(autoDestructSeconds);
        }
    }

    public void DestroyAfterSeconds(float seconds)
    {
        Invoke("DestroyNow", seconds);
    }

    void DestroyNow()
    {
        Destroy(this.gameObject);
    }
}
