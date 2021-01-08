using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBlackboard", menuName = "ScriptableObjects/Blackboard", order = 1)]
public class BlackboardScript : ScriptableObject
{
    public BlackboardNodeData nodeData;
    public BlackboardLocalData localData;
    // public BlackboardGroupData groupData;
    public BlackboardGlobalData globalData;
}
