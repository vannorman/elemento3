using Elemento;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManaPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    int frameSkip = 5;
    int frames = 0;
    // Update is called once per frame
    void Update()
    {
        if (frames++ < frameSkip) return;
        frames = 0;
        foreach (var col in Physics.OverlapSphere(transform.position, 1f))
        {
            Debug.Log("hit:" + col.name);
            if (col.GetComponent<ManaPickup>() is var mp && mp != null)
            {
                ManaController.Instance.PickupMana(mp.manaAmount);
                Destroy(mp.gameObject);
            }
        }
    }
}
