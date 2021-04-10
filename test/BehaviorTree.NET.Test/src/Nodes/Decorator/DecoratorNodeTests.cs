using BehaviorTree.NET.Nodes.Action.Test;
using Xunit;
using BehaviorTree.NET.Blackboard;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class ForwardToChildNode : DecoratorNode
    {
        public ForwardToChildNode(INode child)
            : base(null, new IBlackboardKey[0], child)
        {
        }

        public int Halts { get; private set; }

        public override NodeStatus Tick() => this.Child.Tick();

        public override void Halt()
        {
            this.Halts++;
            base.Halt();
        }
    }

    public class ForwardToChildNodeTests
    {
        [Theory]
        [InlineData(NodeStatus.SUCCESS, 1)]
        [InlineData(NodeStatus.FAILURE, 10)]
        [InlineData(NodeStatus.RUNNING, 100)]
        public void TicksChild(NodeStatus expectedStatus, int expectedTicks)
        {
            var child = new ReturnXNode(expectedStatus);
            var node = new ForwardToChildNode(child);

            Assert.Equal(0, child.Ticks);
            for (int i = 0; i < expectedTicks; i++)
            {
                var status = node.Tick();
            }

            Assert.Equal(expectedTicks, child.Ticks);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void HaltsChild(int expectedHalts)
        {
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new ForwardToChildNode(child);

            Assert.Equal(0, child.Halts);
            for (int i = 0; i < expectedHalts; i++)
            {
                node.Halt();
            }

            Assert.Equal(expectedHalts, node.Halts);
            Assert.Equal(expectedHalts, child.Halts);
        }
    }

    public class DecoratorNodeTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void HaltsChild(int expectedHalts)
        {
            // ForwardToChild simply calls the base class halt (DecoratorNode)
            // the default implementation should call the child's halt
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new ForwardToChildNode(child);

            Assert.Equal(0, child.Halts);
            for (int i = 0; i < expectedHalts; i++)
            {
                node.Halt();
            }

            Assert.Equal(expectedHalts, child.Halts);
        }
    }
}
