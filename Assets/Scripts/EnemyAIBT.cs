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

        // Debug.Log(topNode);
    }

    public void SetTopNode(Selector topNode)
    {
        this.topNode = topNode;
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
            chaseRangeNode.AddProperties(new string[] { "aggroRange" });
            ChaseNode chaseNode = ScriptableObject.CreateInstance<ChaseNode>();
            chaseNode.Construct(blackboard);

            RangeNode attackRangeNode = ScriptableObject.CreateInstance<RangeNode>();
            attackRangeNode.Construct(blackboard);
            attackRangeNode.AddProperties(new string[] { "attackRange" });
            //DebugNode debugNode = ScriptableObject.CreateInstance<DebugNode>();
            //debugNode.Construct(blackboard);

            AttackNode attackNode = CreateInstance<AttackNode>();
            attackNode.Construct(blackboard);

            Sequence chaseSequence = ScriptableObject.CreateInstance<Sequence>();
            chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });
            
            Sequence attackSequence = ScriptableObject.CreateInstance<Sequence>();
            //attackSequence.Construct(new List<AbstractNode> { attackRangeNode, debugNode });
            attackSequence.Construct(new List<AbstractNode> { attackRangeNode, attackNode });


            //Selector behaviorSelector = ScriptableObject.CreateInstance<Selector>();
            //behaviorSelector.Construct(new List<AbstractNode> { attackSequence, chaseSequence });


            topNode = ScriptableObject.CreateInstance<Selector>();
            //topNode.Construct(new List<AbstractNode> { behaviorSelector, idleNode });
            topNode.Construct(new List<AbstractNode> { attackSequence, chaseSequence, idleNode });

            _generated = true;

            Debug.Log("BT generated");
        }
    }

    public void TestConstruct()
    {
        IdleNode idleNode = ScriptableObject.CreateInstance<IdleNode>();
        idleNode.Construct(blackboard);

        DebugNode debugNode = ScriptableObject.CreateInstance<DebugNode>();
        debugNode.Construct(blackboard);

        Sequence sequence = ScriptableObject.CreateInstance<Sequence>();
        sequence.Construct(new List<AbstractNode> { idleNode, debugNode });

        topNode = ScriptableObject.CreateInstance<Selector>();
        topNode.Construct(new List<AbstractNode> { sequence });
    }
}
