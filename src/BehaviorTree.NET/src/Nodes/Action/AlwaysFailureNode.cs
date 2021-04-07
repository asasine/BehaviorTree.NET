namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysFailureNode : Node
    {
        public override NodeStatus Tick()
        {
            return NodeStatus.FAILURE;
        }
    }
}
