using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    [TestFixture]
    public class AlwaysSuccessNodeTests
    {
        [Test]
        public void TickReturnsSuccess()
        {
            var node = new AlwaysSuccessNode();
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
        }
    }
}
