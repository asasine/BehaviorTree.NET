namespace BehaviorTree.NET.Variables
{
    public interface IVariable<T> : IConstant<T>
    {
        void SetValue(T value);
    }
}
