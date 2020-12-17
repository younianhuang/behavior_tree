using System;

namespace BehaviorTree
{
    public enum AbortType
    {
        None = 0,
        Self = 1,
        LowerPriority = 2,
        Both = 3
    }
}
