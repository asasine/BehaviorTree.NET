using BehaviorTree.NET.Nodes.Action;
using BehaviorTree.NET.Nodes.Action.Test;
using Xunit;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class InverterNodeTests
    {
        [Fact]
        public void SuccessChildReturnsFailure()
        {
            var child = new AlwaysSuccessNode();
            var inverterNode = new InverterNode(child);

            var status = inverterNode.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
        }

        [Fact]
        public void FailureChildReturnsSuccess()
        {
            var child = new AlwaysFailureNode();
            var inverterNode = new InverterNode(child);

            var status = inverterNode.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
        }

        [Fact]
        public void RunningChildReturnsRunning()
        {
            var child = new ReturnXNode(NodeStatus.RUNNING);
            var node = new InverterNode(child);
            var status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
        }
    }
}
