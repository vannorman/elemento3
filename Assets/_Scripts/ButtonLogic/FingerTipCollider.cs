using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elemento
{ 
    public class FingerTipCollider : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            //foreach (var col in Physics.OverlapSphere(transform.position, 0.1f, LayerMask.NameToLayer(LayerManager.Button)))
            //{
            //    Debug.Log("Col:" + col.name + ", layer;" + LayerMask.LayerToName(col.gameObject.layer));
            //    col.GetComponent<Button>().TouchWithFingerTip(this);
            //}

            //foreach (var col in Physics.OverlapSphere(transform.position, 0.1f))
            //{
            //    Debug.Log("2 Col:" + col.name + ", layer;" + LayerMask.LayerToName(col.gameObject.layer));
            //    col.GetComponent<Button>().TouchWithFingerTip(this);
            //}
        }
    }
}
