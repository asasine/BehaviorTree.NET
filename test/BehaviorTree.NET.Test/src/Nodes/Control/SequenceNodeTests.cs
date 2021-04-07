using System.Collections.Generic;
using BehaviorTree.NET.Nodes.Action.Test;
using Xunit;

namespace BehaviorTree.NET.Nodes.Control.Test
{
    public class SequenceNodeTests
    {
        [Fact]
        public void NoChildrenReturnsSuccess()
        {
            var node = new SequenceNode(new Node[0]);
            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
        }

        [Fact]
        public void OneSuccessChildReturnsSuccess()
        {
            var child = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<Node>
            {
               child,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.SUCCESS, status);
            Assert.Equal(1, child.Ticks);
        }

        [Fact]
        public void OneFailureChildReturnsSuccess()
        {
            var child = new ReturnXNode(NodeStatus.FAILURE);
            var node = new SequenceNode(new List<Node>
            {
                child,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(1, child.Ticks);
        }

        [Fact]
        public void ChildrenAfterFailureNotTicked()
        {
            // two children, one that always fails and another
            // the other should never be ticked
            var alwaysFailureChild = new ReturnXNode(NodeStatus.FAILURE);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<Node>
            {
                alwaysFailureChild,
                otherChild,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(1, alwaysFailureChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
        }

        [Fact]
        public void ChildrenAfterRunningNotTicked()
        {
            // two children, one that returns failure and another
            // the other should never be ticked
            var alwaysRunningChild = new ReturnXNode(NodeStatus.RUNNING);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<Node>
            {
                alwaysRunningChild,
                otherChild,
            });

            var status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, alwaysRunningChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
        }

        [Fact]
        public void RestartsAfterFailure()
        {
            // three children: success, failure, other
            // index should restart after failure and success should be ticked again
            var alwaysSuccessChild = new ReturnXNode(NodeStatus.SUCCESS);
            var alwaysFailureChild = new ReturnXNode(NodeStatus.FAILURE);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<Node>
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

            status = node.Tick();
            Assert.Equal(NodeStatus.FAILURE, status);
            Assert.Equal(2, alwaysSuccessChild.Ticks);
            Assert.Equal(2, alwaysFailureChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
        }

        [Fact]
        public void DoesNotRestartAfterRunning()
        {
            // three children: success, running, other
            // index should not restart afterrunning and success should not be ticked again
            var alwaysSuccessChild = new ReturnXNode(NodeStatus.SUCCESS);
            var alwaysRunningChild = new ReturnXNode(NodeStatus.RUNNING);
            var otherChild = new ReturnXNode(NodeStatus.SUCCESS);
            var node = new SequenceNode(new List<Node>
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

            status = node.Tick();
            Assert.Equal(NodeStatus.RUNNING, status);
            Assert.Equal(1, alwaysSuccessChild.Ticks);
            Assert.Equal(2, alwaysRunningChild.Ticks);
            Assert.Equal(0, otherChild.Ticks);
        }
    }
}
