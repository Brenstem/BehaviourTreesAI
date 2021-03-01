using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boom : Explosive
{
    [SerializeField] float throwPowerBase;
    [SerializeField] float throwPowerVariance;
    [SerializeField] float throwAngleBase;
    [SerializeField] float throwAngleVariance;


    Rigidbody rigidbody;

    protected override void Awake()
    {
        myLayer = this.gameObject.layer;
        rigidbody = GetComponent<Rigidbody>();

        //TODO make this good
        Vector3 throwVector = transform.forward;

        throwVector = Quaternion.AngleAxis(throwAngleBase, -transform.right) * throwVector;

        throwVector = throwVector.normalized * throwPowerBase;

        Throw(throwVector);

        StartCoroutine(TriggerDelay());
    }

    public void Throw(Vector3 throwVector)
    {
        rigidbody.velocity = throwVector;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void Explode()
    {
        //Instantiate(explosionVFX, transform.position, Quaternion.Euler(-90, 0, 0));

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
        Destroy(this.gameObject);
    }
}
