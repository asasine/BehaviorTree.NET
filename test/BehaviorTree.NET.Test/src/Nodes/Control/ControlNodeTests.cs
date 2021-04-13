using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Nodes.Action.Test;
using BehaviorTree.NET.Blackboard;
using Xunit;

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

    public class ReturnXControlNodeTests
    {
        public static IEnumerable<object[]> ReturnsXTestCases()
        {
            var statuses = Enum.GetValues(typeof(NodeStatus)).Cast<NodeStatus>().ToArray();
            foreach (var childStatus in statuses)
            {
                foreach (var expectedStatus in statuses)
                {
                    yield return new object[] { childStatus, expectedStatus };
                }
            }
        }

        [Theory]
        [MemberData(nameof(ReturnsXTestCases))]
        public void ReturnsX(NodeStatus childStatus, NodeStatus expectedStatus)
        {
            // always ignores childStatus and simply returns expectedStatus
            var child = new ReturnXNode(childStatus);
            var node = new ReturnXControlNode(expectedStatus, new INode[]
            {
                child,
            });

            var actualStatus = node.Tick();
            Assert.Equal(expectedStatus, actualStatus);
        }
    }

    public class ControlNodeTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
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

            Assert.Equal(expectedHalts, node.Halts);
            foreach (var child in children)
            {
                Assert.Equal(expectedHalts, child.Halts);
            }
        }

        [Fact]
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
                        Assert.Equal(1, child.Halts);
                    }
                    else
                    {
                        Assert.Equal(0, child.Halts);
                    }
                }
            }
        }
    }
}
