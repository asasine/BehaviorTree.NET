using BehaviorTree.NET.Variables;

namespace BehaviorTree.NET.Nodes.Decorator
{
    public class RetryNode : DecoratorNode
    {
        private readonly IConstant<int> n;
        private int numFailures;

        public RetryNode(INode child, IConstant<int> n)
            : base(child)
        {
            numFailures = 0;
            this.n = n;
        }

        public override NodeStatus Tick()
        {
            var status = this.Child.Tick();
            switch (status)
            {
                case NodeStatus.SUCCESS:
                    this.Halt();
                    return status;
                case NodeStatus.FAILURE:
                    return HandleFailure(this.n.GetValue());
                case NodeStatus.RUNNING:
                default:
                    return status;
            }
        }

        private NodeStatus HandleFailure(int n)
        {
            if (this.numFailures == n)
            {
                this.Halt();
                return NodeStatus.FAILURE;
            }
            else
            {
                this.numFailures++;
                return NodeStatus.RUNNING;
            }
        }

        public override void Halt()
        {
            numFailures = 0;
            base.Halt();
        }
    }
}
