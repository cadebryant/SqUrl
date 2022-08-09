using Microsoft.Extensions.Logging;
using SqUrl.Exceptions;
using SqUrl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqUrl.Util.Enums;
using static SqUrl.Util.Constants;
using static SqUrl.Util.Extensions;

namespace SqUrl.StateMachine.States
{
    internal class ShortenState : BaseState
    {
        public ShortenState(ILogger logger) : base(logger)
        {
            Initialize(true);
        }

        public override void Initialize(bool showDisplay = true)
        {
            StateName = ApplicationStateName.Shortening.ToString();
            Console.WriteLine("Press [A] to auto-generate a shortened URL, [M] to enter  one manually, [H] to return home, or [Q] to quit: ");
            _keyPressed = Console.ReadKey();
            if (_keyPressed.Key == ConsoleKey.A)
            {
                Console.Write("Enter the original URL: ");
                var origUrl = Console.ReadLine();
                var result = _shortener.Shorten(origUrl);
                if (result.ShortenedUrl == null)
                {
                    _logger.LogError(LogMessages.ShortUrlCreationFailureMessage.Format(origUrl));
                    Console.WriteLine("Press [R] to retry, [H] to return home or [Q] to quit: ");
                }
                else
                {
                    Console.WriteLine($"Your auto-generated URL '{result.ShortenedUrl}' for original URL '{origUrl}' was successfully created!");
                    Console.WriteLine("Press [H] to return home, or [Q] to quit: ");
                }
            }
            var nextState = GetStateFromKey(_keyPressed);
            TransitionTo(nextState);
        }

        public override ApplicationStateName GetStateFromKey(ConsoleKeyInfo keyInfo)
        {
            try
            {
                return keyInfo.Key switch
                {
                    ConsoleKey.R => ApplicationStateName.Shortening,
                    ConsoleKey.M => ApplicationStateName.ShorteningManually,
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
                    ApplicationStateName.Shortening => new ShortenState(_logger),
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
