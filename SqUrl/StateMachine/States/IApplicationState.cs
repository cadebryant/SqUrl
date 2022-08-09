using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqUrl.Util.Enums;

namespace SqUrl.StateMachine.States
{
    internal interface IApplicationState
    {
        string DisplayText { get; }
        string StateName { get; }
        IApplicationState TransitionTo(ApplicationStateName nextState);
        ApplicationStateName GetStateFromKey(ConsoleKeyInfo keyInfo);
    }
}
