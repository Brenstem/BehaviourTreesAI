using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Attacks", "ThrowGrenade")]
public class ThrowGrenade : Action
{
    [Header("Node variables")]

    [SerializeField] GameObject grenadePrefab;
    [SerializeField] float throwPowerBase;
    [SerializeField] float throwPowerVariance;
    [SerializeField] float throwAngleBase;
    [SerializeField] float throwAngleVariance;

    Transform spawnPosition;


    public override void Construct()
    {
        spawnPosition = context.owner.transform.Find("GrenadeSpawnPosition");

        _constructed = true;
    }

    //TODO den skapas i AIn s� de kolliderar med varandra, m�ste skapa den p� en annan position
    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            GameObject grenadeInstance = Instantiate(grenadePrefab, spawnPosition.position, context.owner.transform.rotation);

            float throwAngle = throwAngleBase + Random.Range(-throwAngleVariance, throwAngleVariance);
            float throwPower = throwPowerBase + Random.Range(-throwPowerVariance, throwPowerVariance);

            grenadeInstance.GetComponent<Grenade>().Throw(throwAngle, throwPower);

            NodeState = NodeStates.SUCCESS;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            NodeState = NodeStates.FAILURE;
            return NodeState;
        }
    }
}
