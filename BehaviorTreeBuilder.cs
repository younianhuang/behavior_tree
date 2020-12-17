using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    /// <summary>
    /// Builder for building a behaviour tree.
    /// </summary>
    public class BehaviorTreeBuilder
    {
        public BehaviorTreeBuilder()
        {
            m_ParentNodeStack = new Stack<ParentNode>();

            m_CurrentParentNode = null;

            m_BehaviorTree = null;

            m_CurrentChildNode = null;            
        }

        public BehaviorTreeBuilder Tree(string name)
        {
            m_BehaviorTree = new Tree(name);
            return this;
        }

        /// <summary>
        /// Create an action node.
        /// </summary>
        public BehaviorTreeBuilder Do(string name, Func<BehaviorTreeStatus> fn)
        {
            var node = new ActionNode(name, fn);

            AddChildNode(node);
            
            return this;
        }

        /// <summary>
        /// Create an condition node.
        /// </summary>
        public BehaviorTreeBuilder Condition(string name, Func<bool> fn)
        {
            var node = new ConditionNode(name, fn);

            AddChildNode(node);

            return this;
        }

        /// <summary>
        /// Create an Inverter node that inverts the success/failure of its children.
        /// </summary>
        public BehaviorTreeBuilder Inverter(string name)
        {            
            AddParentNode(new InverterNode(name));
            return this;
        }

        /// <summary>
        /// Create a Repeater node that.
        /// </summary>
        public BehaviorTreeBuilder Repeater(string name, int repeatTimes = RepeaterNode.RepeatForever, bool endOnFailure = false)
        {
            AddParentNode(new RepeaterNode(name, repeatTimes, endOnFailure));
            return this;
        }

        /// <summary>
        /// Create an UntilFailure node.
        /// </summary>
        public BehaviorTreeBuilder UntilFailure(string name)
        {
            AddParentNode(new UntilFailureNode(name));
            return this;
        }

        /// <summary>
        /// Create an UntilSuccess node.
        /// </summary>
        public BehaviorTreeBuilder UntilSuccess(string name)
        {
            AddParentNode(new UntilSuccessNode(name));
            return this;
        }

        /// <summary>
        /// Create an UntilFailure node.
        /// </summary>
        public BehaviorTreeBuilder ReturnFailure(string name)
        {
            AddParentNode(new ReturnFailureNode(name));
            return this;
        }

        /// <summary>
        /// Create an UntilSuccess node.
        /// </summary>
        public BehaviorTreeBuilder ReturnSuccess(string name)
        {
            AddParentNode(new ReturnSuccessNode(name));
            return this;
        }

        /// <summary>
        /// Create a sequence node.
        /// </summary>
        public BehaviorTreeBuilder Sequence(string name)
        {
            AddParentNode(new SequenceNode(name));
            return this;
        }

        /// <summary>
        /// Create a parallel node.
        /// </summary>
        public BehaviorTreeBuilder Parallel(string name)
        {            
            AddParentNode(new ParallelNode(name));
            return this;
        }

        /// <summary>
        /// Create a selector node.
        /// </summary>
        public BehaviorTreeBuilder Selector(string name)
        {
            AddParentNode(new SelectorNode(name));
            return this;
        }

        /// <summary>
        /// Splice a sub tree into the parent tree.
        /// </summary>
        public BehaviorTreeBuilder Splice(Node subTree)
        {
            if (subTree == null)
            {
                throw new ArgumentNullException("Argument subTree is null");
            }

            AddChildNode(subTree);
            return this;
        }

        public BehaviorTreeBuilder ParentNode(ParentNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("Argument node is null");
            }
            AddParentNode(node);
            return this;
        }

        /// <summary>
        /// The end of a parent node.
        /// </summary>
        public BehaviorTreeBuilder End()
        {
            m_CurrentParentNode = m_ParentNodeStack.Pop();
            return this;
        }

        /// <summary>
        /// Return the behavior tree just built.
        /// </summary>
        public Tree Build()
        {            
            if (m_BehaviorTree == null)
            {
                throw new InvalidOperationException("Must call Tree() before call Build()!!!");
            }

            Tree ret = null;

            if (m_CurrentChildNode != null)
            {
                m_BehaviorTree.SetRootNode(m_CurrentChildNode);
                m_CurrentChildNode = null;
            }
            else
            {
                if (m_CurrentParentNode == null || m_ParentNodeStack.Count > 0)
                {
                    throw new InvalidOperationException("End() operation dose not match parent node creation");
                }

                m_BehaviorTree.SetRootNode(m_CurrentParentNode);
                m_CurrentParentNode = null;
            }                                    

            ret = m_BehaviorTree;
            m_BehaviorTree = null;
            return ret;
        }

        /// <summary>
        /// Return the behavior tree just built.
        /// </summary>
        public Node BuildSubTree()
        {
            if (m_BehaviorTree != null)
            {
                throw new InvalidOperationException("Should not call Tree() before call BuildSubTree()!!!");
            }

            Node ret = null;

            if (m_CurrentChildNode != null)
            {
                ret = m_CurrentChildNode;
                m_CurrentChildNode = null;
            }
            else
            {
                if (m_CurrentParentNode == null || m_ParentNodeStack.Count > 0)
                {
                    throw new InvalidOperationException("End() operation dose not match parent node creation");
                }

                ret = m_CurrentParentNode;                
                m_CurrentParentNode = null;
            }
            
            return ret;
        }

        private void AddParentNode(ParentNode node)
        {            
            if (m_CurrentChildNode != null)
            {
                throw new InvalidOperationException("Cannot add parent node " + node.Name + " to child node " + m_CurrentChildNode.Name);
            }

            if (m_ParentNodeStack.Count > 0)
            {
                m_ParentNodeStack.Peek().AddChild(node);
            }           

            m_ParentNodeStack.Push(node);
        }

        private void AddChildNode(Node node)
        {
            if (m_CurrentChildNode != null)
            {
                throw new InvalidOperationException("Cannot add child node " + node.Name + " to child node " + m_CurrentChildNode.Name);
            }

            if (m_ParentNodeStack.Count > 0)
            {
                m_ParentNodeStack.Peek().AddChild(node);
            }
            else
            {                
                m_CurrentChildNode = node;
            }
        }
                
        private Tree m_BehaviorTree;

        private Stack<ParentNode> m_ParentNodeStack;

        private Node m_CurrentParentNode;

        private Node m_CurrentChildNode;
    }    
}
