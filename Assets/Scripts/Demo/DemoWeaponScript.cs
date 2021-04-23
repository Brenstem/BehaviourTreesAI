﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWeaponScript : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject firePFX;
    [SerializeField] private GameObject environmentHitPFX;
    [SerializeField] private Transform firePosition;
    [SerializeField] private float damage;
    [SerializeField] private List<AudioClip> environmentHitClips;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private LayerMask environmentLayerMask;

    [SerializeField] DemoEnemyAI owner;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FireWeapon()
    {
        Collider[] hearingHits = Physics.OverlapSphere(transform.position, 100f, enemyLayerMask);

        foreach (var hit in hearingHits)
        {
            if (hit.GetComponent<DemoEnemyAI>() != null)
            {
                hit.GetComponent<DemoEnemyAI>().TurnTowards(transform, 10f, 0.1f);
            }
        }

        if (firePFX != null)
            Instantiate(firePFX, firePosition.position, Quaternion.LookRotation(firePosition.forward));

        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);

        if (gameObject.CompareTag("Player"))
        {
            RaycastHit hit;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Physics.Raycast(mousePosition, Camera.main.transform.forward, out hit, Mathf.Infinity, environmentLayerMask);


            //firePosition.transform.LookAt(hit.point);

            firePosition.transform.LookAt(new Vector3(hit.point.x, firePosition.transform.position.y, hit.point.z));

            if (Physics.Raycast(firePosition.position, firePosition.forward, 200f))
            {
                RaycastHit[] hits = Physics.RaycastAll(firePosition.position, firePosition.forward, 200f);

                Hit(hits);
            }

            Debug.DrawRay(firePosition.position, firePosition.forward * 10, Color.red, 5f);
        }
        else if (Physics.Raycast(firePosition.position, firePosition.forward, 200f))
        {
            RaycastHit[] hits = Physics.RaycastAll(firePosition.position, firePosition.forward, 200f);

            Hit(hits);
        }

        if (owner != null)
            owner.animator.SetTrigger("Shoot");
    }

    public void Hit(RaycastHit[] hits)
    {
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        foreach (var hit in hits)
        {

            if (!hit.collider.CompareTag(this.tag))
            {

                if (hit.collider.CompareTag("Enemy") || hit.collider.tag == "Player")
                {
                    hit.collider.GetComponent<Health>().Damage(damage, hit.normal, hitClip);
                    break;
                }
                else if (hit.collider.CompareTag("Environment"))
                {
                    if (environmentHitPFX != null)
                    {
                        GameObject instance = Instantiate(environmentHitPFX, hit.point, Quaternion.LookRotation(hit.normal));

                        if (environmentHitClips.Count > 0)
                        {
                            int i = Random.Range(0, environmentHitClips.Count);
                            instance.GetComponent<AudioSource>().PlayOneShot(environmentHitClips[i]);
                        }
                    }
                    break;
                }
                else if (hit.collider.CompareTag("EnemyNearMissRadius"))
                {
                    hit.collider.GetComponentInParent<Health>().Damage(0);
                }
            }
        }
    }
}
