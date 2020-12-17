using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    /// <summary>
    /// The sequence node is similar to an "and" operation. It will return failure as soon as one of its child tasks return failure.
    /// If a child task returns success then it will sequentially run the next task. If all child tasks return success then it will return success.
    /// </summary>
    public class SequenceNode : CompositeNode
    {
        public SequenceNode() : base()
        {

        }

        public SequenceNode(string name) : base(name)
        {
            m_CurrentChildIndex = 0;            
        }

        public SequenceNode(SequenceNode other) : base(other)
        {

        }

        protected override void OnChildExecuted(BehaviorTreeStatus status)
        {
            if (status == BehaviorTreeStatus.Success)
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
            var node = new SequenceNode(this);            

            for (int i = 0; i < m_Children.Count; i++)
            {
                node.AddChild(m_Children[i].Clone(sharedVariableTable));
            }

            return node;
        }
    }
}
