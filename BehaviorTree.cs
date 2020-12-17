using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// The entry class for behaviour tree.
    /// </summary>
    public class Tree : ISharedVariableContainer
    {
        public Tree()
        {

        }


        public Tree(string name, Node root = null)        {
            m_Name = name;
            m_RootNode = root;
            m_SharedVariables = new Dictionary<string, SharedVariable>();
            m_RestartWhenComplete = false;
        }

        public Tree(Tree other)
        {
            m_Name = other.m_Name;
                    
            m_SharedVariables = new Dictionary<string, SharedVariable>();

            foreach (var pair in other.m_SharedVariables)
            {
                m_SharedVariables.Add(pair.Key, pair.Value.Clone());
            }

            m_RestartWhenComplete = other.m_RestartWhenComplete;
        }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        public BehaviorTreeStatus Tick()
        {
            if (m_RestartWhenComplete &&
                (m_RootNode.ExecutionStatus == BehaviorTreeStatus.Success || m_RootNode.ExecutionStatus == BehaviorTreeStatus.Failure))
            {
                m_RootNode.Restart();
            }

            return m_RootNode.Tick();
        }

        public void SetExternalSource(object agent, object world)
        {
            if (m_RootNode == null)
            {
                throw new InvalidOperationException("Root Node is empty!!!");
            }

            m_RootNode.SetExternalSource(agent, world);
        }

        /// <summary>
        /// Awake is used to initialize any variables or state before this tree start. 
        /// Awake is called only once during the lifetime of this tree.
        /// </summary>
        public virtual void Awake()
        {
            m_RootNode.Awake();
        }

        public virtual Tree Clone(object agent, object world)
        {
            var tree = new Tree(this);       

            //m_SharedVariables["Self"].SetValue(agent);
            
            tree.m_RootNode = m_RootNode.Clone(agent, world, tree);
                                   
            return tree;
        }

        public virtual Tree Clone()
        {
            var tree = new Tree(this);
            
            tree.m_RootNode = m_RootNode.Clone(tree);

            return tree;
        }

        /// <summary>
        /// Restart this behavior tree. This make this tree can be reevaluted.
        /// </summary>
        public void Restart()
        {
            m_RootNode.Restart();
        }

        /// <summary>
        /// The execution status of this tree.
        /// </summary>
        public BehaviorTreeStatus ExecutionStatus
        {
            get => m_RootNode.ExecutionStatus;
        }

        public void SetRootNode(Node root)
        {
            m_RootNode = root;
        }

        public Node GetRootNode()
        {
            return m_RootNode;
        }

        public void SetVariable(SharedVariable variable)
        {
            string name = variable.Name;

            if (m_SharedVariables == null)
            {
                m_SharedVariables = new Dictionary<string, SharedVariable>();
            }

            if (m_SharedVariables.ContainsKey(name))
            {
                m_SharedVariables[name] = variable;
            }
            else
            {
                m_SharedVariables.Add(name, variable);
            }
        }

        public void SetVariable(string name, SharedVariable variable)
        {
            variable.Name = name;

            if (m_SharedVariables.ContainsKey(name))
            {
                m_SharedVariables[name] = variable;
            }
            else
            {
                m_SharedVariables.Add(name, variable);
            }
        }

        public SharedVariable GetVariable(string name)
        {
            if (m_SharedVariables.ContainsKey(name))
            {
                return m_SharedVariables[name];
            }

            throw new ArgumentException("Variable " + name + "  dos't exist!");
        }

        public bool HasVariable(string name)
        {
            return m_SharedVariables.ContainsKey(name);
        }

        public bool RestartWhenComplete
        {
            get=> m_RestartWhenComplete;
            set => m_RestartWhenComplete = value;
        }

        [SerializeField]
        private string m_Name;

        [SerializeField]
        private Node m_RootNode;

        private Dictionary<string, SharedVariable> m_SharedVariables;

        private bool m_RestartWhenComplete;
    }
}
