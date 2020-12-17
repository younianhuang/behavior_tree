using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// The selector task is similar to an "or" operation. It will return success as soon as one of its child tasks return success.
    /// If a child task returns failure then it will sequentially run the next task. If no child task returns success then it will return failure.
    /// </summary>
    public class SelectorNode : CompositeNode
    {
        public SelectorNode() : base()
        {

        }

        public SelectorNode(string name) : base(name)
        {
            m_CurrentChildIndex = 0;
        }

        public SelectorNode(SelectorNode other) : base(other)
        {

        }

        protected override void OnChildExecuted(BehaviorTreeStatus status)
        {
            if (status == BehaviorTreeStatus.Failure)
            {
                ///continue exe
                m_CurrentChildIndex++;
                if (m_CurrentChildIndex >= m_Children.Count)
                {
                    SetExecutionStatus(status);                    
                }
                else
                {
                    SetExecutionStatus(BehaviorTreeStatus.Running);
                }
            }
            else
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
            var node = new SelectorNode(this);
            
            for (int i = 0; i < m_Children.Count; i++)
            {
                node.AddChild(m_Children[i].Clone(sharedVariableTable));
            }

            return node;
        }
    }
}
