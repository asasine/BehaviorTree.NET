using System.Collections.Generic;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Control
{
    public class FallbackNode : ControlNode
    {
        public FallbackNode(IEnumerable<INode> children)
            : this(null, children)
        {
        }

        public FallbackNode(IBlackboard blackboard, IEnumerable<INode> children)
            : base(blackboard, new BlackboardKey[0], children)
        {
        }

        public override NodeStatus Tick()
        {
            foreach (var child in this.Children)
            {
                var status = child.Tick();
                switch (status)
                {
                    case NodeStatus.SUCCESS:
                        this.Halt();
                        return status;
                    case NodeStatus.RUNNING:
                        return status;
                    case NodeStatus.FAILURE:
                        continue;
                }
            }

            this.Halt();
            return NodeStatus.FAILURE;
        }
    }
}
