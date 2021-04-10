namespace BehaviorTree.NET.Blackboard
{
    public interface IBlackboard
    {
        IBlackboardReader<T> CreateInputEntry<T>(in BlackboardKey key);
        IBlackboardWriter<T> CreateOutputEntry<T>(in BlackboardKey key);
    }
}
