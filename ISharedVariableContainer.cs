using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    public interface ISharedVariableContainer
    {
        void SetVariable(string name, SharedVariable variable);

        void SetVariable(SharedVariable variable);

        SharedVariable GetVariable(string name);

        bool HasVariable(string name);
    }
}
