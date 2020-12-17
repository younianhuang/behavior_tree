using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    /// <summary>
    /// The abstract class for nodes which have child nodes.
    /// </summary>
    public abstract class ParentNode : Node
    {
        public ParentNode() : base()
        {

        }

        public ParentNode(string name) : base(name)
        {
            m_Name = name;
        }

        public ParentNode(ParentNode other) : base(other)
        {            
        }

        /// <summary>
        /// Add a child to this node.
        /// </summary>
        public abstract void AddChild(Node child);
    }   
}
