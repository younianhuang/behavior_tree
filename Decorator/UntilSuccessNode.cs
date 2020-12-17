using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// The UntilSuccessNode will repeat execution of its child task until the child taskthe child task returns success.
    /// </summary>
    public class UntilSuccessNode : DecoratorNode
    {
        public UntilSuccessNode() : base()
        {

        }

        public UntilSuccessNode(string name) : base(name)
        {
            
        }

        public UntilSuccessNode(UntilSuccessNode other) : base(other)
        {

        }

        protected override void ProcessChildStatus(BehaviorTreeStatus status)
        {
            if (status == BehaviorTreeStatus.Success)
            {
                SetExecutionStatus(status);
            }
            else if (status == BehaviorTreeStatus.Failure)
            {
                m_Child.Restart();
            }
        }

        public override Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable)
        {
            return Clone(sharedVariableTable);
        }

        public override Node Clone(ISharedVariableContainer sharedVariableTable)
        {
            var node = new UntilSuccessNode(this);

            node.m_Child = m_Child.Clone(sharedVariableTable);

            return node;
        }
    }
}

