namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysSuccessNode : ActionNode
    {
        public override NodeStatus Tick()
        {
            return NodeStatus.SUCCESS;
        }
    }
}
