using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Decorator.Test
{
    public class ForwardToChildNode : DecoratorNode
    {
        public ForwardToChildNode(INode child)
            : base(child)
        {
        }

        public int Halts { get; private set; }

        public override NodeStatus Tick() => this.Child.Tick();

        public override void Halt()
        {
            this.Halts++;
            base.Halt();
        }
    }

    [TestFixture]
    public class ForwardToChildNodeTests
    {
        [Test]
        [TestCase(NodeStatus.SUCCESS, 1)]
        [TestCase(NodeStatus.FAILURE, 10)]
        [TestCase(NodeStatus.RUNNING, 100)]
        public void TicksChild(NodeStatus expectedStatus, int expectedTicks)
        {
            var child = new ReturnXNode(expectedStatus);
            var node = new ForwardToChildNode(child);

            Assert.That(child.Ticks, Is.EqualTo(0));
            for (int i = 0; i < expectedTicks; i++)
            {
                var status = node.Tick();
            }

            Assert.That(child.Ticks, Is.EqualTo(expectedTicks));
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void HaltsChild(int expectedHalts)
        {
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new ForwardToChildNode(child);

            Assert.That(child.Halts, Is.EqualTo(0));
            for (int i = 0; i < expectedHalts; i++)
            {
                node.Halt();
            }

            Assert.That(node.Halts, Is.EqualTo(expectedHalts));
            Assert.That(child.Halts, Is.EqualTo(expectedHalts));
        }
    }

    [TestFixture]
    public class DecoratorNodeTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void HaltsChild(int expectedHalts)
        {
            // ForwardToChild simply calls the base class halt (DecoratorNode)
            // the default implementation should call the child's halt
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new ForwardToChildNode(child);

            Assert.That(child.Halts, Is.EqualTo(0));
            for (int i = 0; i < expectedHalts; i++)
            {
                node.Halt();
            }

            Assert.That(child.Halts, Is.EqualTo(expectedHalts));
        }
    }
}
