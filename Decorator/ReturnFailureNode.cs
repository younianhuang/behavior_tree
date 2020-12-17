using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// The UntilSuccessNode will repeat execution of its child task until the child taskthe child task returns success.
    /// </summary>
    public class ReturnFailureNode : DecoratorNode
    {
        public ReturnFailureNode() : base()
        {

        }

        public ReturnFailureNode(string name) : base(name)
        {

        }

        public ReturnFailureNode(ReturnFailureNode other) : base(other)
        {

        }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>

        protected override void ProcessChildStatus(BehaviorTreeStatus status)
        {
            if (status == BehaviorTreeStatus.Success)
            {
                status = BehaviorTreeStatus.Failure;
            }

            SetExecutionStatus(status);
        }

        public override Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable)
        {
            var node = new ReturnFailureNode(this);

            node.m_Child = m_Child.Clone(agent, world, sharedVariableTable);

            return node;
        }

        public override Node Clone(ISharedVariableContainer sharedVariableTable)
        {
            var node = new ReturnFailureNode(this);

            node.m_Child = m_Child.Clone(sharedVariableTable);

            return node;
        }
    }
}

