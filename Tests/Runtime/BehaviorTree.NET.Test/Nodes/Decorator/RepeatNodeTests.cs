using BehaviorTree.NET.Nodes.Action.Test;
using BehaviorTree.NET.Variables;
using Xunit;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class RepeatNodeTests
    {
        [Fact]
        public void ReturnsFailureImmediately()
        {
            var n = new InMemoryVariable<int>(0);
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RepeatNode(child, n);
            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(1, child.Ticks);
        }

        [Fact]
        public void HaltsAfterFailure()
        {
            var n = new InMemoryVariable<int>(0);
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RepeatNode(child, n);
            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(1, child.Halts);
        }

        [Fact]
        public void HaltsAfterSuccess()
        {
            var n = new InMemoryVariable<int>(0);
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RepeatNode(child, n);
            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(1, child.Halts);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void DoesNotHaltAfterRunning(int n)
        {
            var child = new ReturnXNode(NodeStatus.RUNNING);
            var node = new RepeatNode(child, new InMemoryVariable<int>(n));
            for (int i = 0; i < n; i++)
            {
                var status = node.Tick();
                Assert.Equal(NodeStatus.RUNNING, status);
            }

            Assert.Equal(n, child.Ticks);
            Assert.Equal(0, child.Halts);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void ReturnsSuccessAfterNSuccesses(int n)
        {
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RepeatNode(child, new InMemoryVariable<int>(n));
            for (int i = 0; i < n; i++)
            {
                Assert.Equal(NodeStatus.RUNNING, node.Tick());
            }

            Assert.Equal(NodeStatus.SUCCESS, node.Tick());

            // ticked once at the beginning and once more for every tick that returned success until n
            Assert.Equal(n + 1, child.Ticks);
        }
    }
}
