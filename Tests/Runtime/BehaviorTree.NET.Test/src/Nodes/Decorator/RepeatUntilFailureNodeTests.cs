using BehaviorTree.NET.Nodes.Action.Test;
using Xunit;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class RepeatUntilFailureNodeTests
    {
        [Fact]
        public void HaltCallsChildHalt()
        {
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RepeatUntilFailureNode(child);
            node.Halt();
            Assert.Equal(1, child.Halts);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void TicksChildOncePerTick(int n)
        {
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RepeatUntilFailureNode(child);
            for (int i = 0; i < n; i++)
            {
                node.Tick();
            }

            Assert.Equal(n, child.Ticks);
        }

        [Theory]
        [InlineData(NodeStatus.SUCCESS, NodeStatus.RUNNING)]
        [InlineData(NodeStatus.RUNNING, NodeStatus.RUNNING)]
        [InlineData(NodeStatus.FAILURE, NodeStatus.SUCCESS)]
        public void ReturnsAppropriateStatus(NodeStatus expectedChildStatus, NodeStatus expectedParentStatus)
        {
            var child = new ReturnXNode(expectedChildStatus);
            var node = new RepeatUntilFailureNode(child);
            var status = node.Tick();
            Assert.Equal(expectedParentStatus, status);
        }

        [Fact]
        public void HaltsOnChildFailure()
        {
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RepeatUntilFailureNode(child);
            var status = node.Tick();
            Assert.Equal(1, child.Halts);
        }
    }
}
