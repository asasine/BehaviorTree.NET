using System.Collections.Generic;

namespace BehaviorTree.NET.Nodes.Control
{
    public interface IControlNode : INode
    {
        IReadOnlyList<INode> Children { get; }
    }
}
