namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysSuccessNode : Node
    {
        public override NodeStatus Tick()
        {
            return NodeStatus.SUCCESS;
        }
    }
}
