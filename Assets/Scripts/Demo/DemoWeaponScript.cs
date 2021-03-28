using System.Collections;
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
                hit.GetComponent<DemoEnemyAI>().TurnTowards(transform);
            }

            // Action.RaiseInterruptEvent(new InterruptEventArgs(hit.GetComponent<BaseAI>().GetBehaviourTreeInstance().context.id));
        }

        if (firePFX != null)
            Instantiate(firePFX, firePosition.position, Quaternion.LookRotation(firePosition.forward));

        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);

        if (Physics.Raycast(firePosition.position, firePosition.forward, 200f))
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
