using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask triggerLayers;
    [SerializeField] LayerMask damageLayers;
    [SerializeField] float triggerDelayTime;
    [SerializeField] float explosionDelayTime;
    [SerializeField] float damage;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material activeMaterial;

    [SerializeField] MeshRenderer light;

    int myLayer;

    private void Awake()
    {
        myLayer = this.gameObject.layer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerLayers == (triggerLayers | 1 << other.gameObject.layer))
        {
            StartCoroutine(TriggerDelay());
        }
    }

    IEnumerator TriggerDelay()
    {
        light.material = activeMaterial;
        yield return new WaitForSeconds(triggerDelayTime);
        Explode();
    }

    public IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(explosionDelayTime);
        Explode();
    }

    public void Explode()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.Euler(-90, 0, 0));

        Collider[] hitTargets = Physics.OverlapSphere(transform.position, explosionRadius, damageLayers | 1 << myLayer);
        foreach (Collider target in hitTargets)
        {
            if (target.gameObject.layer == myLayer)
            {
                target.GetComponent<Mine>().StartCoroutine(target.GetComponent<Mine>().ExplosionDelay());
            }
            else
            {
                target.GetComponent<Health>().Damage(damage);
            }
        }
        Destroy(this.gameObject);
    }
}
