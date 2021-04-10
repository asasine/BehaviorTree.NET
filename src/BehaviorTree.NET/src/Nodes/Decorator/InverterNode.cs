using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Decorator
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(INode child)
            : this(null, child)
        {
        }

        public InverterNode(IBlackboard blackboard, INode child)
            : base(blackboard, new IBlackboardKey[0], child)
        {
        }

        public override NodeStatus Tick()
        {
            var status = this.Child.Tick();
            switch (status)
            {
                case NodeStatus.SUCCESS:
                    return NodeStatus.FAILURE;
                case NodeStatus.FAILURE:
                    return NodeStatus.SUCCESS;
                case NodeStatus.RUNNING:
                default:
                    return status;
            }
        }
    }
}
