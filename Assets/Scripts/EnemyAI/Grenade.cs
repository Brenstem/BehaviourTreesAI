﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosive
{
    Rigidbody rigidbody;

    private AudioSource audioSource;

    protected override void Awake()
    {
        myLayer = this.gameObject.layer;
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(TriggerDelay());
    }

    public void Throw(float angle, float power)
    {
        Vector3 throwVector = transform.forward;

        throwVector = Quaternion.AngleAxis(angle, -transform.right) * throwVector;

        throwVector = throwVector.normalized * power;

        rigidbody.velocity = throwVector;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void Explode()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.Euler(-90, 0, 0));

        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
            StartCoroutine(PlayExplosionSound());
        }

        Collider[] hitTargets = Physics.OverlapSphere(transform.position, explosionRadius, damageLayers | 1 << myLayer);
        foreach (Collider target in hitTargets)
        {
            if (target.gameObject.layer == myLayer)
            {
                target.GetComponent<Explosive>().StartCoroutine(target.GetComponent<Explosive>().ExplosionDelay());
            }
            else
            {
                target.GetComponent<Health>().Damage(damage);
            }
        }
    }

    private IEnumerator PlayExplosionSound()
    {
        yield return new WaitUntil(() => { return !audioSource.isPlaying; });
        Destroy(this.gameObject);
    }
}
