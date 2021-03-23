using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWeaponScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject firePFX;
    [SerializeField] GameObject environmentHitPFX;
    [SerializeField] Transform firePosition;
    [SerializeField] float damage;

    [SerializeField] DemoEnemyAI owner;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FireWeapon()
    {
        if (firePFX != null)
            Instantiate(firePFX, firePosition.position, Quaternion.LookRotation(firePosition.forward));

        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);

        if (Physics.Raycast(firePosition.position, firePosition.forward, 200f))
        {
            RaycastHit[] hit = Physics.RaycastAll(firePosition.position, firePosition.forward, 200f);
            Hit(hit);
        }

        if (owner != null)
            owner.animator.SetTrigger("Shoot");
    }

    public void Hit(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            if (!hit.collider.CompareTag(this.tag))
            {
                if (hit.collider.CompareTag("Environment"))
                {
                    if (environmentHitPFX != null)
                        Instantiate(environmentHitPFX, hit.point, Quaternion.LookRotation(hit.normal));

                    break;
                }
                else if (hit.collider.CompareTag("Enemy") || hit.collider.tag == "Player")
                {
                    hit.collider.GetComponent<Health>().Damage(damage, hit.normal);
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
