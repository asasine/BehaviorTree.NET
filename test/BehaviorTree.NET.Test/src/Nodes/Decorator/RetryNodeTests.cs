using BehaviorTree.NET.Exceptions;
using BehaviorTree.NET.Nodes.Action.Test;
using BehaviorTree.NET.Blackboard;
using Xunit;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class RetryNodeTests
    {
        [Fact]
        public void MissingPortThrows()
        {
            var blackboard = new Blackboard.Blackboard();
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RetryNode(blackboard, child);
            Assert.ThrowsAny<BlackboardEntryNotProvidedException>(() => node.Tick());
        }

        [Fact]
        public void ReturnsSuccessImmediately()
        {
            var blackboard = new Blackboard.Blackboard();
            blackboard[RetryNode.N] = 0;
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RetryNode(blackboard, child);
            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(1, child.Ticks);
        }

        [Fact]
        public void HaltsAfterSuccess()
        {
            var blackboard = new Blackboard.Blackboard();
            blackboard[RetryNode.N] = 0;
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RetryNode(blackboard, child);
            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(1, child.Halts);
        }

        [Fact]
        public void HaltsAfterFailure()
        {
            var blackboard = new Blackboard.Blackboard();
            blackboard[RetryNode.N] = 0;
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RetryNode(blackboard, child);
            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(1, child.Halts);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void DoesNotHaltAfterRunning(int n)
        {
            var blackboard = new Blackboard.Blackboard();
            blackboard[RetryNode.N] = 0;
            var child = new ReturnXNode(NodeStatus.RUNNING);
            var node = new RetryNode(blackboard, child);
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
        public void ReturnsFailureAfterNFailures(int n)
        {
            var blackboard = new Blackboard.Blackboard();
            blackboard[RetryNode.N] = n;

            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RetryNode(blackboard, child);
            for (int i = 0; i < n; i++)
            {
                Assert.Equal(NodeStatus.RUNNING, node.Tick());
            }

            Assert.Equal(NodeStatus.FAILURE, node.Tick());

            // ticked once at the beginning and once more for every tick that returned failure until n
            Assert.Equal(n + 1, child.Ticks);
        }
    }
}
