using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// The UntilFailureNode will repeat execution of its child task until the child taskthe child task returns success.
    /// </summary>
    public class UntilFailureNode : DecoratorNode
    {
        public UntilFailureNode() : base()
        {

        }

        public UntilFailureNode(string name) : base(name)
        {

        }

        public UntilFailureNode(UntilFailureNode other) : base(other)
        {

        }

        protected override void ProcessChildStatus(BehaviorTreeStatus status)
        {
            if (status == BehaviorTreeStatus.Success || status == BehaviorTreeStatus.Inactive)
            {
                m_Child.Restart();
                SetExecutionStatus(BehaviorTreeStatus.Running);
            }
            else //if (status == BehaviorTreeStatus.Failure)
            {
                SetExecutionStatus(status);
            }            
        }

        public override Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable)
        {
            return Clone(sharedVariableTable);
        }

        public override Node Clone(ISharedVariableContainer sharedVariableTable)
        {
            var node = new UntilFailureNode(this);

            node.m_Child = m_Child.Clone(sharedVariableTable);

            return node;
        }
    }
}


