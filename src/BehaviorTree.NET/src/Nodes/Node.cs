using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Exceptions;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes
{
    public abstract class Node : INode, IBlackboard
    {
        private readonly Lazy<IReadOnlyCollection<BlackboardKey>> inputEntries;
        private readonly Lazy<IReadOnlyCollection<BlackboardKey>> outputEntries;

        public Node(IBlackboard blackboard, IEnumerable<BlackboardKey> blackboardEntries)
        {
            this.Blackboard = blackboard;
            this.BlackboardEntries = blackboardEntries.ToArray();

            this.inputEntries = new Lazy<IReadOnlyCollection<BlackboardKey>>(() => this.BlackboardEntries.Where(key => key.IsInput).ToArray());
            this.outputEntries = new Lazy<IReadOnlyCollection<BlackboardKey>>(() => this.BlackboardEntries.Where(key => key.IsOutput).ToArray());
        }

        public IBlackboard Blackboard { get; }

        public IReadOnlyCollection<BlackboardKey> BlackboardEntries { get; }
        private IReadOnlyCollection<BlackboardKey> InputEntries => inputEntries.Value;
        private IReadOnlyCollection<BlackboardKey> OutputEntries => outputEntries.Value;

        public abstract void Halt();
        public abstract NodeStatus Tick();

        public IBlackboardReader<T> CreateInputEntry<T>(in BlackboardKey key)
        {
            var isInputPort = this.InputEntries.Contains(key);
            if (!isInputPort)
            {
                throw new BlackboardEntryNotDeclaredException(key);
            }

            return Blackboard.CreateInputEntry<T>(key);
        }

        public IBlackboardWriter<T> CreateOutputEntry<T>(in BlackboardKey key)
        {
            var isOutputPort = this.OutputEntries.Contains(key);
            if (!isOutputPort)
            {
                throw new BlackboardEntryNotDeclaredException(key);
            }

            return Blackboard.CreateOutputEntry<T>(key);
        }
    }
}