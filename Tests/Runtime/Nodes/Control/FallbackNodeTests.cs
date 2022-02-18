using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Nodes.Action.Test;
using NUnit.Framework;

namespace BehaviorTree.NET.Nodes.Control.Test
{
    [TestFixture]
    public class FallbackNodeTests
    {
        [Test]
        public void NoChildrenReturnsFailure()
        {
            var node = new FallbackNode(new INode[0]);
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

            var node = new FallbackNode(children);

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
            // all children failing should fail a fallback
            // a failed fallback should halt all children
            var children = Enumerable
                .Range(0, 10)
                .Select(_ => new ReturnXNode(NodeStatus.FAILURE))
                .ToArray();

            var node = new FallbackNode(children);

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
            // two children: success and another
            // other should not be ticked
            // a successful fallback should halt all children
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<INode>
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
            // two children: running and another
            // other should not be ticked
            // a running fallback should not halt any children yet
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<INode>
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
        public void ContinuesAfterFailure()
        {
            // three children: failure, success, and another
            // index should continue after failure and tick success
            // index should restart after success and failure should be ticked again
            // a successful fallback should halt all children
            var failure = new ReturnXNode(NodeStatus.FAILURE);
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<INode>
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
            Assert.That(failure.Halts, Is.EqualTo(1));
            Assert.That(success.Halts, Is.EqualTo(1));
            Assert.That(other.Halts, Is.EqualTo(1));

            status = node.Tick();
            Assert.That(status, Is.EqualTo(NodeStatus.SUCCESS));
            Assert.That(failure.Ticks, Is.EqualTo(2));
            Assert.That(success.Ticks, Is.EqualTo(2));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(failure.Halts, Is.EqualTo(2));
            Assert.That(success.Halts, Is.EqualTo(2));
            Assert.That(other.Halts, Is.EqualTo(2));
        }

        [Test]
        public void RestartsAfterRunning()
        {
            // three children: failure, running, and another
            // index should continue after failure and tick running
            // index should restart after running and failure should be ticked again
            // an incomplete fallback should not halt any children yet
            var failure = new ReturnXNode(NodeStatus.FAILURE);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<INode>
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
            Assert.That(failure.Ticks, Is.EqualTo(2));
            Assert.That(running.Ticks, Is.EqualTo(2));
            Assert.That(other.Ticks, Is.EqualTo(0));
            Assert.That(failure.Halts, Is.EqualTo(0));
            Assert.That(running.Halts, Is.EqualTo(0));
            Assert.That(other.Halts, Is.EqualTo(0));
        }
    }
}
