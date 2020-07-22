﻿using Elemento;
using OVR;
using OVRTouchSample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ForcePushController : MonoBehaviour
{
    public float minForce = 50;
    public float maxForce = 400f;
    public float secondsToCharge = 4;
    public GameObject particleSystemPrefab;
    public static ForcePushController Instance { get; private set; }

    public class ForcePushSpellEffect
    {
        public ParticleSystem fx;
        public float force;
        public bool active;
        public ForcePushSpellEffect(ParticleSystem _fx, float _force, bool _active)
        {
            fx = _fx;
            force = _force;
            active = _active;
        }
    }

    private void Start()
    {
        Instance = this;
    }

    Dictionary<HandPoseTracker, ForcePushSpellEffect> fx = new Dictionary<HandPoseTracker, ForcePushSpellEffect>();

    internal static void BuildUpForceStatic(HandPoseTracker handTracker)
    {
        Instance.BuildUpForce(handTracker);
    }

    void BuildUpForce(HandPoseTracker handTracker)
    { 
        // happens every frame the power up pose is held
        if (!fx.ContainsKey(handTracker))
        {
            // init particles for this hand
            fx.Add(handTracker, 
                new ForcePushSpellEffect(Instantiate(particleSystemPrefab).GetComponent<ParticleSystem>(), minForce, false)); // mixing static ref with instance. oh well
        }
        var cur = fx[handTracker];
        if (!cur.active)
        { 
            cur.active = true;
            var shape = fx[handTracker].fx.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.1f;
            if (toCancel.ContainsKey(handTracker))
            {
                toCancel.Remove(handTracker);
            }

        }
        cur.force += (maxForce - minForce)/secondsToCharge * Time.deltaTime;
        var em = cur.fx.emission;
        em.rateOverTime = cur.force;
        em.enabled = true;
        var main = cur.fx.main;
        main.startSize = cur.force / 8000f;
        main.startSpeed = -cur.force / 16000f;
        var distFromStubToPalmOffset = 0.06f;
        cur.fx.transform.position = handTracker.hand.position + handTracker.WristForwardDirection * distFromStubToPalmOffset;
    }

    static Dictionary<HandPoseTracker, float> toCancel = new Dictionary<HandPoseTracker, float>();

    public static void Cancel(HandPoseTracker handTracker)
    {
        var cancelTime = Spells.forcePush.sequence[Spells.forcePush.sequence.Count - 2].time;
        if (toCancel.ContainsKey(handTracker))
        {
            toCancel[handTracker] = cancelTime;
        }
        else
        { 
            toCancel.Add(handTracker, cancelTime);

        }
        Instance.fx[handTracker].active = false;

    }

    

    public void PushNowFx(HandPoseTracker handTracker)
    {
       
            var curFx = fx[handTracker].fx;
            var shape = curFx.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.radius = 0.02f;
           
            var main = curFx.main;
            main.startSpeed = 2.0f;
            fx[handTracker].fx.transform.forward = handTracker.PalmForwardDirection;

        
    }

    void Update()
    {
        if (toCancel.Count > 0)
        { 
            foreach (var kvp in fx)
            {
                if (toCancel.ContainsKey(kvp.Key))
                {
                    toCancel[kvp.Key] -= Time.deltaTime; // count this down to zero before canceling
                    if (toCancel[kvp.Key] < 0)
                    {
                        toCancel.Remove(kvp.Key);
                        CancelFx(kvp.Key);
                    }
                }
            }
        }
    }

    private void CancelFx(HandPoseTracker key)
    {
        // turn particles off and reset force to zero
        var em = fx[key].fx.emission;
        em.enabled = false;
        fx[key].force = minForce;
        fx[key].active = false;
    }

    internal float GetForceAmountForHand(HandPoseTracker handTracker)
    {
        return fx[handTracker].force;
    }

    internal float GetRangeForHand(HandPoseTracker handTracker)
    {
        return fx[handTracker].force/12f;
    }

    internal static void PushNow(HandPoseTracker handTracker)
    {
        var effectAngle = 30;
        Physics.SphereCastAll(handTracker.hand.position, 5f, handTracker.PalmForwardDirection,Instance.GetRangeForHand(handTracker)).ToList()
            .Where(x => Vector3.Angle(x.transform.position-Camera.main.transform.position,Camera.main.transform.forward) < effectAngle).ToList()
            .ForEach(x =>
            {
                if (x.collider.GetComponent<IForcePushActionHandler>() is var ai && ai != null)
                {
                    ai.OnForcePushAction(ForcePushController.Instance.GetForceAmountForHand(handTracker)); // a bit convoluted logic, but gets the job done
                }
                if (x.collider.GetComponent<Rigidbody>() is var rb && rb != null)
                {
                    if (rb.isKinematic && rb.GetComponent<AllowKinematicToggle>())
                    {
                        rb.isKinematic = false;
                    }
                    if (!rb.isKinematic)
                    {
                        rb.AddForce(Utils2.FlattenVector(handTracker.PalmForwardDirection) * Instance.GetForceAmountForHand(handTracker));
                    }
                }
            });

        Utils2.SpellDebug("Force push");
        int numDebugSpheres = Mathf.RoundToInt(ForcePushController.Instance.GetForceAmountForHand(handTracker) / 50);
        for (var i = 0; i < numDebugSpheres; i++)
        {
            Utils2.DebugSphere(handTracker.hand.position + handTracker.PalmForwardDirection * i * .1f, 0.1f, Color.blue, 0.5f);
        }
        Utils2.DebugSphere(handTracker.hand.position + handTracker.PalmForwardDirection * numDebugSpheres * .1f, 0.2f, Color.green, 0.5f);
        Instance.PushNowFx(handTracker);

    

    }
}