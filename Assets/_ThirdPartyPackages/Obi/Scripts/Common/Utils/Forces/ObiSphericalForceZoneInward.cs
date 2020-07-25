using UnityEngine;
using System;
namespace Obi
{
	public class ObiSphericalForceZoneInward : ObiExternalForce
	{

		public float outerRadius = 5;
		public float innerRadius = .1f;
		public float moveInsideSpeed = 5f;
		public float keepInsideForce = 5f;

		int frames = 0;
		int frameSkip = 5;

        public float Rand1 { get { return UnityEngine.Random.Range(-1f, 1f); } }

        public override void ApplyForcesToActor(ObiActor actor)
		{
			if (frames++ < frameSkip) return;
			frameSkip = 0;
			Matrix4x4 l2sTransform = actor.solver.transform.worldToLocalMatrix * transform.localToWorldMatrix;

			Vector4 center = l2sTransform.MultiplyPoint3x4(Vector4.zero);
			Vector4 forward = l2sTransform.MultiplyVector(Vector3.forward);

			// Calculate force intensity for each actor particle:
			for (int i = 0; i < actor.activeParticleCount; ++i)
			{

				Vector4 distanceVector = actor.solver.positions[actor.solverIndices[i]] - center;

				float sqrMag = distanceVector.sqrMagnitude;
				if (distanceVector.magnitude > outerRadius)
				{
					actor.solver.positions[actor.solverIndices[i]] = center + distanceVector.normalized * innerRadius + new Vector4(Rand1, Rand1, Rand1, Rand1) * .1f; // add a random bit so it doesn't put particles on top of each other
					//var force = distanceVector.normalized * -keepInsideForce;
					//if (actor.usesCustomExternalForces)
					//	actor.solver.wind[actor.solverIndices[i]] += force;
					//else
					//	actor.solver.externalForces[actor.solverIndices[i]] += force;
				}
				else if (distanceVector.magnitude < innerRadius)
				{

				}

	
				




			}
			Oni.SetParticleExternalForces(actor.solver.OniSolver, actor.solver.wind.GetIntPtr());
			Oni.SetParticleWinds(actor.solver.OniSolver, actor.solver.wind.GetIntPtr());

		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = new Color(0, 0.7f, 1, 1);
			Gizmos.DrawWireSphere(Vector3.zero, outerRadius);

			float turb = GetTurbulence(1);

			
			Gizmos.DrawLine(new Vector3(0, 0, -outerRadius * 0.5f) * turb, new Vector3(0, 0, outerRadius * 0.5f) * turb);
			Gizmos.DrawLine(new Vector3(0, -outerRadius * 0.5f, 0) * turb, new Vector3(0, outerRadius * 0.5f, 0) * turb);
			Gizmos.DrawLine(new Vector3(-outerRadius * 0.5f, 0, 0) * turb, new Vector3(outerRadius * 0.5f, 0, 0) * turb);
			
		}
	}
}

