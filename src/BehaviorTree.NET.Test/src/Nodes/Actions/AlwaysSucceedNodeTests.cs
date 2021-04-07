using Xunit;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    public class AlwaysSucceedNodeTests
    {
        [Fact]
        public void TickReturnsTrue()
        {
            var node = new AlwaysSucceedNode();
            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
        }
    }
}
