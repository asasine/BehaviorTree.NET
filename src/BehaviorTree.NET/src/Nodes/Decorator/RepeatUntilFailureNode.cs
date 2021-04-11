using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Decorator
{
    public class RepeatUntilFailureNode : DecoratorNode
    {
        public RepeatUntilFailureNode(INode child)
            : this(null, child)
        {
        }

        public RepeatUntilFailureNode(IBlackboard blackboard, INode child)
            : base(blackboard, new BlackboardKey[0], child)
        {
        }

        public override NodeStatus Tick()
        {
            var status = this.Child.Tick();
            switch (status)
            {
                case NodeStatus.SUCCESS:
                    return NodeStatus.RUNNING;
                case NodeStatus.FAILURE:
                    this.Halt();
                    return NodeStatus.SUCCESS;
                case NodeStatus.RUNNING:
                default:
                    return status;
            }
        }
    }
}
