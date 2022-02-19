using System.Collections.Generic;

namespace BehaviorTree.NET.Nodes.Control
{
    public class SequenceWithMemoryNode : ControlNode
    {
        private int index;

        public SequenceWithMemoryNode(IEnumerable<INode> children)
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
                        this.Halt();
                        return status;
                    case NodeStatus.RUNNING:
                        return status;
                }
            }

            this.Halt();
            return NodeStatus.SUCCESS;
        }

        public override void Halt()
        {
            this.index = 0;
            base.Halt();
        }
    }
}
