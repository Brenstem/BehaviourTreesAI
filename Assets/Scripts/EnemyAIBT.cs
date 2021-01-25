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
            RangeNode chaseRangeNode = ScriptableObject.CreateInstance<RangeNode>();
            chaseRangeNode.Construct(blackboard);
            chaseRangeNode.AddProperties(new string[] { "aggroRange" });

            ChaseNode chaseNode = ScriptableObject.CreateInstance<ChaseNode>();
            chaseNode.Construct(blackboard);

            RangeNode attackRangeNode = ScriptableObject.CreateInstance<RangeNode>();
            attackRangeNode.Construct(blackboard);
            attackRangeNode.AddProperties(new string[] { "attackRange" });

            WaitNode attackStartupNode = CreateInstance<WaitNode>();
            attackStartupNode.Construct(blackboard);
            attackStartupNode.AddProperties(new string[] { "attackStartupTime" });

            AttackNode attackNode = CreateInstance<AttackNode>();
            attackNode.Construct(blackboard);

            WaitNode attackEndLagNode = CreateInstance<WaitNode>();
            attackEndLagNode.Construct(blackboard);
            attackEndLagNode.AddProperties(new string[] { "attackEndLagTime" });


            ConcurrentNode attackSequence = ScriptableObject.CreateInstance<ConcurrentNode>();
            attackSequence.Construct(new List<AbstractNode> { attackRangeNode, attackStartupNode, attackNode, attackEndLagNode });

            Sequence chaseSequence = ScriptableObject.CreateInstance<Sequence>();
            chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });

            IdleNode idleNode = ScriptableObject.CreateInstance<IdleNode>();
            idleNode.Construct(blackboard);

            topNode = ScriptableObject.CreateInstance<Selector>();
            topNode.Construct(new List<AbstractNode> { attackSequence, chaseSequence, idleNode });

            _generated = true;

            Debug.Log("BT generated");
        }
    }

    public void TestConstruct()
    {
        if (!_generated)
        {
            RangeNode attackRangeNode = ScriptableObject.CreateInstance<RangeNode>();
            attackRangeNode.Construct(blackboard);
            attackRangeNode.AddProperties(new string[] { "attackRange" });

            AttackNode attackNode = CreateInstance<AttackNode>();
            attackNode.Construct(blackboard);

            WaitNode waitNode = CreateInstance<WaitNode>();
            waitNode.Construct(blackboard);
            waitNode.AddProperties(new string[] { "attackStartupTime" });


            ConcurrentNode attackSequence = ScriptableObject.CreateInstance<ConcurrentNode>();
            attackSequence.Construct(new List<AbstractNode> { attackRangeNode, attackNode, waitNode });

            IdleNode idleNode = ScriptableObject.CreateInstance<IdleNode>();
            idleNode.Construct(blackboard);


            topNode = ScriptableObject.CreateInstance<Selector>();
            topNode.Construct(new List<AbstractNode> { attackSequence, idleNode });

            _generated = true;

            Debug.Log("BT generated");
        }
    }
}
