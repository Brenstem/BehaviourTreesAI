using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAIBT", menuName = "BehaviorTrees/EnemyAIBT", order = 0)]
public class EnemyAIBT : ScriptableObject
{
    public Selector topNode;

    [HideInInspector] public Context blackboard;

    private bool _generated;

    private void OnEnable()
    {
        //måste ha för att ScriptableObject
        _generated = false;
    }

    public void ConstructBehaviourTree()
    {
        Debug.Log(_generated);

        if (!_generated)
        {
            IdleNode idleNode = ScriptableObject.CreateInstance<IdleNode>();
            idleNode.Construct(blackboard);

            RangeNode chaseRangeNode = ScriptableObject.CreateInstance<RangeNode>();
            chaseRangeNode.Construct(blackboard);
            chaseRangeNode.AddProperty(blackboard.nodeData.Get<float>("aggroRange"));
            ChaseNode chaseNode = ScriptableObject.CreateInstance<ChaseNode>();
            chaseNode.Construct(blackboard);

            RangeNode attackRangeNode = ScriptableObject.CreateInstance<RangeNode>();
            attackRangeNode.Construct(blackboard);
            attackRangeNode.AddProperty(blackboard.nodeData.Get<float>("attackRange"));
            DebugNode debugNode = ScriptableObject.CreateInstance<DebugNode>();
            debugNode.Construct(blackboard);


            Sequence chaseSequence = ScriptableObject.CreateInstance<Sequence>();
            chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });
            
            Sequence attackSequence = ScriptableObject.CreateInstance<Sequence>();
            attackSequence.Construct(new List<AbstractNode> { attackRangeNode, debugNode });


            Selector behaviorSelector = ScriptableObject.CreateInstance<Selector>();
            behaviorSelector.Construct(new List<AbstractNode> { attackSequence, chaseSequence });


            topNode = ScriptableObject.CreateInstance<Selector>();
            //topNode.Construct(new List<AbstractNode> { chaseSequence, idleNode });
            topNode.Construct(new List<AbstractNode> { behaviorSelector, idleNode });


            //FIXA!!!
            _generated = true;

            Debug.Log("BT generated");
        }
    }
}
