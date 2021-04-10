namespace BehaviorTree.NET.Blackboard
{
    public class BlackboardInput : IBlackboardKey
    {
        public BlackboardInput(string key)
        {
            this.Key = key;
        }

        public string Key { get; }

        public bool Equals(IBlackboardKey other)
        {
            if (other == null) return false;
            if (this == other) return true;
            if (other is BlackboardInput that)
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
