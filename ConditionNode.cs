using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// A behaviour tree leaf node for test property of game.
    /// </summary>
    public class ConditionNode : Node
    {
        public ConditionNode() : base()
        {

        }

        public ConditionNode(string name, Func<bool> fn) : base(name)
        {
            if (fn == null)
            {
                throw new InvalidOperationException("ConditionNode " + name + "  bind null delegate.");
            }

            m_Func = fn;
        }

        public ConditionNode(ConditionNode other) : base(other)
        {

        }

        public override void Reevaluate()
        {
            SetExecutionStatus((m_Func() ? BehaviorTreeStatus.Success : BehaviorTreeStatus.Failure));
        }

        protected override void DoTick()
        {
            if (ExecutionStatus == BehaviorTreeStatus.Inactive || ExecutionStatus == BehaviorTreeStatus.Running)
            {
                SetExecutionStatus((m_Func() ? BehaviorTreeStatus.Success : BehaviorTreeStatus.Failure));
            }
        }

        public override Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable)
        {
            return new ConditionNode(this);
        }

        public override Node Clone(ISharedVariableContainer sharedVariableTable)
        {
            return new ConditionNode(this);
        }

        public void Bind(Func<bool> fn)
        {
            if (fn == null)
            {
                throw new InvalidOperationException("ConditionNode " + m_Name + "  bind null delegate.");
            }

            m_Func = fn;
        }

        /// <summary>
        /// Function to invoke for the evaluation.
        /// </summary>
        private Func<bool> m_Func;
    }
}
