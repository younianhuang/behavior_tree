using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : Node
    {
        public ActionNode() : base()
        {

        }

        public ActionNode(string name, Func<BehaviorTreeStatus> fn) : base(name)
        {
            if (fn == null)
            {
                throw new InvalidOperationException("ActionNode "+ name + "  bind null delegate.");
            }

            m_Func = fn;
        }

        public ActionNode(ActionNode other) : base(other)
        {

        }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        protected override void DoTick()
        {            
            SetExecutionStatus(m_Func());
        }

        public override Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable)
        {
            return new ActionNode(this);
        }

        public override Node Clone(ISharedVariableContainer sharedVariableTable)
        {
            return new ActionNode(this);            
        }

        public void Bind(Func<BehaviorTreeStatus> fn)
        {
            if (fn == null)
            {
                throw new InvalidOperationException("ActionNode " + m_Name + "  bind null delegate.");
            }

            m_Func = fn;
        }

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private Func<BehaviorTreeStatus> m_Func;
    }
}
