using System.Collections.Generic;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Control
{
    public class SequenceNode : ControlNode
    {
        private int index;

        public SequenceNode(IEnumerable<INode> children)
            : this(null, children)
        {
        }

        public SequenceNode(IBlackboard blackboard, IEnumerable<INode> children)
            : base(blackboard, new IBlackboardKey[0], children)
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
