using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes
{
    public abstract class Node : INode
    {
        public Node(IBlackboard blackboard, IEnumerable<IBlackboardKey> blackboardEntries)
        {
            this.Blackboard = blackboard;
            this.BlackboardEntries = blackboardEntries.ToArray();
            this.InputEntries = this.BlackboardEntries.SelectWhereIs<IBlackboardKey, BlackboardInput>().ToArray();
            this.OutputEntries = this.BlackboardEntries.SelectWhereIs<IBlackboardKey, BlackboardOutput>().ToArray();
        }

        public IBlackboard Blackboard { get; }

        public IReadOnlyCollection<IBlackboardKey> BlackboardEntries { get; }

        private IReadOnlyCollection<BlackboardInput> InputEntries { get; }
        private IReadOnlyCollection<BlackboardOutput> OutputEntries { get; }

        public abstract void Halt();
        public abstract NodeStatus Tick();
    }
}
