﻿using System.Collections.Generic;

namespace BehaviorTree.NET.Nodes.Control
{
    public class SequenceNode : ControlNode
    {
        private int currentRunningIndex;

        public SequenceNode(IEnumerable<INode> children)
            : base(children)
        {
            this.currentRunningIndex = 0;
        }

        public override NodeStatus Tick()
        {
            for (int i = 0; i < this.Children.Count; i++)
            {
                var child = this.Children[i];
                var status = child.Tick();
                switch (status)
                {
                    case NodeStatus.SUCCESS:
                        continue;
                    case NodeStatus.FAILURE:
                        this.Halt();
                        return status;
                    case NodeStatus.RUNNING:
                        if (i != this.currentRunningIndex)
                        {
                            // a new child is running, all children after i, up to the previously running child (inclusive) should be halted
                            for (int j = i + 1; j <= this.currentRunningIndex; j++)
                            {
                                this.HaltChild(j);
                            }
                        }

                        this.currentRunningIndex = i;
                        return status;
                }
            }

            this.Halt();
            return NodeStatus.SUCCESS;
        }

        public override void Halt()
        {
            this.currentRunningIndex = 0;
            base.Halt();
        }
    }
}
