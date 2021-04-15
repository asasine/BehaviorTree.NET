using BehaviorTree.NET.Variables;

namespace BehaviorTree.NET.Nodes.Decorator
{
    public class RepeatNode : DecoratorNode
    {
        private readonly IConstant<int> n;

        private int numSuccesses;

        public RepeatNode(INode child, IConstant<int> n)
            : base(child)
        {
            this.numSuccesses = 0;
            this.n = n;
        }

        public override NodeStatus Tick()
        {
            var status = this.Child.Tick();
            switch (status)
            {
                case NodeStatus.SUCCESS:
                    return HandleSuccess(this.n.GetValue());
                case NodeStatus.FAILURE:
                    this.Halt();
                    return status;
                case NodeStatus.RUNNING:
                default:
                    return status;
            }
        }

        private NodeStatus HandleSuccess(int n)
        {
            if (this.numSuccesses == n)
            {
                this.Halt();
                return NodeStatus.SUCCESS;
            }
            else
            {
                this.numSuccesses++;
                return NodeStatus.RUNNING;
            }
        }

        public override void Halt()
        {
            this.numSuccesses = 0;
            base.Halt();
        }
    }
}
