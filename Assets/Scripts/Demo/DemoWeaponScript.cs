using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWeaponScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
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
        GameObject instance = Instantiate(projectile, firePosition.position, firePosition.rotation);
        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);
        instance.tag = this.tag;
        instance.GetComponent<DemoProjectile>().damage = damage;
        if(owner != null)
            owner.animator.SetTrigger("Shoot");
    }

    public void FireWeapon(Quaternion parentRotation)
    {
        GameObject instance = Instantiate(projectile, firePosition.position, parentRotation);
        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);
        instance.tag = this.tag;
        instance.GetComponent<DemoProjectile>().damage = damage;
        if(owner != null)
            owner.animator.SetTrigger("Shoot");
    }
}
