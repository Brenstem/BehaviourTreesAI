using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Explosive
{
    [SerializeField] protected LayerMask triggerLayers;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material activeMaterial;

    [SerializeField] MeshRenderer light;

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (triggerLayers == (triggerLayers | 1 << other.gameObject.layer))
        {
            StartCoroutine(TriggerDelay());
        }
    }
    protected override IEnumerator TriggerDelay()
    {
        light.material = activeMaterial;
        yield return new WaitForSeconds(triggerDelayTime);
        Explode();
    }

    public override void Explode()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.Euler(-90, 0, 0));

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
