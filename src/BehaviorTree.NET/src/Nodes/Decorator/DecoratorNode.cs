namespace BehaviorTree.NET.Nodes.Decorator
{
    public abstract class DecoratorNode : Node
    {
        public DecoratorNode(Node child)
        {
            this.Child = child;
        }

        public Node Child { get; }
    }
}
