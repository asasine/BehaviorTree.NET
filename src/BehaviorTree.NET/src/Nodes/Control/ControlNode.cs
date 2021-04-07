using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree.NET.Nodes.Control
{
    public abstract class ControlNode : INode
    {
        public ControlNode(IEnumerable<INode> children)
        {
            this.Children = children.ToList();
        }

        public IReadOnlyList<INode> Children { get; }

        public abstract NodeStatus Tick();

        public virtual void Halt()
        {
            for (int i = 0; i < this.Children.Count; i++)
            {
                HaltChild(i);
            }
        }

        public void HaltChild(int index)
        {
            this.Children[index].Halt();
        }
    }
}
