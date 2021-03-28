using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Movement", "GuessPlayerPosition")]
public class GuessPlayerPosition : Action
{
    [Header("Node variables")]
    [SerializeField] float influence;

    float ownerSpeed;
    Transform ownerTransform;
    Transform playerTransform;
    Rigidbody playerRigidbody;

    public override void Construct()
    {
        ownerTransform = context.owner.transform;
        ownerSpeed = context.owner.agent.speed;
        playerTransform = context.globalData.player.transform;
        playerRigidbody = context.globalData.player.GetComponent<Rigidbody>();

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //Vector3 guessedPlayerPosition = playerTransform.position + playerRigidbody.velocity * Vector3.Distance(ownerTransform.position, playerTransform.position) * ownerSpeed * influence;

            Vector3 guessedPlayerPosition = playerTransform.position + playerRigidbody.velocity * influence;

            context.localData.Set<Vector3>("LastKnownPlayerPosition", guessedPlayerPosition);

            Debug.DrawLine(playerTransform.position, guessedPlayerPosition, Color.blue, 10);


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
