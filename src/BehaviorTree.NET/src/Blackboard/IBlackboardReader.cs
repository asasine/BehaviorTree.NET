namespace BehaviorTree.NET.Blackboard
{
    public interface IBlackboardReader<T>
    {
        ref readonly BlackboardKey Key { get; }
        bool TryGetValue(out T value);
    }
}
