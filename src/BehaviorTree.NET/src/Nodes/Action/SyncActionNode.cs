namespace BehaviorTree.NET.Nodes.Action
{
    public abstract class SyncActionNode : INode
    {
        public abstract NodeStatus Tick();

        public virtual void Halt()
        {
        }
    }
}
