namespace BehaviorTree.NET.Blackboard
{
    public interface IVariable<T> : IConstant<T>
    {
        void SetValue(T value);
    }
}
