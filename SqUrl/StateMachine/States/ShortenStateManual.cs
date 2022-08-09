using Microsoft.Extensions.Logging;
using SqUrl.Exceptions;
using SqUrl.Service;
using SqUrl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqUrl.Util.Enums;

namespace SqUrl.StateMachine.States
{
    internal class ShortenStateManual : BaseState
    {
        public ShortenStateManual(ILogger logger) : base(logger)
        {
            Initialize(true);
        }

        public override void Initialize(bool showDisplay = true)
        {
            StateName = ApplicationStateName.ShorteningManually.ToString();
            Console.Write("Enter the original URL: ");
            var origUrl = Console.ReadLine();
            Console.Write("Enter your custom short URL: ");
            var customUrl = Console.ReadLine();
            var result = _shortener.TryCreateManual(origUrl, customUrl);
            if (result)
            {
                Console.WriteLine($"Your custom URL '{customUrl}' for original URL '{origUrl}' was successfully created!");
                Console.WriteLine("Press [H] to return home, [Q] to quit: ");
            }
            else
            {
                Console.WriteLine($"Custom URL '{customUrl}' already exists!");
                Console.WriteLine("Press [R] to retry, [H] to return home, or [Q] to quit: ");
            }
            _keyPressed = Console.ReadKey();
            var nextState = GetStateFromKey(_keyPressed);
            TransitionTo(nextState);
        }

        public override ApplicationStateName GetStateFromKey(ConsoleKeyInfo keyInfo)
        {
            try
            {
                return keyInfo.Key switch
                {
                    ConsoleKey.R => ApplicationStateName.ShorteningManually,
                    ConsoleKey.Q => ApplicationStateName.Quitting,
                    ConsoleKey.H => ApplicationStateName.Home,
                    _ => throw new InvalidTransitionException(Constants.LogMessages.InvalidTransitionKeyMessage.Format(keyInfo.Key, StateName))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Initialize(false);
                return ApplicationStateName.Home;
            }
        }

        public override void ShowDisplay()
        {
            throw new NotImplementedException();
        }

        public override IApplicationState TransitionTo(ApplicationStateName nextState)
        {
            try
            {
                return nextState switch
                {
                    ApplicationStateName.Quitting => new QuitState(),
                    ApplicationStateName.ShorteningManually => new ShortenStateManual(_logger),
                    ApplicationStateName.Home => new HomeState(_logger),
                    _ => throw new InvalidTransitionException(Constants.LogMessages.InvalidTransitionMessage.Format(StateName, nextState))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Initialize(false);
                return this;
            }
        }
    }
}
