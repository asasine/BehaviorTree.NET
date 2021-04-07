using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree.NET.Nodes.Control
{
    public abstract class ControlNode : Node
    {
        public ControlNode(IReadOnlyCollection<Node> children)
        {
            this.Children = children.ToList();
        }

        public IReadOnlyList<Node> Children { get; }
    }
}
