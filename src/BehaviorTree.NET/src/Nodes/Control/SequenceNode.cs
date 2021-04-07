using System.Collections.Generic;

namespace BehaviorTree.NET.Nodes.Control
{
    public class SequenceNode : ControlNode
    {
        private int index;

        public SequenceNode(IReadOnlyCollection<Node> children)
            : base(children)
        {
            this.index = 0;
        }

        public override NodeStatus Tick()
        {
            for (; this.index < this.Children.Count; this.index++)
            {
                var child = this.Children[index];
                var status = child.Tick();
                switch (status)
                {
                    case NodeStatus.SUCCESS:
                        continue;
                    case NodeStatus.FAILURE:
                        this.index = 0;
                        return status;
                    case NodeStatus.RUNNING:
                        return status;
                }
            }

            return NodeStatus.SUCCESS;
        }
    }
}
