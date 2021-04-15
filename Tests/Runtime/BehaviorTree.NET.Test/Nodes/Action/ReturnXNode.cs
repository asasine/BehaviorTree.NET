using Xunit;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    public class ReturnXNode : INode
    {
        private readonly NodeStatus x;

        public ReturnXNode(NodeStatus x)
        {
            this.x = x;
            this.Ticks = 0;
            this.Halts = 0;
        }

        public int Ticks { get; private set; }
        public int Halts { get; private set; }

        public NodeStatus Tick()
        {
            this.Ticks++;
            return this.x;
        }

        public void Halt()
        {
            this.Halts++;
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
            Assert.Equal(0, node.Halts);
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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void HaltCounter(int expectedHalts)
        {
            var node = new ReturnXNode(NodeStatus.SUCCESS);
            for (int i = 0; i < expectedHalts; i++)
            {
                node.Halt();
            }

            Assert.Equal(expectedHalts, node.Halts);
        }
    }
}
