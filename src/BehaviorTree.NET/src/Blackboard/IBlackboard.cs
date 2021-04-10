namespace BehaviorTree.NET.Blackboard
{
    public interface IBlackboard
    {
        object this[string key] { set; }
        bool TryGetValue(string key, out object value);
    }
}
