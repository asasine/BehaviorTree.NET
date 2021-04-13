namespace BehaviorTree.NET.Nodes.Action
{
    public abstract class SyncActionNode : IActionNode
    {
        public abstract NodeStatus Tick();

        public virtual void Halt()
        {
        }
    }
}
