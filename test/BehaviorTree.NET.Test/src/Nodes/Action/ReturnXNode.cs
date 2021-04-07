using Xunit;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    public class ReturnXNode : ActionNode
    {
        private readonly NodeStatus x;

        public ReturnXNode(NodeStatus x)
        {
            this.x = x;
            this.Ticks = 0;
        }

        public int Ticks { get; private set; }

        public override NodeStatus Tick()
        {
            this.Ticks++;
            return this.x;
        }
    }

    public class ReturnXNodeTests
    {
        [Theory]
        [InlineData(NodeStatus.SUCCESS)]
        [InlineData(NodeStatus.FAILURE)]
        [InlineData(NodeStatus.RUNNING)]
        public void NoTicks(NodeStatus expectedStatus)
        {
            var node = new ReturnXNode(expectedStatus);
            Assert.Equal(0, node.Ticks);
        }

        [Theory]
        [InlineData(NodeStatus.SUCCESS)]
        [InlineData(NodeStatus.FAILURE)]
        [InlineData(NodeStatus.RUNNING)]
        public void ReturnsExpectedOnce(NodeStatus expectedStatus)
        {
            var node = new ReturnXNode(expectedStatus);
            var actualStatus = node.Tick();
            Assert.Equal(expectedStatus, actualStatus);
            Assert.Equal(1, node.Ticks);
        }

        [Theory]
        [InlineData(NodeStatus.SUCCESS, 10)]
        [InlineData(NodeStatus.FAILURE, 100)]
        [InlineData(NodeStatus.RUNNING, 42)]
        public void ReturnsExpectedNTimes(NodeStatus expectedStatus, int expectedTicks)
        {
            var node = new ReturnXNode(expectedStatus);
            for (int i = 0; i < expectedTicks; i++)
            {
                var actualStatus = node.Tick();
                Assert.Equal(expectedStatus, actualStatus);
            }

            Assert.Equal(expectedTicks, node.Ticks);
        }
    }
}
