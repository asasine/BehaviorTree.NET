using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Nodes.Action.Test;
using Xunit;

namespace BehaviorTree.NET.Nodes.Control.Test
{
    public class SequenceNodeTests
    {
        [Fact]
        public void NoChildrenReturnsSuccess()
        {
            var node = new SequenceNode(new INode[0]);
            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
        }

        [Fact]
        public void AllChildrenSucceed()
        {
            // a successful sequence returns success
            // halt should be called when the sequence completes
            var children = Enumerable
                .Range(0, 10)
                .Select(_ => new ReturnXNode(NodeStatus.SUCCESS))
                .ToArray();

            var node = new SequenceNode(children);

            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            foreach (var child in children)
            {
                Assert.Equal(1, child.Ticks);
                Assert.Equal(1, child.Halts);
            }
        }

        [Fact]
        public void AllChildrenFail()
        {
            // a failing sequence should return failure
            // halt should be called when a sequence fails
            var children = Enumerable
                .Range(0, 10)
                .Select(_ => new ReturnXNode(NodeStatus.FAILURE))
                .ToArray();

            var node = new SequenceNode(children);

            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                var expectedTicks = i == 0 ? 1 : 0;
                Assert.Equal(expectedTicks, child.Ticks);
                Assert.Equal(1, child.Halts);
            }
        }

        [Fact]
        public void ChildrenAfterFailureNotTicked()
        {
            // two children, one that always fails and another
            // the other should never be ticked
            // halt should be called on all children after a failure
            var alwaysFailureChild = new ReturnXNode(NodeStatus.FAILURE);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                alwaysFailureChild,
                otherChild,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(1, alwaysFailureChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
            Assert.Equal(1, alwaysFailureChild.Halts);
            Assert.Equal(1, otherChild.Halts);
        }

        [Fact]
        public void ChildrenAfterRunningNotTicked()
        {
            // two children, one that returns running and another
            // the other should never be ticked
            // since the sequence is incomplete, halt should not be called yet
            var alwaysRunningChild = new ReturnXNode(NodeStatus.RUNNING);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                alwaysRunningChild,
                otherChild,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, alwaysRunningChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
            Assert.Equal(0, alwaysRunningChild.Halts);
            Assert.Equal(0, otherChild.Halts);
        }

        [Fact]
        public void ContinuesAfterSuccess()
        {
            // three children: success, failure, other
            // index should continue after success and tick failure
            // index should restart after failure and success should be ticked again
            // halt should be called on all children after each failure
            var alwaysSuccessChild = new ReturnXNode(NodeStatus.SUCCESS);
            var alwaysFailureChild = new ReturnXNode(NodeStatus.FAILURE);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                alwaysSuccessChild,
                alwaysFailureChild,
                otherChild,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(1, alwaysSuccessChild.Ticks);
            Assert.Equal(1, alwaysFailureChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
            Assert.Equal(1, alwaysSuccessChild.Halts);
            Assert.Equal(1, alwaysFailureChild.Halts);
            Assert.Equal(1, otherChild.Halts);

            status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(2, alwaysSuccessChild.Ticks);
            Assert.Equal(2, alwaysFailureChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
            Assert.Equal(2, alwaysSuccessChild.Halts);
            Assert.Equal(2, alwaysFailureChild.Halts);
            Assert.Equal(2, otherChild.Halts);
        }

        [Fact]
        public void ResumesFromRunning()
        {
            // three children: success, running, other
            // index should continue after success and tick running
            // index should not restart after running and success should not be ticked again
            // halts should not be called on incomplete sequences
            var alwaysSuccessChild = new ReturnXNode(NodeStatus.SUCCESS);
            var alwaysRunningChild = new ReturnXNode(NodeStatus.RUNNING);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                alwaysSuccessChild,
                alwaysRunningChild,
                otherChild,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, alwaysSuccessChild.Ticks);
            Assert.Equal(1, alwaysRunningChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
            Assert.Equal(0, alwaysSuccessChild.Halts);
            Assert.Equal(0, alwaysRunningChild.Halts);
            Assert.Equal(0, otherChild.Halts);

            status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, alwaysSuccessChild.Ticks);
            Assert.Equal(2, alwaysRunningChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
            Assert.Equal(0, alwaysSuccessChild.Halts);
            Assert.Equal(0, alwaysRunningChild.Halts);
            Assert.Equal(0, otherChild.Halts);
        }

        [Fact]
        public void RestartsAfterHalt()
        {
            // sequence should restart from beginning when halted while still running
            var alwaysSuccessChild = new ReturnXNode(NodeStatus.SUCCESS);
            var alwaysRunningChild = new ReturnXNode(NodeStatus.RUNNING);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                alwaysSuccessChild,
                alwaysRunningChild,
                otherChild,
            });

            // starts at the beginning
            var status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, alwaysSuccessChild.Ticks);
            Assert.Equal(1, alwaysRunningChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);

            // resumes from running child
            status = node.Tick();
            Assert.Equal(1, alwaysSuccessChild.Ticks);
            Assert.Equal(2, alwaysRunningChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);

            node.Halt();
            Assert.Equal(1, alwaysSuccessChild.Halts);
            Assert.Equal(1, alwaysRunningChild.Halts);
            Assert.Equal(1, otherChild.Halts);

            // restarts at the beginning
            status = node.Tick();
            Assert.Equal(2, alwaysSuccessChild.Ticks);
            Assert.Equal(3, alwaysRunningChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
        }
    }
}
