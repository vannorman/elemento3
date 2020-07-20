using Elemento;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePushController : MonoBehaviour
{
    public static float ForceAmount = 0f;
    public float maxForce = 400f;
    public ParticleSystem forceParticles;

    public static ForcePushController Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }
    internal static void BuildUpForce(HandPoseTracker handTracker)
    {
        ForceAmount += 100 * Time.deltaTime;
        var em = Instance.forceParticles.emission;
        em.rateOverTime = ForceAmount;
        em.enabled = true;
        var main = Instance.forceParticles.main;
        main.startSize = ForceAmount / 8000f;
        main.startSpeed = ForceAmount / 16000f;
        var distFromStubToPalmOffset = 0.06f;
        Instance.forceParticles.transform.position = handTracker.hand.position + handTracker.WristForwardDirection * distFromStubToPalmOffset;
    }

    public static void Cancel()
    {
        var em = Instance.forceParticles.emission;
        em.enabled = false;
        ForceAmount = 0;
    }
}
