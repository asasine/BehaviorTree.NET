using System;

namespace BehaviorTree.NET.Blackboard
{
    public readonly struct BlackboardKey
    {
        public readonly string key;
        public readonly BlackboardEntryType type;

        public BlackboardKey(in string key, in BlackboardEntryType type)
        {
            this.key = key;
            this.type = type;
        }

        public bool IsInput => this.type.HasFlag(BlackboardEntryType.Input);
        public bool IsOutput => this.type.HasFlag(BlackboardEntryType.Output);

        public static implicit operator string(BlackboardKey key) => key.key;
        public static implicit operator BlackboardEntryType(BlackboardKey key) => key.type;

        public override readonly string ToString() => $"{this.key} ({this.type})";
    }

    [Flags]
    public enum BlackboardEntryType
    {
        Input = 1 << 0,
        Output = 1 << 1,
    }
}
