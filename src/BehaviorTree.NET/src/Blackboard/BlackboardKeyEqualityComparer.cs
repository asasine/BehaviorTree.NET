using System.Collections.Generic;

namespace BehaviorTree.NET.Blackboard
{
    public class BlackboardKeyEqualityComparer : IEqualityComparer<IBlackboardKey>
    {
        public bool Equals(IBlackboardKey x, IBlackboardKey y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            if (x is BlackboardInput xInput && y is BlackboardInput yInput)
            {
                return xInput.Equals(yInput);
            }
            else if (x is BlackboardOutput xOuput && y is BlackboardOutput yOutput)
            {
                return xOuput.Equals(yOutput);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(IBlackboardKey obj) => obj.GetHashCode();
    }
}
