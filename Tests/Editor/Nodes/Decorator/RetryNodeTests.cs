using BehaviorTree.NET.Variables;
using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    [TestFixture]
    public class RetryNodeTests
    {
        [Test]
        public void ReturnsSuccessImmediately()
        {
            var n = new InMemoryVariable<int>(0);
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RetryNode(child, n);
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
            Assert.That(child.Ticks, Is.EqualTo(1));
        }

        [Test]
        public void HaltsAfterSuccess()
        {
            var n = new InMemoryVariable<int>(0);
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new RetryNode(child, n);
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
            Assert.That(child.Halts, Is.EqualTo(1));
        }

        [Test]
        public void HaltsAfterFailure()
        {
            var n = new InMemoryVariable<int>(0);
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RetryNode(child, n);
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
            Assert.That(child.Halts, Is.EqualTo(1));
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void DoesNotHaltAfterRunning(int n)
        {
            var child = new ReturnXNode(NodeStatus.RUNNING);
            var node = new RetryNode(child, new InMemoryVariable<int>(n));
            for (int i = 0; i < n; i++)
            {
                var status = node.Tick();
                Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            }

            Assert.That(child.Ticks, Is.EqualTo(n));
            Assert.That(child.Halts, Is.EqualTo(0));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void ReturnsFailureAfterNFailures(int n)
        {
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new RetryNode(child, new InMemoryVariable<int>(n));
            for (int i = 0; i < n; i++)
            {
                Assert.That(node.Tick(), Is.EqualTo(NodeStatus.RUNNING));
            }

            Assert.That(node.Tick(), Is.EqualTo(NodeStatus.FAILURE));

            // ticked once at the beginning and once more for every tick that returned failure until n
            Assert.That(child.Ticks, Is.EqualTo(n + 1));
        }
    }
}
