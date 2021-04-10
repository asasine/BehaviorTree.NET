using BehaviorTree.NET.Exceptions;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Decorator
{
    public class RetryNode : DecoratorNode
    {
        public static readonly BlackboardKey KEY_N = new BlackboardKey("n", BlackboardEntryType.Input);

        private readonly IBlackboardReader<int> inputN;

        private int numFailures;

        public RetryNode(IBlackboard blackboard, INode child)
            : base(blackboard, new BlackboardKey[] { KEY_N }, child)
        {
            numFailures = 0;
            inputN = this.CreateInputEntry<int>(KEY_N);
        }

        public override NodeStatus Tick()
        {
            if (!this.inputN.TryGetValue(out int n))
            {
                throw new BlackboardEntryNotProvidedException(inputN.Key);
            }

            var status = this.Child.Tick();
            switch (status)
            {
                case NodeStatus.SUCCESS:
                    this.Halt();
                    return status;
                case NodeStatus.FAILURE:
                    return HandleFailure(n);
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
