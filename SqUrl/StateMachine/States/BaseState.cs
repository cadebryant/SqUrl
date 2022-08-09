using Microsoft.Extensions.Logging;
using SqUrl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using static SqUrl.Util.Enums;
using SqUrl.Service;

namespace SqUrl.StateMachine.States
{
    internal abstract class BaseState : IApplicationState
    {
        protected ResourceManager _resourceManager;
        protected ILogger _logger;
        protected ConsoleKeyInfo _keyPressed;
        protected IUrlShortener _shortener;

        public BaseState(ILogger logger)
        {
            _resourceManager = new ResourceManager(Enums.ResourceType.Strings.ToString(), typeof(SqUrl.Program).Assembly);
            _logger = logger;
            _shortener = new UrlShortener(logger, null);
        }
        public string DisplayText { get; protected set; }
        public string StateName { get; protected set; }

        public abstract IApplicationState TransitionTo(ApplicationStateName nextState);

        public abstract ApplicationStateName GetStateFromKey(ConsoleKeyInfo keyInfo);

        public abstract void ShowDisplay();

        public abstract void Initialize(bool showDisplay = true);
    }
}
