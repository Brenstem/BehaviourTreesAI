using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Attacks", "SuicideBombNode")]
public class SuicideBombNode : Action
{
    [Header("Node variables")]
    [SerializeField] private GameObject grenadePrefab;


    public override void Construct()
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //Write your behaviour code here!

            GameObject instance = Instantiate(grenadePrefab, context.owner.transform.position + new Vector3(0, 2f, 1f), Quaternion.Euler(Vector3.zero));
            instance.GetComponent<Grenade>().Explode();

            NodeState = NodeStates.SUCCESS;

            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
