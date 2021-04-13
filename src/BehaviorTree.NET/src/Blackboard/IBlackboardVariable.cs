namespace BehaviorTree.NET.Blackboard
{
    public interface IInputVariable<T>
    {
        T GetValue();
    }

    public interface IOutputVariable<T>
    {
        void SetValue(T value);
    }

    internal interface IVariable<T> : IInputVariable<T>, IOutputVariable<T>
    {
    }

    public class MutableVariable<T> : IVariable<T>
    {
        private T value;

        public MutableVariable(T @default = default)
        {
            value = @default;
        }

        public T GetValue() => this.value;

        public void SetValue(T value) => this.value = value;
    }
}
