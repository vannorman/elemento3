using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Oculus.Platform;

namespace Elemento
{
    public class LaserController : MonoBehaviour // :SpellController?
    {
        public GameObject laserPrefab;
        static Vector3 startScale;
        public GameObject burnPrefab;
        static LaserInfo laser;
        public static LaserController Instance;
        public float scaleFactor = 0.1f;
        public ParticleSystem fireParticleFx;



        // Start is called before the first frame update
        void Start()
        {
            laser = Instantiate(laserPrefab).GetComponent<LaserInfo>();
            startScale = laser.transform.localScale;
            Instance = this;

            if (GameObject.FindObjectOfType<DebugFPS>() is var fps && fps != null)
            {
                fps.onMouseButtonHelld_0 += MouseButtonHeld;
                fps.onMouseButtonUp_0 += MouseButtonUp;
            }


        }

        private void MouseButtonUp(DebugFPS fps)
        {
            Cancel();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                var emitParams = new ParticleSystem.EmitParams();
                emitParams.position = Vector3.zero;
                fireParticleFx.Emit(emitParams, 1);
            }
        }

        private void MouseButtonHeld(DebugFPS fps)
        {
            var start = fps.transform.position + (-fps.transform.right - fps.transform.up) * .5f;
            DrawLaser(start, start + fps.transform.forward, 0.05f);
        }

        internal static void OnLaserActivated(HandPoseTracker currentHand)
        {
            var pinchPosition = (currentHand._indexTip.position + currentHand._thumbTip.position) / 2f;
            var widthBetweenFingers = (currentHand._indexTip.position - currentHand._thumbTip.position).magnitude;
            Instance.DrawLaser(Camera.main.transform.position, pinchPosition, widthBetweenFingers);
        }


        public void DrawLaser(Vector3 origin, Vector3 pinchPosition, float widthBetweenFingers)
        { 
            var laserDirection = (pinchPosition - origin).normalized;
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

                if (hit.collider.GetComponent<DamageReceiver>() is DamageReceiver dr)
                {
                    float laserDamage = 1;
                    dr.TakeDamage(laserDamage);
                    Instance.DamageFX(hit.point, hit.collider.transform);
                }
            }
            else
            {
                var module = laser.sparks.emission;
                module.rateOverTime = 0;
                laser.endFlare.gameObject.SetActive(false);
            }

        }

        private void DamageFX(Vector3 point, Transform transform)
        {
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.position = point;
            fireParticleFx?.Emit(emitParams,1);
        }

        internal static void Cancel()
        {
            laser.gameObject.SetActive(false);

        }


    }

}
