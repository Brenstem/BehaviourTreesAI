using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAIBT", menuName = "BehaviorTrees/EnemyAIBT", order = 0)]
public class EnemyAIBT : ScriptableObject
{
    public Selector topNode;

    [HideInInspector] public Context blackboard;

    private bool _generated;

    public void ConstructBehaviourTree()
    {
        Debug.Log(_generated);

        if (!_generated)
        {
            IdleNode idleNode = ScriptableObject.CreateInstance<IdleNode>();
            idleNode.Construct(blackboard);
            RangeNode chaseRangeNode = ScriptableObject.CreateInstance<RangeNode>();
            chaseRangeNode.Construct(blackboard);
            ChaseNode chaseNode = ScriptableObject.CreateInstance<ChaseNode>();
            chaseNode.Construct(blackboard);

            Sequence chaseSequence = ScriptableObject.CreateInstance<Sequence>();
            chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });

            topNode = ScriptableObject.CreateInstance<Selector>();
            topNode.blackboard = blackboard;
            topNode.Construct(new List<AbstractNode> { chaseSequence, idleNode });


            //_generated = true;

            Debug.Log("BT generated");
        }
    }
}
