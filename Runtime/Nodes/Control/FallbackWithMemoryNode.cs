using System.Collections.Generic;

namespace BehaviorTree.NET.Nodes.Control
{
    public class FallbackWithMemoryNode : ControlNode
    {
        private int index;

        public FallbackWithMemoryNode(IEnumerable<INode> children)
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
                        this.Halt();
                        return status;
                    case NodeStatus.FAILURE:
                        continue;
                    case NodeStatus.RUNNING:
                        return status;
                }
            }

            this.Halt();
            return NodeStatus.FAILURE;
        }

        public override void Halt()
        {
            this.index = 0;
            base.Halt();
        }
    }
}
