using System.Linq;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Exceptions
{
    [System.Serializable]
    public class BlackboardEntryNotProvidedException : System.Exception
    {
        public BlackboardEntryNotProvidedException(params IBlackboardKey[] keys)
            : base($"Blackboard entries missing entries: [{string.Join<IBlackboardKey>(", ", keys)}]")
        { }

        protected BlackboardEntryNotProvidedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
