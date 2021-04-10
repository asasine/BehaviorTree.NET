namespace BehaviorTree.NET.Blackboard
{
    public static class BlackboardExtensions
    {
        public static bool TryGetValue<T>(this IBlackboard blackboard, string id, out T value)
        {
            if (blackboard.TryGetValue(id, out object currentValue))
            {
                value = (T)currentValue;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
    }
}
