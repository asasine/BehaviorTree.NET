namespace BehaviorTree.NET.Blackboard
{
    public interface IBlackboardWriter<T>
    {
        ref readonly BlackboardKey Key { get; }
        void SetValue(in T value);
    }
}
