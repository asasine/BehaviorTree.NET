using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Control
{
    public abstract class ControlNode : Node
    {
        public ControlNode(IBlackboard blackboard, IEnumerable<BlackboardKey> blackboardEntries, IEnumerable<INode> children)
            : base(blackboard, blackboardEntries)
        {
            this.Children = children.ToList();
        }

        public IReadOnlyList<INode> Children { get; }

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
