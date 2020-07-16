using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestBurn_C : MonoBehaviour
{

    public bool UseLifeTime = false;
    public float lifetime = 2f;

    void Start()
    {
        if (UseLifeTime)
            Destroy(gameObject, lifetime);
    }
}