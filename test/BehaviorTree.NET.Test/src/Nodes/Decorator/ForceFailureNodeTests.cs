using BehaviorTree.NET.Nodes.Action;
using Xunit;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class ForceFailureNodeTests
    {
        [Fact]
        public void SuccessChildReturnsFailure()
        {
            var child = new AlwaysSuccessNode();
            var node = new ForceFailureNode(child);

            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
        }

        [Fact]
        public void FailureChildReturnsFailure()
        {
            var child = new AlwaysFailureNode();
            var node = new ForceFailureNode(child);

            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
        }
    }
}
