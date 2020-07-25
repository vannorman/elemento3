using AquariusMax.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    public GameObject prefab;
    public float radius;
    public float arc;
    public float density;
    public bool make = false;
    // Start is called before the first frame update
    void MakeNow()
    {
        var points = Utils2.GetCircleOfPoints(arc, radius, density);
        foreach (var p in points)
        {
            var a = Instantiate(prefab, transform.position + p, Quaternion.identity);
            a.transform.LookAt(transform.position);
            a.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (make)
        {
            make = false;
            MakeNow();
        }
    }
}
