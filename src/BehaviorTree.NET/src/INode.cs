namespace BehaviorTree.NET
{
    public interface INode
    {
        NodeStatus Tick();
        void Halt();
    }
}
