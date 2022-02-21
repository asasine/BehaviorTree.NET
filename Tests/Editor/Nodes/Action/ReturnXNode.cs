using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    public class ReturnXNode : ReturnStatusFromCollectionNode
    {
        public ReturnXNode(NodeStatus x)
            : base(new NodeStatus[] { x })
        {
        }
    }

    [TestFixture]
    public class ReturnXNodeTests
    {
        [Test]
        [TestCase(NodeStatus.SUCCESS)]
        [TestCase(NodeStatus.FAILURE)]
        [TestCase(NodeStatus.RUNNING)]
        public void NoTicks(NodeStatus expectedStatus)
        {
            var node = new ReturnXNode(expectedStatus);
            Assert.That(node.Ticks, Is.EqualTo(0));
            Assert.That(node.Halts, Is.EqualTo(0));
        }

        [Test]
        [TestCase(NodeStatus.SUCCESS)]
        [TestCase(NodeStatus.FAILURE)]
        [TestCase(NodeStatus.RUNNING)]
        public void ReturnsExpectedOnce(NodeStatus expectedStatus)
        {
            var node = new ReturnXNode(expectedStatus);
            var actualStatus = node.Tick();
            Assert.That(actualStatus, Is.EqualTo(expectedStatus));
            Assert.That(node.Ticks, Is.EqualTo(1));
        }

        [Test]
        [TestCase(NodeStatus.SUCCESS, 10)]
        [TestCase(NodeStatus.FAILURE, 100)]
        [TestCase(NodeStatus.RUNNING, 42)]
        public void ReturnsExpectedNTimes(NodeStatus expectedStatus, int expectedTicks)
        {
            var node = new ReturnXNode(expectedStatus);
            for (int i = 0; i < expectedTicks; i++)
            {
                var actualStatus = node.Tick();
                Assert.That(actualStatus, Is.EqualTo(expectedStatus));
            }

            Assert.That(node.Ticks, Is.EqualTo(expectedTicks));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void HaltCounter(int expectedHalts)
        {
            var node = new ReturnXNode(NodeStatus.SUCCESS);
            for (int i = 0; i < expectedHalts; i++)
            {
                node.Halt();
            }

            Assert.That(node.Halts, Is.EqualTo(expectedHalts));
        }
    }
}
