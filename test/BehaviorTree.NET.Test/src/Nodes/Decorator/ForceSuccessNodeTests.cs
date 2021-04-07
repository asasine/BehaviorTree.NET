using BehaviorTree.NET.Nodes.Action;
using Xunit;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class ForceSuccessNodeTests
    {
        [Fact]
        public void SuccessChildReturnsSuccess()
        {
            var child = new AlwaysSuccessNode();
            var node = new ForceSuccessNode(child);

            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
        }

        [Fact]
        public void FailureChildReturnsSuccess()
        {
            var child = new AlwaysFailureNode();
            var node = new ForceSuccessNode(child);

            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
        }
    }
}
