using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Control.Test
{
    [TestFixture]
    public class FallbackWithMemoryNodeTests
    {
        [Test]
        public void NoChildrenReturnsFailure()
        {
            var node = new FallbackWithMemoryNode(Array.Empty<INode>());
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
        }

        [Test]
        public void AllChildrenSucceed()
        {
            // a single successful child in a fallback should return success
            // a successful fallback should halt all children
            var children = Enumerable
                .Range(0, 10)
                .Select(_ => new ReturnXNode(NodeStatus.SUCCESS))
                .ToArray();

            var node = new FallbackWithMemoryNode(children);

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                var expectedTicks = i == 0 ? 1 : 0;
                Assert.That(child.Ticks, Is.EqualTo(expectedTicks));
                Assert.That(child.Halts, Is.EqualTo(1));
            }
        }

        [Test]
        public void AllChildrenFail()
        {
            // a failing fallback should return failure
            // halt should be called when a fallback fails
            var children = Enumerable
                .Range(0, 10)
                .Select(_ => new ReturnXNode(NodeStatus.FAILURE))
                .ToArray();

            var node = new FallbackWithMemoryNode(children);

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.FAILURE));
            foreach (var child in children)
            {
                Assert.That(child.Ticks, Is.EqualTo(1));
                Assert.That(child.Halts, Is.EqualTo(1));
            }
        }

        [Test]
        public void ChildrenAfterSuccessNotTicked()
        {
            // two children, one that always succeeds and another
            // the other should never be ticked
            // halt should be called on all children after a success
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackWithMemoryNode(new List<INode>
            {
                success,
                other,
            });

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
            Assert.That(success.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(success.Halts, Is.EqualTo(1));
            Assert.That(other.Halts, Is.EqualTo(1));
        }

        [Test]
        public void ChildrenAfterRunningNotTicked()
        {
            // two children, one that returns running and another
            // the other should never be ticked
            // since the fallback is incomplete, halt should not be called yet
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackWithMemoryNode(new List<INode>
            {
                running,
                other,
            });

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
            Assert.That(other.Halts, Is.EqualTo(0));
        }

        [Test]
        public void RestartsAfterSuccess()
        {
            // three children: failure, success, other
            // index should continue after failure and tick success
            // index should restart after success and failure should be ticked again
            // halt should be called on all children after each success
            var failure = new ReturnXNode(NodeStatus.FAILURE);
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackWithMemoryNode(new List<INode>
            {
                failure,
                success,
                other,
            });

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
            Assert.That(failure.Ticks, Is.EqualTo(1));
            Assert.That(success.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(success.Halts, Is.EqualTo(1));
            Assert.That(failure.Halts, Is.EqualTo(1));
            Assert.That(other.Halts, Is.EqualTo(1));

            status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
            Assert.That(failure.Ticks, Is.EqualTo(2));
            Assert.That(success.Ticks, Is.EqualTo(2));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(success.Halts, Is.EqualTo(2));
            Assert.That(failure.Halts, Is.EqualTo(2));
            Assert.That(other.Halts, Is.EqualTo(2));
        }

        [Test]
        public void ResumesFromRunning()
        {
            // three children: failure, running, other
            // index should continue after failure and tick running
            // index should not restart after running and failure should not be ticked again
            // halts should not be called on incomplete fallbacks
            var failure = new ReturnXNode(NodeStatus.FAILURE);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackWithMemoryNode(new List<INode>
            {
                failure,
                running,
                other,
            });

            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(failure.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(failure.Halts, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
            Assert.That(other.Halts, Is.EqualTo(0));

            status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(failure.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(2));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(failure.Halts, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
            Assert.That(other.Halts, Is.EqualTo(0));
        }

        [Test]
        public void RestartsAfterHalt()
        {
            // fallback should restart from beginning when halted while still running
            var failure = new ReturnXNode(NodeStatus.FAILURE);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackWithMemoryNode(new List<INode>
            {
                failure,
                running,
                other,
            });

            // starts at the beginning
            var status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.RUNNING));
            Assert.That(failure.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(1));
            Assert.That(other.Ticks, Is.EqualTo(0));

            // resumes from running child
            node.Tick();
            Assert.That(failure.Ticks, Is.EqualTo(1));
            Assert.That(running.Ticks, Is.EqualTo(2));
            Assert.That(other.Ticks, Is.EqualTo(0));

            node.Halt();
            Assert.That(failure.Halts, Is.EqualTo(1));
            Assert.That(running.Halts, Is.EqualTo(1));
            Assert.That(other.Halts, Is.EqualTo(1));

            // restarts at the beginning
            node.Tick();
            Assert.That(failure.Ticks, Is.EqualTo(2));
            Assert.That(running.Ticks, Is.EqualTo(3));
            Assert.That(other.Ticks, Is.EqualTo(0));
        }
    }
}
