using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree.NET.Nodes.Control
{
    public abstract class ControlNode : Node
    {
        public ControlNode(IEnumerable<Node> children)
        {
            this.Children = children.ToList();
        }

        public IReadOnlyList<Node> Children { get; }

        public override void Halt()
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
