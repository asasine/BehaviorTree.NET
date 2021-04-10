using System.Collections.Generic;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Decorator
{
    public abstract class DecoratorNode : Node
    {
        public DecoratorNode(IBlackboard blackboard, IEnumerable<BlackboardKey> blackboardEntries, INode child)
            : base(blackboard, blackboardEntries)
        {
            this.Child = child;
        }

        public INode Child { get; }

        public override void Halt() => this.Child.Halt();
    }
}
