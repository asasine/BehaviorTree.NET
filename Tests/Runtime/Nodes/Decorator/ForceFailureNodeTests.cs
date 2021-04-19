using BehaviorTree.NET.Nodes.Action;
using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    [TestFixture]
    public class ForceFailureNodeTests
    {
        [Test]
        public void SuccessChildReturnsFailure()
        {
            var child = new AlwaysSuccessNode();
            var node = new ForceFailureNode(child);

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
        }

        [Test]
        public void FailureChildReturnsFailure()
        {
            var child = new AlwaysFailureNode();
            var node = new ForceFailureNode(child);

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
        }

        [Test]
        public void RunningChildReturnsRunning()
        {
            var child = new ReturnXNode(NodeStatus.RUNNING);
            var node = new ForceFailureNode(child);

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
        }
    }
}
