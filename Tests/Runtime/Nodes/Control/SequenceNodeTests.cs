using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Control.Test
{
    [TestFixture]
    public class SequenceNodeTests
    {
        [Test]
        public void NoChildrenReturnsSuccess()
        {
            var node = new SequenceNode(new INode[0]);
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
        }

        [Test]
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
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
            foreach (var child in children)
            {
                Assert.That(child.Ticks, Is.EqualTo(1));
                Assert.That(child.Halts, Is.EqualTo(1));
            }
        }

        [Test]
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
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                var expectedTicks = i == 0 ? 1 : 0;
                Assert.That(child.Ticks, Is.EqualTo(expectedTicks));
                Assert.That(child.Halts, Is.EqualTo(1));
            }
        }

        [Test]
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
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
            Assert.That(alwaysFailureChild.Ticks, Is.EqualTo(1));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));
            Assert.That(alwaysFailureChild.Halts, Is.EqualTo(1));
            Assert.That(otherChild.Halts, Is.EqualTo(1));
        }

        [Test]
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
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(alwaysRunningChild.Ticks, Is.EqualTo(1));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));
            Assert.That(alwaysRunningChild.Halts, Is.EqualTo(0));
            Assert.That(otherChild.Halts, Is.EqualTo(0));
        }

        [Test]
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
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
            Assert.That(alwaysSuccessChild.Ticks, Is.EqualTo(1));
            Assert.That(alwaysFailureChild.Ticks, Is.EqualTo(1));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));
            Assert.That(alwaysSuccessChild.Halts, Is.EqualTo(1));
            Assert.That(alwaysFailureChild.Halts, Is.EqualTo(1));
            Assert.That(otherChild.Halts, Is.EqualTo(1));

            status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
            Assert.That(alwaysSuccessChild.Ticks, Is.EqualTo(2));
            Assert.That(alwaysFailureChild.Ticks, Is.EqualTo(2));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));
            Assert.That(alwaysSuccessChild.Halts, Is.EqualTo(2));
            Assert.That(alwaysFailureChild.Halts, Is.EqualTo(2));
            Assert.That(otherChild.Halts, Is.EqualTo(2));
        }

        [Test]
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
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(alwaysSuccessChild.Ticks, Is.EqualTo(1));
            Assert.That(alwaysRunningChild.Ticks, Is.EqualTo(1));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));
            Assert.That(alwaysSuccessChild.Halts, Is.EqualTo(0));
            Assert.That(alwaysRunningChild.Halts, Is.EqualTo(0));
            Assert.That(otherChild.Halts, Is.EqualTo(0));

            status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(alwaysSuccessChild.Ticks, Is.EqualTo(1));
            Assert.That(alwaysRunningChild.Ticks, Is.EqualTo(2));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));
            Assert.That(alwaysSuccessChild.Halts, Is.EqualTo(0));
            Assert.That(alwaysRunningChild.Halts, Is.EqualTo(0));
            Assert.That(otherChild.Halts, Is.EqualTo(0));
        }

        [Test]
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
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(alwaysSuccessChild.Ticks, Is.EqualTo(1));
            Assert.That(alwaysRunningChild.Ticks, Is.EqualTo(1));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));

            // resumes from running child
            status = node.Tick();
            Assert.That(alwaysSuccessChild.Ticks, Is.EqualTo(1));
            Assert.That(alwaysRunningChild.Ticks, Is.EqualTo(2));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));

            node.Halt();
            Assert.That(alwaysSuccessChild.Halts, Is.EqualTo(1));
            Assert.That(alwaysRunningChild.Halts, Is.EqualTo(1));
            Assert.That(otherChild.Halts, Is.EqualTo(1));

            // restarts at the beginning
            status = node.Tick();
            Assert.That(alwaysSuccessChild.Ticks, Is.EqualTo(2));
            Assert.That(alwaysRunningChild.Ticks, Is.EqualTo(3));
            Assert.That(otherChild.Ticks, Is.EqualTo(0));
        }
    }
}
