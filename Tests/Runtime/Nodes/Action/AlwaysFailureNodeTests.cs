using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    [TestFixture]
    public class AlwaysFailureNodeTests
    {
        [Test]
        public void TickReturnsFailure()
        {
            var node = new AlwaysFailureNode();
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
        }
    }
}
