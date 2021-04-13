namespace BehaviorTree.NET.Nodes.Decorator
{
    public class RepeatUntilFailureNode : DecoratorNode
    {
        public RepeatUntilFailureNode(INode child)
            : base(child)
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
