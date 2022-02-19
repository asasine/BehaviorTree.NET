using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Action.Test
{
    /// <summary>
    /// This node returns statuses from a <see cref="IReadOnlyCollection{T}"/> of <see cref="NodeStatus"/>.
    /// After exhausting the collection, tick will restart from the beginning.
    /// </summary>
    public class ReturnStatusFromCollectionNode : INode
    {
        private readonly IReadOnlyCollection<NodeStatus> statuses;
        private int statusIndex;

        public ReturnStatusFromCollectionNode(params NodeStatus[] statuses)
            : this((IReadOnlyCollection<NodeStatus>)statuses)
        {
        }

        public ReturnStatusFromCollectionNode(IReadOnlyCollection<NodeStatus> statuses)
        {
            this.statuses = statuses;
            this.statusIndex = 0;
            this.Ticks = 0;
            this.Halts = 0;
        }

        public int Ticks { get; private set; }
        public int Halts { get; private set; }

        public NodeStatus Tick()
        {
            this.Ticks++;
            var status = this.statuses.ElementAt(this.statusIndex);
            this.statusIndex = (this.statusIndex + 1) % statuses.Count;
            return status;
        }

        public void Halt()
        {
            this.Halts++;
        }
    }

    [TestFixture]
    public class ReturnStatusFromCollectionNodeTests
    {
        [Test]
        public void NoTicks()
        {
            var node = new ReturnStatusFromCollectionNode();
            Assert.That(node.Ticks, Is.EqualTo(0));
            Assert.That(node.Halts, Is.EqualTo(0));
        }

        [Test]
        public void ParamsConstructor()
        {
            var node = new ReturnStatusFromCollectionNode(NodeStatus.SUCCESS, NodeStatus.FAILURE);
            var actualStatus = node.Tick();
            Assert.That(actualStatus, Is.EqualTo(NodeStatus.SUCCESS));

            actualStatus = node.Tick();
            Assert.That(actualStatus, Is.EqualTo(NodeStatus.FAILURE));
        }

        [Test]
        public void IReadOnlyCollectionConstructor()
        {
            var node = new ReturnStatusFromCollectionNode(new List<NodeStatus>
            {
                NodeStatus.SUCCESS,
                NodeStatus.FAILURE,
            });

            var actualStatus = node.Tick();
            Assert.That(actualStatus, Is.EqualTo(NodeStatus.SUCCESS));

            actualStatus = node.Tick();
            Assert.That(actualStatus, Is.EqualTo(NodeStatus.FAILURE));
        }

        [Test]
        [TestCase(NodeStatus.SUCCESS)]
        [TestCase(NodeStatus.FAILURE)]
        [TestCase(NodeStatus.RUNNING)]
        public void ReturnsSingleElementOnce(NodeStatus expectedStatus)
        {
            var node = new ReturnStatusFromCollectionNode(expectedStatus);
            var actualStatus = node.Tick();
            Assert.That(actualStatus, Is.EqualTo(expectedStatus));
            Assert.That(node.Ticks, Is.EqualTo(1));
        }

        [Test]
        [TestCase(NodeStatus.SUCCESS, 10)]
        [TestCase(NodeStatus.FAILURE, 100)]
        [TestCase(NodeStatus.RUNNING, 42)]
        public void ReturnsSingleElementNTimes(NodeStatus expectedStatus, int expectedTicks)
        {
            var node = new ReturnStatusFromCollectionNode(expectedStatus);
            for (int i = 0; i < expectedTicks; i++)
            {
                var actualStatus = node.Tick();
                Assert.That(actualStatus, Is.EqualTo(expectedStatus));
            }

            Assert.That(node.Ticks, Is.EqualTo(expectedTicks));
        }

        [Test]
        public void RepeatsSequence()
        {
            const int n = 10;
            var node = new ReturnStatusFromCollectionNode(NodeStatus.SUCCESS, NodeStatus.FAILURE);
            for (int i = 0; i <= n; i++)
            {
                var actualStatus = node.Tick();
                Assert.That(actualStatus, Is.EqualTo(NodeStatus.SUCCESS));

                actualStatus = node.Tick();
                Assert.That(actualStatus, Is.EqualTo(NodeStatus.FAILURE));
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void HaltCounter(int expectedHalts)
        {
            var node = new ReturnStatusFromCollectionNode(NodeStatus.SUCCESS);
            for (int i = 0; i < expectedHalts; i++)
            {
                node.Halt();
            }

            Assert.That(node.Halts, Is.EqualTo(expectedHalts));
        }
    }
}
