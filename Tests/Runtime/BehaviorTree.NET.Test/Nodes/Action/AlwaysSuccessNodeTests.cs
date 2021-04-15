using Xunit;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    public class AlwaysSuccessNodeTests
    {
        [Fact]
        public void TickReturnsSuccess()
        {
            var node = new AlwaysSuccessNode();
            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
        }
    }
}
