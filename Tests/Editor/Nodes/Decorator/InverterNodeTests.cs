using BehaviorTree.NET.Nodes.Action;
using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    [TestFixture]
    public class InverterNodeTests
    {
        [Test]
        public void SuccessChildReturnsFailure()
        {
            var child = new AlwaysSuccessNode();
            var inverterNode = new InverterNode(child);

            var status = inverterNode.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
        }

        [Test]
        public void FailureChildReturnsSuccess()
        {
            var child = new AlwaysFailureNode();
            var inverterNode = new InverterNode(child);

            var status = inverterNode.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
        }

        [Test]
        public void RunningChildReturnsRunning()
        {
            var child = new ReturnXNode(NodeStatus.RUNNING);
            var node = new InverterNode(child);
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
        }
    }
}
