using System.Collections.Generic;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes
{
    public interface INode
    {
        NodeStatus Tick();
        void Halt();

        IBlackboard Blackboard { get; }
        IReadOnlyCollection<IBlackboardKey> BlackboardEntries { get; }
    }
}
