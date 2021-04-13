namespace BehaviorTree.NET.Nodes
{
    public interface INode
    {
        NodeStatus Tick();
        void Halt();
    }
}
