using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class LaserController : MonoBehaviour
{
    public GameObject laserPrefab;
    static Vector3 startScale;
    public GameObject burnPrefab;
    static LaserInfo laser;
    static LaserController Instance;
    public float scaleFactor = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        laser = Instantiate(laserPrefab).GetComponent<LaserInfo>();
        startScale = laser.transform.localScale;
        Instance = this;
    }

    internal static void DrawLaser(Vector3 pinchPosition, float widthBetweenFingers)
    {
        var laserDirection = (pinchPosition - Camera.main.transform.position).normalized;
        laser.gameObject.SetActive(true);
        laser.transform.position = pinchPosition;
        laser.transform.forward = laserDirection;

        // we'll say 0.1 meters between finger width is the "normal" size for the laser. If you pinch smaller, laser gets smaller.
        laser.transform.localScale = new Vector3(startScale.x * widthBetweenFingers * Instance.scaleFactor, startScale.y * widthBetweenFingers * Instance.scaleFactor, startScale.z);

        var range = 25f;
        if (Physics.Raycast(pinchPosition, laserDirection, out var hit, range))
        {
            var module = laser.sparks.emission;
            module.rateOverTime = 50;
            laser.endFlare.gameObject.SetActive(true);
            laser.endFlare.transform.position = hit.point;
            laser.sparks.transform.position = hit.point;
            var rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            var burnMark = Instantiate(Instance.burnPrefab, hit.point + hit.normal * Random.Range(0.001f, 0.003f), rot);
            burnMark.transform.SetParent(hit.collider.transform);
        }
        else
        {
            var module = laser.sparks.emission;
            module.rateOverTime = 0;
            laser.endFlare.gameObject.SetActive(false);
        }
      
    }


    internal static void Cancel()
    {
        laser.gameObject.SetActive(false);

    }


}
