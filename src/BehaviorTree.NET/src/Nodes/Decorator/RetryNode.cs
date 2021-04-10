using System.Collections.Generic;
using BehaviorTree.NET.Exceptions;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Decorator
{
    public class RetryNode : DecoratorNode
    {
        public const string N = "n";

        private static BlackboardInput INPUT_N = new BlackboardInput(N);

        private static IEnumerable<IBlackboardKey> BLACKBOARD_ENTRIES = new IBlackboardKey[]
        {
            INPUT_N,
        };

        private int numFailures;

        public RetryNode(IBlackboard blackboard, INode child)
            : base(blackboard, BLACKBOARD_ENTRIES, child)
        {
            numFailures = 0;
        }

        public override NodeStatus Tick()
        {
            if (!this.TryGetInputBlackboardEntry(INPUT_N, out int n))
            {
                throw new BlackboardEntryNotProvidedException(INPUT_N);
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
