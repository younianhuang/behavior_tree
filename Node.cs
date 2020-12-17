using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{    
    /// <summary>
    /// The abstract class for behaviour tree nodes.
    /// </summary>
    public abstract class Node 
    {
        #region Functions
        public Node()
        {            
            m_ExecutionStatus = BehaviorTreeStatus.Inactive;
        }

        public Node(string name)
        {
            m_Name = name;
            m_ExecutionStatus = BehaviorTreeStatus.Inactive;
        }

        public Node(Node other)
        {
            m_Name = other.m_Name;
            m_ExecutionStatus = BehaviorTreeStatus.Inactive;
        }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        public virtual BehaviorTreeStatus Tick()
        {
            if (m_ExecutionStatus == BehaviorTreeStatus.Running)
            {
                DoTick();
            }
            else if (m_ExecutionStatus == BehaviorTreeStatus.Inactive)
            {
                SetExecutionStatus(BehaviorTreeStatus.Running);

                DoTick();
            }

            return m_ExecutionStatus;
        }

        /// <summary>
        /// Awake is used to initialize any variables or state before this node start. 
        /// Awake is called only once during the lifetime of this node.
        /// </summary>
        public virtual void Awake()
        {

        }

        public virtual void SetExternalSource(object agent, object world)
        {

        }

        public abstract Node Clone(object agent, object world, ISharedVariableContainer sharedVariableTable);

        public abstract Node Clone(ISharedVariableContainer sharedVariableTable);

        public SharedVariable CloneSharedVariable(ISharedVariableContainer sharedVariableTable, SharedVariable variable)
        {
            if (variable != null)
            {
                if (string.IsNullOrEmpty(variable.Name))
                {
                    // the sharedVariable does not exist in table, clone it
                    return variable.Clone();                    
                }
                else if(sharedVariableTable != null)
                {                     
                    // the sharedVariable exist in table, build the reference to it
                    return sharedVariableTable.GetVariable(variable.Name);                    
                }
            }

            return null;
        }

        /// <summary>
        /// Restart this node. This make this node can be reevaluted.
        /// </summary>
        public virtual void Restart()
        {
            SetExecutionStatus(BehaviorTreeStatus.Inactive);           
        }

        /// <summary>
        /// Reevaluate this node. 
        /// </summary>
        public virtual void Reevaluate()
        {

        }

        /// <summary>
        /// Check whether this node can conditional abort or not.         
        /// </summary>
        public virtual bool ConditionalAbort()
        {
            return false;
        }

        public virtual void OnAborted()
        {
            SetExecutionStatus(BehaviorTreeStatus.Failure);
        }

        protected abstract void DoTick();
       
        protected void SetExecutionStatus(BehaviorTreeStatus status)
        { 
            if (m_ExecutionStatus != status)
            {
                m_ExecutionStatus = status;

                if (StatusChangedEvent != null)
                {
                    StatusChangedEvent(m_ExecutionStatus);
                }                
            }
        }

        protected void ReportError(Exception exception)
        {
            if (ErrorEvent != null)
            {
                ErrorEvent(exception);
            }
        }

        #endregion


        #region Properties
        /// <summary>
        /// The execution status of this node.
        /// </summary>
        public BehaviorTreeStatus ExecutionStatus
        {
            get => m_ExecutionStatus;
        }

        public string Name
        {
            set => m_Name = value;
            get => m_Name;
        }
        #endregion

        #region Events
        public event Action<BehaviorTreeStatus> StatusChangedEvent;
        public event Action<Exception> ErrorEvent;
        #endregion

        private BehaviorTreeStatus m_ExecutionStatus= BehaviorTreeStatus.Inactive;

        [SerializeField]
        protected string m_Name;
    }    
}
