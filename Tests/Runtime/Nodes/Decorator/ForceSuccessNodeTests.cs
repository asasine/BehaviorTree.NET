using BehaviorTree.NET.Nodes.Action;
using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    [TestFixture]
    public class ForceSuccessNodeTests
    {
        [Test]
        public void SuccessChildReturnsSuccess()
        {
            var child = new AlwaysSuccessNode();
            var node = new ForceSuccessNode(child);

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
        }

        [Test]
        public void FailureChildReturnsSuccess()
        {
            var child = new AlwaysFailureNode();
            var node = new ForceSuccessNode(child);

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
        }

        [Test]
        public void RunningChildReturnsRunning()
        {
            var child = new ReturnXNode(NodeStatus.RUNNING);
            var node = new ForceSuccessNode(child);

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
        }
    }
}
