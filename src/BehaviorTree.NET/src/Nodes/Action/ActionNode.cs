using System.Collections.Generic;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Action
{
    public abstract class ActionNode : Node
    {
        public ActionNode(IBlackboard blackboard, IEnumerable<BlackboardKey> blackboardEntries)
            : base(blackboard, blackboardEntries)
        {
        }
    }
}
