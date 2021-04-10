using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Decorator
{
    public class ForceFailureNode : DecoratorNode
    {
        public ForceFailureNode(INode child)
            : this(null, child)
        {
        }

        public ForceFailureNode(IBlackboard blackboard, INode child)
            : base(blackboard, new BlackboardKey[0], child)
        {
        }

        public override NodeStatus Tick()
        {
            var status = this.Child.Tick();
            switch (status)
            {
                case NodeStatus.SUCCESS:
                case NodeStatus.FAILURE:
                    return NodeStatus.FAILURE;
                case NodeStatus.RUNNING:
                default:
                    return status;
            }
        }
    }
}
