using System.Collections.Generic;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Action
{
    public abstract class SyncActionNode : ActionNode
    {
        protected SyncActionNode(IBlackboard blackboard, IEnumerable<IBlackboardKey> blackboardEntries)
            : base(blackboard, blackboardEntries)
        {
        }

        public override void Halt()
        {
        }
    }
}
