namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysFailureNode : SyncActionNode
    {
        public override NodeStatus Tick()
        {
            return NodeStatus.FAILURE;
        }
    }
}
