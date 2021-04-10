using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysSuccessNode : SyncActionNode
    {
        public AlwaysSuccessNode()
            : this(null)
        {
        }

        public AlwaysSuccessNode(IBlackboard blackboard)
            : base(blackboard, new IBlackboardKey[0])
        {
        }

        public override NodeStatus Tick()
        {
            return NodeStatus.SUCCESS;
        }
    }
}
