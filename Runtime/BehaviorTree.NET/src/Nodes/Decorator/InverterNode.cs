namespace BehaviorTree.NET.Nodes.Decorator
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(INode child)
            : base(child)
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
