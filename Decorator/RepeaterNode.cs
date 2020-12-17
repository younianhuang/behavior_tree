using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// The RepeaterNode will repeat execution of its child task until the child task has been run a specified number of times.
    /// "It has the option of continuing to execute the child task even if the child task returns a failure."
    /// </summary>
    public class RepeaterNode : DecoratorNode
    {
        public const int RepeatForever = -1;

        public RepeaterNode() : base()
        {

        }

        public RepeaterNode(string name, int repeatTimes= RepeatForever, bool endOnFailure = false) : base(name)
        {
            m_RepeatTimes = repeatTimes;

            m_EndOnFailure = endOnFailure;
        }

        public RepeaterNode(RepeaterNode other) : base(other)
        {
            m_RepeatTimes = other.m_RepeatTimes;

            m_EndOnFailure = other.m_EndOnFailure;
        }

        protected override void ProcessChildStatus(BehaviorTreeStatus status)
        {
            if (m_EndOnFailure && status == BehaviorTreeStatus.Failure)
            {
                m_ExecutionCount++;
                SetExecutionStatus(BehaviorTreeStatus.Failure);
            }
            else if (status == BehaviorTreeStatus.Success || status == BehaviorTreeStatus.Failure)
            {
                m_ExecutionCount++;
                if (m_RepeatTimes != RepeatForever && m_ExecutionCount >= m_RepeatTimes)
                {
                    SetExecutionStatus(BehaviorTreeStatus.Success);
                }
                else
                {
                    m_Child.Restart();
                }
            }
        }
        
        public override void Restart()
        {
            base.Restart();
            
            m_ExecutionCount = 0;
        }

        public override Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable)
        {
            return Clone(sharedVariableTable);
        }

        public override Node Clone(ISharedVariableContainer sharedVariableTable)
        {
            var node = new RepeaterNode(this);

            node.m_Child = m_Child.Clone(sharedVariableTable);

            return node;
        }

        public int RepeatTimes
        {
            get => m_RepeatTimes;
            set => m_RepeatTimes = value;
        }

        public bool EndOnFailure
        {
            get => m_EndOnFailure;
            set => m_EndOnFailure = value;
        }

        // The number of times to repeat the execution of its child task
        [SerializeField]
        private int m_RepeatTimes = RepeatForever;

        // The number of times the child task has been run.
        private int m_ExecutionCount = 0;

        // Should the task return if the child task returns a failure
        [SerializeField]
        private bool m_EndOnFailure = false;
    }
}



