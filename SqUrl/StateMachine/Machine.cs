using Microsoft.Extensions.Logging;
using SqUrl.StateMachine.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqUrl.Util.Enums;

namespace SqUrl.StateMachine
{
    internal class Machine
    {
        private ILogger _logger;
        public Machine(ILogger logger)
        {
            _logger = logger;
            ReturnHome();
        }
        
        public ApplicationStateName CurrentStateName { get; private set; }
        public IApplicationState CurrentState { get; private set; }
        public void Transition(IApplicationState nextState)
        {
            CurrentState.TransitionTo(Enum.TryParse<ApplicationStateName>(nextState.StateName, out var state) ? state : ApplicationStateName.Home);
        }
        public void ReturnHome()
        {
            CurrentState = new HomeState(_logger);
            CurrentStateName = ApplicationStateName.Home;
        }
    }
}
