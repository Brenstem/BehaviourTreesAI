using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] float throwPowerBase;
    [SerializeField] float throwPowerVariance;
    [SerializeField] float throwAngleBase;
    [SerializeField] float throwAngleVariance;

    private DemoEnemyAI owner;


    private void Start()
    {
        owner = GetComponentInParent<DemoEnemyAI>();
    }

    private void ThrowGrenade()
    {
        GameObject grenadeInstance = Instantiate(grenadePrefab, spawnPosition.position, owner.transform.rotation);

        float throwAngle = throwAngleBase + Random.Range(-throwAngleVariance, throwAngleVariance);
        float throwPower = throwPowerBase + Random.Range(-throwPowerVariance, throwPowerVariance);

        grenadeInstance.GetComponent<Grenade>().Throw(throwAngle, throwPower);
    }
}
