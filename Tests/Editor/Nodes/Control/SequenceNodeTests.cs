using System;
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
            var node = new SequenceNode(Array.Empty<INode>());
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
        public void RestartsAfterRunning()
        {
            // three children: success, running, other
            // index should continue after success and tick running
            // index should restart after running, and success should be ticked again
            // halts should not be called on incomplete sequences
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                success,
                running,
                other,
            });

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(success.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(success.Halts, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
            Assert.That(other.Halts, Is.EqualTo(0));

            status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(success.Ticks, Is.EqualTo(2));
            Assert.That(running.Ticks, Is.EqualTo(2));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(success.Halts, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
            Assert.That(other.Halts, Is.EqualTo(0));
        }

        [Test]
        public void RestartsAfterHalt()
        {
            // sequence should restart from beginning when halted while still running
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                success,
                running,
                other,
            });

            // starts at the beginning
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(success.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));

            // restarts, up to running child
            node.Tick();
            Assert.That(success.Ticks, Is.EqualTo(2));
            Assert.That(running.Ticks, Is.EqualTo(2));
            Assert.That(other.Ticks, Is.EqualTo(0));

            node.Halt();
            Assert.That(success.Halts, Is.EqualTo(1));
            Assert.That(running.Halts, Is.EqualTo(1));
            Assert.That(other.Halts, Is.EqualTo(1));

            // restarts at the beginning
            node.Tick();
            Assert.That(success.Ticks, Is.EqualTo(3));
            Assert.That(running.Ticks, Is.EqualTo(3));
            Assert.That(other.Ticks, Is.EqualTo(0));
        }

        [Test]
        public void HaltsChildrenAfterNewChildReturnsRunning()
        {
            // four children: flip_flop, success, running, and other
            // flip_flop returns success on its first tick, running on its second tick
            // after flip_flop returns running, success and running should be halted
            // after flip_flop returns running, other should not be halted
            // after flip_flop returns running, flip_flop should not be halted
            var flip_flop = new ReturnStatusFromCollectionNode(NodeStatus.SUCCESS, NodeStatus.RUNNING);
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                flip_flop,
                success,
                running,
                other,
            });

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(flip_flop.Ticks, Is.EqualTo(1));
            Assert.That(success.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(flip_flop.Halts, Is.EqualTo(0));
            Assert.That(success.Halts, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
            Assert.That(other.Halts, Is.EqualTo(0));

            status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(flip_flop.Ticks, Is.EqualTo(2));
            Assert.That(success.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(flip_flop.Halts, Is.EqualTo(0));
            Assert.That(success.Halts, Is.EqualTo(1));
            Assert.That(running.Halts, Is.EqualTo(1));
            Assert.That(other.Halts, Is.EqualTo(0));
        }

        [Test]
        public void RepeatedRunningDoesNotHalt()
        {
            // two children: success and running
            // both nodes should not be halted
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var node = new SequenceNode(new List<INode>
            {
                success,
                running,
            });

            node.Tick();
            node.Tick();

            Assert.That(success.Ticks, Is.EqualTo(2));
            Assert.That(running.Ticks, Is.EqualTo(2));
            Assert.That(success.Halts, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
        }

        [Test]
        public void RunningAfterHaltAfterRunningDoesNotHaltPreviouslyChildren()
        {
            // four children: flip_flop, success, running, and other
            // the sequence is ticked, halted, and ticked
            // flip_flop will return success on the first tick, running on the second tick
            // normally, after flip_flop returns running, success and running should be halted
            // since the sequence is halted between the first and second tick, they should not be halted
            var flip_flop = new ReturnStatusFromCollectionNode(NodeStatus.SUCCESS, NodeStatus.RUNNING);
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<INode>
            {
                flip_flop,
                success,
                running,
                other,
            });

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(flip_flop.Ticks, Is.EqualTo(1));
            Assert.That(success.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(flip_flop.Halts, Is.EqualTo(0));
            Assert.That(success.Halts, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
            Assert.That(other.Halts, Is.EqualTo(0));

            node.Halt();
            Assert.That(flip_flop.Halts, Is.EqualTo(1));
            Assert.That(success.Halts, Is.EqualTo(1));
            Assert.That(running.Halts, Is.EqualTo(1));
            Assert.That(other.Halts, Is.EqualTo(1));

            // prime flip_flop so that it's next tick, the one from sequence, is running
            flip_flop.Tick();

            status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(flip_flop.Ticks, Is.EqualTo(3)); // two from sequence, one from primer
            Assert.That(success.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(flip_flop.Halts, Is.EqualTo(1));
            Assert.That(success.Halts, Is.EqualTo(1));
            Assert.That(running.Halts, Is.EqualTo(1));
            Assert.That(other.Halts, Is.EqualTo(1));
        }
    }
}
