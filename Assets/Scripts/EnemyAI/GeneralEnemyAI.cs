using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralEnemyAI : BaseAI
{
    [SerializeField] private GameObject soldier;

    public DemoEnemyAI SoldierAI { get { return soldierAI; } }

    private DemoEnemyAI soldierAI;

    new void Awake()
    {
        base.Awake();

        soldierAI = soldier.GetComponent<DemoEnemyAI>();

        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("TookDamage", false); });
        behaviourTree.context.localData.Add<Vector3>(() => { return new BlackBoardProperty<Vector3>("positionToGoTo", Vector3.zero); });
    }

    private void Update()
    {
        float moveX = agent.velocity.x;
        float moveZ = agent.velocity.z;

        animator.SetFloat("moveZ", moveZ);
        animator.SetFloat("moveX", moveX);
    }

    private void FixedUpdate()
    {
        if (!GetComponent<Health>().isDead)
        {
            behaviourTree.topNodeInstance.Evaluate();
        }
    }
}
