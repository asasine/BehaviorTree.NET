using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysFailureNode : SyncActionNode
    {
        public AlwaysFailureNode()
            : this(null)
        {
        }

        public AlwaysFailureNode(IBlackboard blackboard)
            : base(blackboard, new IBlackboardKey[0])
        {
        }

        public override NodeStatus Tick()
        {
            return NodeStatus.FAILURE;
        }
    }
}
