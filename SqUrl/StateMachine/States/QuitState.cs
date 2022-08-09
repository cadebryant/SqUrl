using Microsoft.Extensions.Logging;
using SqUrl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqUrl.Util.Enums;

namespace SqUrl.StateMachine.States
{
    internal class QuitState : IApplicationState
    {
        public QuitState()
        {
            Console.WriteLine(DisplayText);
            Environment.Exit(0);
        }
        
        public string DisplayText => "Quitting...";

        public string StateName => ApplicationStateName.Quitting.ToString();

        public Enums.ApplicationStateName GetStateFromKey(ConsoleKeyInfo keyInfo)
        {
            throw new NotImplementedException();
        }

        public IApplicationState TransitionTo(ApplicationStateName nextState)
        {
            throw new NotImplementedException();
        }
    }
}
