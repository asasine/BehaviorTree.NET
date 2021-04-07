namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysSuccessNode : SyncActionNode
    {
        public override NodeStatus Tick()
        {
            return NodeStatus.SUCCESS;
        }
    }
}
