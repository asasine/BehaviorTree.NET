using System;

namespace BehaviorTree.NET.Blackboard
{
    public interface IBlackboardKey : IEquatable<IBlackboardKey>
    {
        string Key { get; }

        bool IEquatable<IBlackboardKey>.Equals(IBlackboardKey other)
        {
            if (other == null) return false;
            if (this == other) return true;
            if (this.Key == null && other.Key == null) return true;
            else if (this.Key == null) return false;
            else if (other.Key == null) return false;
            else return this.Key.Equals(other.Key);
        }
    }
}
