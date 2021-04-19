namespace BehaviorTree.NET.Nodes.Decorator
{
    public interface IDecoratorNode : INode
    {
        INode Child { get; }
    }
}
