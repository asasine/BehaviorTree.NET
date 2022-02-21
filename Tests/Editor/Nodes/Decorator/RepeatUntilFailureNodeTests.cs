using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    [TestFixture]
    public class RepeatUntilFailureNodeTests
    {
        [Test]
        public void HaltCallsChildHalt()
        {
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RepeatUntilFailureNode(child);
            node.Halt();
            Assert.That(child.Halts, Is.EqualTo(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void TicksChildOncePerTick(int n)
        {
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RepeatUntilFailureNode(child);
            for (int i = 0; i < n; i++)
            {
                node.Tick();
            }

            Assert.That(child.Ticks, Is.EqualTo(n));
        }

        [Test]
        [TestCase(NodeStatus.SUCCESS, NodeStatus.RUNNING)]
        [TestCase(NodeStatus.RUNNING, NodeStatus.RUNNING)]
        [TestCase(NodeStatus.FAILURE, NodeStatus.SUCCESS)]
        public void ReturnsAppropriateStatus(NodeStatus expectedChildStatus, NodeStatus expectedParentStatus)
        {
            var child = new ReturnXNode(expectedChildStatus);
            var node = new RepeatUntilFailureNode(child);
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(expectedParentStatus));
        }

        [Test]
        public void HaltsOnChildFailure()
        {
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RepeatUntilFailureNode(child);
            var status = node.Tick();
            Assert.That(child.Halts, Is.EqualTo(1));
        }
    }
}
