using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// The UntilSuccessNode will repeat execution of its child task until the child taskthe child task returns success.
    /// </summary>
    public class ReturnSuccessNode : DecoratorNode
    {
        public ReturnSuccessNode() : base()
        {

        }

        public ReturnSuccessNode(string name) : base(name)
        {

        }

        public ReturnSuccessNode(ReturnSuccessNode other) : base(other)
        {

        }

        protected override void ProcessChildStatus(BehaviorTreeStatus status)
        {
            if (status == BehaviorTreeStatus.Failure)
            {
                status = BehaviorTreeStatus.Success;
            }

            SetExecutionStatus(status);
        }

        public override Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable)
        {
            return Clone(sharedVariableTable);
        }

        public override Node Clone(ISharedVariableContainer sharedVariableTable)
        {
            var node = new ReturnSuccessNode(this);

            node.m_Child = m_Child.Clone(sharedVariableTable);

            return node;
        }
    }
}


