using System.Collections.Generic;

namespace BehaviorTree.NET.Nodes.Control
{
    public class FallbackNode : ControlNode
    {
        public FallbackNode(IEnumerable<Node> children)
            : base(children)
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
