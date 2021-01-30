using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWeaponScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePosition;
    public void FireWeapon()
    {
        GameObject instance = Instantiate(projectile, firePosition.position, firePosition.rotation);
        instance.tag = this.tag;
    }
}
