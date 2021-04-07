using System.Collections.Generic;
using BehaviorTree.NET.Nodes.Action.Test;
using Xunit;

namespace BehaviorTree.NET.Nodes.Control.Test
{
    public class FallbackNodeTests
    {
        [Fact]
        public void NoChildrenReturnsFailure()
        {
            var node = new FallbackNode(new Node[0]);
            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
        }

        [Fact]
        public void OneSuccessChildReturnsSuccess()
        {
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<Node>
            {
                child,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(1, child.Ticks);
        }

        [Fact]
        public void OneFailureChildReturnsFailure()
        {
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new FallbackNode(new List<Node>
            {
                child,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(1, child.Ticks);
        }

        [Fact]
        public void ChildrenAfterSuccessNotTicked()
        {
            // two children: success and another
            // other should not be ticked
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<Node>
            {
                success,
                other,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(1, success.Ticks);
            Assert.Equal(0, other.Ticks);
        }

        [Fact]
        public void ChildrenAfterRunningNotTicked()
        {
            // two children: running and another
            // other should not be ticked
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<Node>
            {
                running,
                other,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, running.Ticks);
            Assert.Equal(0, other.Ticks);
        }

        [Fact]
        public void ContinuesAfterFailure()
        {
            // three children: failure, success, and another
            // index should continue after failure and tick success
            // index should restart after success and failure should be ticked again
            var failure = new ReturnXNode(NodeStatus.FAILURE);
            var success = new ReturnXNode(NodeStatus.SUCCESS);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<Node>
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

            status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(2, failure.Ticks);
            Assert.Equal(2, success.Ticks);
            Assert.Equal(0, other.Ticks);
        }

        [Fact]
        public void ContinuesAfterRunning()
        {
            // three children: failure, running, and another
            // index should continue after failure and tick running
            // index should restart after running and failure should be ticked again
            var failure = new ReturnXNode(NodeStatus.FAILURE);
            var running = new ReturnXNode(NodeStatus.RUNNING);
            var other = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new FallbackNode(new List<Node>
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

            status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(2, failure.Ticks);
            Assert.Equal(2, running.Ticks);
            Assert.Equal(0, other.Ticks);
        }
    }
}