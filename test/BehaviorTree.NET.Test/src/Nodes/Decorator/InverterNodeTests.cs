using BehaviorTree.NET.Nodes.Action;
using Xunit;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class InverterNodeTests
    {
        [Fact]
        public void TickReturnsFailure()
        {
            var child = new AlwaysSuccessNode();
            var inverterNode = new InverterNode(child);

            var status = inverterNode.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
        }

        [Fact]
        public void TickReturnsSuccess()
        {
            var child = new AlwaysFailureNode();
            var inverterNode = new InverterNode(child);

            var status = inverterNode.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
        }
    }
}
