using System.Linq;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Exceptions
{
    [System.Serializable]
    public class BlackboardEntryNotDeclaredException : System.Exception
    {
        public BlackboardEntryNotDeclaredException(params BlackboardKey[] keys)
            : base($"Blackboard entries not declared: [{string.Join<BlackboardKey>(", ", keys)}]")
        { }

        protected BlackboardEntryNotDeclaredException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
