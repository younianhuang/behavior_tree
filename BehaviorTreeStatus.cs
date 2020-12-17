using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    /// <summary>
    /// The type of execution status when invoking behaviour tree nodes.
    /// </summary>
    public enum BehaviorTreeStatus
    {
        Inactive,
        Success,
        Failure,
        Running
    }
}
