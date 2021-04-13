namespace BehaviorTree.NET.Nodes.Decorator
{

    public abstract class DecoratorNode : IDecoratorNode
    {
        public DecoratorNode(INode child)
        {
            this.Child = child;
        }

        public INode Child { get; }

        public abstract NodeStatus Tick();
        public virtual void Halt() => this.Child.Halt();
    }
}
