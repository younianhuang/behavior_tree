using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    /// <summary>
    /// The InverterNode will invert the return value of the child after it has finished executing.
    /// If the child returns success, the inverter task will return failure.If the child returns failure, the inverter task will return success.
    /// </summary>
    public class InverterNode : DecoratorNode
    {
        public InverterNode() : base()
        {

        }

        public InverterNode(string name) : base(name)
        {
           
        }

        public InverterNode(InverterNode other) : base(other)
        {

        }

        protected override void ProcessChildStatus(BehaviorTreeStatus status)
        {
            if (status == BehaviorTreeStatus.Success)
            {
                SetExecutionStatus(BehaviorTreeStatus.Failure);
            }
            else if (status == BehaviorTreeStatus.Failure)
            {
                SetExecutionStatus(BehaviorTreeStatus.Success);
            }
            else
            {
                SetExecutionStatus(BehaviorTreeStatus.Running);
            }
        }

        public override Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable)
        {
            return Clone(sharedVariableTable);
        }

        public override Node Clone(ISharedVariableContainer sharedVariableTable)
        {
            var node = new InverterNode(this);

            node.m_Child = m_Child.Clone(sharedVariableTable);

            return node;
        }
    }
}



