namespace BehaviorTree.NET.Nodes.Action
{
    public class AlwaysSucceedNode : Node
    {
        public override NodeStatus Tick()
        {
            return NodeStatus.SUCCESS;
        }
    }
}
