using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Control.Test
{
    public class ReturnXControlNode : ControlNode
    {
        private readonly NodeStatus x;

        public ReturnXControlNode(NodeStatus x, IEnumerable<INode> children)
            : base(children)
        {
            this.x = x;
        }

        public int Halts { get; private set; }

        public override NodeStatus Tick() => this.x;

        public override void Halt()
        {
            this.Halts++;
            base.Halt();
        }
    }

    [TestFixture]
    public class ReturnXControlNodeTests
    {
        [Test]
        public void ReturnsX([Values] NodeStatus childStatus, [Values] NodeStatus expectedStatus)
        {
            // always ignores childStatus and simply returns expectedStatus
            var child = new ReturnXNode(childStatus);
            var node = new ReturnXControlNode(expectedStatus, new INode[]
            {
                child,
            });

            var actualStatus = node.Tick();
            Assert.That(actualStatus, Is.EqualTo(expectedStatus));
        }
    }

    [TestFixture]
    public class ControlNodeTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void HaltCallsAllChildrenHalt(int expectedHalts)
        {
            var children = new ReturnXNode[]
            {
                new ReturnXNode(NodeStatus.SUCCESS),
                new ReturnXNode(NodeStatus.FAILURE),
                new ReturnXNode(NodeStatus.RUNNING),
            };

            var node = new ReturnXControlNode(NodeStatus.SUCCESS, children);
            for (int i = 0; i < expectedHalts; i++)
            {
                node.Halt();
            }

            Assert.That(node.Halts, Is.EqualTo(expectedHalts));
            foreach (var child in children)
            {
                Assert.That(child.Halts, Is.EqualTo(expectedHalts));
            }
        }

        [Test]
        public void HaltChildCallsChildHalt()
        {
            var children = new ReturnXNode[]
            {
                new ReturnXNode(NodeStatus.SUCCESS),
                new ReturnXNode(NodeStatus.FAILURE),
                new ReturnXNode(NodeStatus.RUNNING),
            };

            var node = new ReturnXControlNode(NodeStatus.SUCCESS, children);

            // halt each child in turn, assert no other halts were called
            for (int haltIndex = 0; haltIndex < children.Length; haltIndex++)
            {
                node.HaltChild(haltIndex);

                for (int i = 0; i < children.Length; i++)
                {
                    var child = children[i];
                    if (i <= haltIndex)
                    {
                        Assert.That(child.Halts, Is.EqualTo(1));
                    }
                    else
                    {
                        Assert.That(child.Halts, Is.EqualTo(0));
                    }
                }
            }
        }
    }
}
