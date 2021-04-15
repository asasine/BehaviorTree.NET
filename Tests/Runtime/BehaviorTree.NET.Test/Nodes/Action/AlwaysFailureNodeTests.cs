using Xunit;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    public class AlwaysFailureNodeTests
    {
        [Fact]
        public void TickReturnsFailure()
        {
            var node = new AlwaysFailureNode();
            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
        }
    }
}
