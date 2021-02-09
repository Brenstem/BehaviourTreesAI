using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWeaponScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePosition;
    [SerializeField] float damage;
    public void FireWeapon()
    {
        GameObject instance = Instantiate(projectile, firePosition.position, firePosition.rotation);
        instance.tag = this.tag;
        instance.GetComponent<DemoProjectile>().damage = damage;
    }
    public void FireWeapon(Quaternion parentRotation)
    {
        GameObject instance = Instantiate(projectile, firePosition.position, parentRotation);
        instance.tag = this.tag;
        instance.GetComponent<DemoProjectile>().damage = damage;
    }
}
