using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// A parent node which has multi child nodes.
    /// </summary>
    public abstract class CompositeNode : ParentNode
    {
        public CompositeNode() : base()
        {
            m_Children = new List<Node>();
        }

        public CompositeNode(string name) : base(name)
        {
            m_Children = new List<Node>();

            m_AbortType = AbortType.None;
        }

        public CompositeNode(CompositeNode other) : base(other)
        {
            m_Children = new List<Node>();

            m_AbortType = other.m_AbortType;
        }

        /// <summary>
        /// Add a child to this node.
        /// </summary>
        public override void AddChild(Node child)
        {
            m_Children.Add(child);
        }

        protected bool CanExecute()
        {
            return ExecutionStatus == BehaviorTreeStatus.Running && m_CurrentChildIndex < m_Children.Count;
        }

        public override void SetExternalSource(object agent, object world)
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                m_Children[i].SetExternalSource(agent, world);                
            }
        }

        /// <summary>
        /// Awake is used to initialize any variables or state before this node start. 
        /// Awake is called only once during the lifetime of this node.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            foreach (var child in m_Children)
            {
                child.Awake();
            }            
        }

        /// <summary>
        /// Restart this node. This make all child node can be reevaluated.
        /// </summary>
        public override void Restart()
        {
            base.Restart();

            foreach (var child in m_Children)
            {
                child.Restart();
            }

            m_CurrentChildIndex = 0;
        }

        protected override void DoTick()
        {
            ProcessConditionalAbort();

            ProcessChildrenTick();
        }

        public override void Reevaluate()
        {
            if (ExecutionStatus == BehaviorTreeStatus.Running &&
                (m_AbortType == AbortType.Self || m_AbortType == AbortType.Both))
            {                
                ReevaluateChildren(0, m_CurrentChildIndex);
            }
            else if (ExecutionStatus != BehaviorTreeStatus.Running &&
                (m_AbortType == AbortType.LowerPriority || m_AbortType == AbortType.Both))
            {
                ReevaluateChildren(0, m_Children.Count);
            }
        }

        public override bool ConditionalAbort()
        {
            if (ExecutionStatus != BehaviorTreeStatus.Running &&
                (m_AbortType == AbortType.LowerPriority || m_AbortType == AbortType.Both))
            {
                for (int index = 0; index < m_Children.Count; index++)
                {
                    Node child = m_Children[index];

                    BehaviorTreeStatus previousStatus = child.ExecutionStatus;

                    child.Reevaluate();
                    if (child.ExecutionStatus != previousStatus)
                    {                        
                        OnConditionalAbort(index);
                        return true;
                    }
                }
            }

            return false;
        }

        public override void OnAborted()
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                m_Children[i].OnAborted();
            }

            base.OnAborted();
        }

        protected virtual void ProcessConditionalAbort()
        {
            if (m_AbortType == AbortType.Self || m_AbortType == AbortType.Both)
            {                
                ReevaluateChildren(0, m_CurrentChildIndex);
            }
            else if (m_AbortType == AbortType.None)
            {
                ConditionalAbortChildren();
            }
        }

        protected virtual void ProcessChildrenTick()
        {
            while (CanExecute())
            {
                BehaviorTreeStatus status = m_Children[m_CurrentChildIndex].Tick();
                if (status == BehaviorTreeStatus.Running)
                {
                    break;
                }
                else
                {
                    OnChildExecuted(status);
                }
            }
        }

        protected virtual void ReevaluateChildren(int start, int end)
        {
            for (int index = 0; index < end; index++)
            {
                Node child = m_Children[index];
                if (child.ExecutionStatus == BehaviorTreeStatus.Success || child.ExecutionStatus == BehaviorTreeStatus.Failure)
                {
                    BehaviorTreeStatus previousStatus = child.ExecutionStatus;

                    child.Reevaluate();
                    if (child.ExecutionStatus != previousStatus)
                    {
                        OnConditionalAbort(index);
                        break;
                    }
                }
            }
        }


        protected virtual void ConditionalAbortChildren()
        {
            for (int index = 0; index < m_CurrentChildIndex; index++)
            {
                Node child = m_Children[index];
                if (child.ConditionalAbort())
                {
                    OnConditionalAbort(index);
                    break;
                }
            }
        }

        protected virtual void OnConditionalAbort(int index)
        {
            int nextChildIndex = index + 1;
            if (nextChildIndex < m_Children.Count)
            {
                m_Children[nextChildIndex].OnAborted();
            }

            m_CurrentChildIndex = index;
            OnChildExecuted(m_Children[m_CurrentChildIndex].ExecutionStatus);
            if (ExecutionStatus == BehaviorTreeStatus.Running && nextChildIndex < m_Children.Count)
            {
                RestartChildren(nextChildIndex);
            }
        }

        protected void RestartChildren(int start)
        {
            for (int i = start; i < m_Children.Count; i++)
            {
                m_Children[i].Restart();
            }
        }

        public AbortType AbortType
        {
            get { return m_AbortType; }
        }

        protected virtual void OnChildExecuted(BehaviorTreeStatus status) { }

        /// <summary>
        /// List of child nodes.
        /// </summary>
        [SerializeField]
        protected List<Node> m_Children;

        protected int m_CurrentChildIndex;
        [SerializeField]
        protected AbortType m_AbortType;
    }
}
