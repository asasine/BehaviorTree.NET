namespace BehaviorTree.NET.Nodes.Decorator
{
    public class ForceFailureNode : DecoratorNode
    {
        public ForceFailureNode(Node child)
            : base(child)
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
