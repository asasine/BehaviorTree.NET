namespace BehaviorTree.NET
{
    public abstract class Node
    {
        public abstract NodeStatus Tick();
        public abstract void Halt();
    }
}
