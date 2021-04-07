using System.Collections.Generic;
using System.Linq;
using BehaviorTree.NET.Nodes.Action.Test;
using Xunit;

namespace BehaviorTree.NET.Nodes.Control.Test
{
    public class FallbackNodeTests
    {
        [Fact]
        public void NoChildrenReturnsFailure()
        {
            var node = new FallbackNode(new INode[0]);
            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
        }

        [Fact]
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
            Assert.Equal(NodeStatus.SUCCESS, status);
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                var expectedTicks = i == 0 ? 1 : 0;
                Assert.Equal(expectedTicks, child.Ticks);
                Assert.Equal(1, child.Halts);
            }
        }

        [Fact]
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
            Assert.Equal(NodeStatus.FAILURE, status);
            foreach (var child in children)
            {
                Assert.Equal(1, child.Ticks);
                Assert.Equal(1, child.Halts);
            }
        }

        [Fact]
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
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(1, success.Ticks);
            Assert.Equal(0, other.Ticks);
            Assert.Equal(1, success.Halts);
            Assert.Equal(1, other.Halts);
        }

        [Fact]
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
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, running.Ticks);
            Assert.Equal(0, other.Ticks);
            Assert.Equal(0, running.Halts);
            Assert.Equal(0, other.Halts);
        }

        [Fact]
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
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(1, failure.Ticks);
            Assert.Equal(1, success.Ticks);
            Assert.Equal(0, other.Ticks);
            Assert.Equal(1, failure.Halts);
            Assert.Equal(1, success.Halts);
            Assert.Equal(1, other.Halts);

            status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(2, failure.Ticks);
            Assert.Equal(2, success.Ticks);
            Assert.Equal(0, other.Ticks);
            Assert.Equal(2, failure.Halts);
            Assert.Equal(2, success.Halts);
            Assert.Equal(2, other.Halts);
        }

        [Fact]
        public void ContinuesAfterRunning()
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
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, failure.Ticks);
            Assert.Equal(1, running.Ticks);
            Assert.Equal(0, other.Ticks);
            Assert.Equal(0, failure.Halts);
            Assert.Equal(0, running.Halts);
            Assert.Equal(0, other.Halts);

            status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(2, failure.Ticks);
            Assert.Equal(2, running.Ticks);
            Assert.Equal(0, other.Ticks);
            Assert.Equal(0, failure.Halts);
            Assert.Equal(0, running.Halts);
            Assert.Equal(0, other.Halts);
        }
    }
}
