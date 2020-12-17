
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    /// <summary>
    /// The decorator node is a wrapper that can only have one child. The decorator task will modify the behavior of the child task in some way
    /// </summary>
    public abstract class DecoratorNode : ParentNode
    {
        public DecoratorNode() : base()
        {

        }

        public DecoratorNode(string name) : base(name)
        {
            m_Child = null;
        }

        public DecoratorNode(DecoratorNode other) : base(other)
        {

        }

        /// <summary>
        /// Add a child to this node.
        /// </summary>
        public override void AddChild(Node child)
        {
            if (m_Child != null)
            {                
                throw new InvalidOperationException("DecoratorNode " + m_Name + " already has child node");
            }

            m_Child = child;
        }

        public override void SetExternalSource(object agent, object world)
        {
            if (m_Child != null)
            {
                m_Child.SetExternalSource(agent, world);
            }
        }

        /// <summary>
        /// Awake is used to initialize any variables or state before this node start. 
        /// Awake is called only once during the lifetime of this node.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            m_Child.Awake();
        }

        public override void Restart()
        {
            base.Restart();

            m_Child.Restart();
        }

        protected override void DoTick()
        {
            BehaviorTreeStatus status = m_Child.Tick();
            ProcessChildStatus(status);
        }

        public override void Reevaluate()
        {
            BehaviorTreeStatus previousStatus = m_Child.ExecutionStatus;
            m_Child.Reevaluate();
            if (m_Child.ExecutionStatus != previousStatus)
            {
                ProcessChildStatus(m_Child.ExecutionStatus);
            }
        }

        public override bool ConditionalAbort()
        {                       
            if (m_Child.ConditionalAbort())
            {
                ProcessChildStatus(m_Child.ExecutionStatus);
                return true;
            }

            return false;
        }

        public override void OnAborted()
        {
            m_Child.OnAborted();

            base.OnAborted();
        }

        protected abstract void ProcessChildStatus(BehaviorTreeStatus status);

        //protected abstract void ProcessExecutionStatus(BehaviorTreeStatus status);

        /// <summary>
        /// List of child nodes.
        /// </summary>
        [SerializeField]
        protected Node m_Child;
    }
}

