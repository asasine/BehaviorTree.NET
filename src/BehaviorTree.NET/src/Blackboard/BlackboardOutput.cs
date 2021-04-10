namespace BehaviorTree.NET.Blackboard
{
    public class BlackboardOutput : IBlackboardKey
    {
        public BlackboardOutput(string key)
        {
            this.Key = key;
        }

        public string Key { get; }

        public bool Equals(IBlackboardKey other)
        {
            if (other == null) return false;
            if (this == other) return true;
            if (other is BlackboardOutput that)
            {
                return this.Key.Equals(that.Key);
            }
            else
            {
                return false;
            }
        }
    }
}
