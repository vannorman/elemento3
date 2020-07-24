using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public float hitPoints = 5;
    public GameObject deadReplacementPrefab;
    public GameObject deathFx;
    public void TakeDamage(float damageAmount)
    {
        hitPoints -= damageAmount;
        if (hitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (deadReplacementPrefab)
        {
            var dr = Instantiate(deadReplacementPrefab, transform.position, transform.rotation);
            dr.transform.SetParent(transform.parent);
        }

        if (deathFx)
        {
            var dx = Instantiate(deathFx, transform.position, transform.rotation);
            dx.transform.SetParent(transform.parent);
        }
        Destroy(this.gameObject);
    }
}
