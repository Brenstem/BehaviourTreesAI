using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] protected GameObject explosionVFX;
    [SerializeField] protected float explosionRadius;
    [SerializeField] protected LayerMask damageLayers;
    [SerializeField] protected float triggerDelayTime;
    [SerializeField] protected float explosionDelayTime;
    [SerializeField] protected float damage;
    //[SerializeField] Material defaultMaterial;
    //[SerializeField] Material activeMaterial;

    protected int myLayer;

    protected virtual void Awake()
    {
        myLayer = this.gameObject.layer;
    }

    protected virtual IEnumerator TriggerDelay()
    {
        yield return new WaitForSeconds(triggerDelayTime);
        Explode();
    }

    public virtual IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(explosionDelayTime);
        Explode();
    }

    public virtual void Explode()
    {
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
