namespace BehaviorTree.NET.Variables
{
    public class InMemoryVariable<T> : IVariable<T>
    {
        private T value;

        public InMemoryVariable(T @default = default)
        {
            value = @default;
        }

        public T GetValue() => this.value;

        public void SetValue(T value) => this.value = value;
    }
}
