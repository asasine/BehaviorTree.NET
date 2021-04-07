namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysFailureNode : ActionNode
    {
        public override NodeStatus Tick()
        {
            return NodeStatus.FAILURE;
        }
    }
}
