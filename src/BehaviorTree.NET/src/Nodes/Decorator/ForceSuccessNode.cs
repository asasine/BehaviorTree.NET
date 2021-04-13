namespace BehaviorTree.NET.Nodes.Decorator
{
    public class ForceSuccessNode : DecoratorNode
    {
        public ForceSuccessNode(INode child)
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
                    return NodeStatus.SUCCESS;
                case NodeStatus.RUNNING:
                default:
                    return status;
            }
        }
    }
}
