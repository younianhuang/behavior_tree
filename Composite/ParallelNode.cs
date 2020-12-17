using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// The parallel node will run each child task until a child task returns failure simultaneously. 
    /// The difference with sequence node is that the parallel task will run all of its children tasks simultaneously.
    /// The parallel task will return success once all of its children tasks have return success.
    /// If one tasks returns failure the parallel task will end all of the child tasks and return failure
    /// </summary>
    public class ParallelNode : CompositeNode
    {
        public ParallelNode() : base()
        {

        }

        public ParallelNode(string name) : base(name)
        {
            
        }

        public ParallelNode(ParallelNode other) : base(other)
        {

        }

        public override void Restart()
        {
            base.Restart();

            m_NumOfSuccessChildren = 0;
        }

        protected override void ProcessConditionalAbort()
        {
            if (m_AbortType == AbortType.Self || m_AbortType == AbortType.Both)
            {
                ReevaluateChildren(0, m_Children.Count);
            }
            else if (m_AbortType == AbortType.None)
            {
                ConditionalAbortChildren();
            }
        }

        protected override void ProcessChildrenTick()
        {            
            while (CanExecute())
            {
                var child = m_Children[m_CurrentChildIndex];

                // skip child already success
                if (child.ExecutionStatus != BehaviorTreeStatus.Success)
                {                    
                    BehaviorTreeStatus status = child.Tick();
                    OnChildExecuted(status);                   
                }

                m_CurrentChildIndex++;
            }

            m_CurrentChildIndex = 0;
        }

        
        protected override void OnChildExecuted(BehaviorTreeStatus status)
        {
            if (status == BehaviorTreeStatus.Success)
            {
                ///continue exe
                m_NumOfSuccessChildren++;
                if (m_NumOfSuccessChildren >= m_Children.Count)
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
            var node = new ParallelNode(this);
            
            for (int i = 0; i < m_Children.Count; i++)
            {
                node.AddChild(m_Children[i].Clone(sharedVariableTable));
            }

            return node;
        }

        int m_NumOfSuccessChildren;
                
    }
}
