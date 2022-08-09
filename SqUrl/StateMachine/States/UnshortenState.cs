using Microsoft.Extensions.Logging;
using SqUrl.Exceptions;
using SqUrl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqUrl.Util.Constants;
using static SqUrl.Util.Enums;

namespace SqUrl.StateMachine.States
{
    internal class UnshortenState : BaseState
    {
        public UnshortenState(ILogger logger) : base(logger)
        {
            Initialize(true);
        }
        
        public override ApplicationStateName GetStateFromKey(ConsoleKeyInfo keyInfo)
        {
            try
            {
                return keyInfo.Key switch
                {
                    ConsoleKey.R => ApplicationStateName.Unshortening,
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

        public override void Initialize(bool showDisplay = true)
        {
            StateName = ApplicationStateName.Unshortening.ToString();
            Console.Write("Enter your short URL: ");
            var shortUrl = Console.ReadLine();
            var result = _shortener.Unshorten(shortUrl, out long statCount);
            if (result.OriginalUrl == null)
            {
                _logger.LogError(LogMessages.UrlUnshorteningFailureMessage.Format(shortUrl));
                Console.WriteLine("Press [R] to retry, [H] to return home, or [Q] to quit: ");
            }
            else
            {
                Console.WriteLine($"Your original URL '{result.OriginalUrl}' for short URL '{shortUrl}' was successfully returned!");
                Console.WriteLine($"Usage statistics for '{result.OriginalUrl}': Total request count = {statCount}.");
                Console.WriteLine("Press [H] to return home, or [Q] to quit: ");
            }
            _keyPressed = Console.ReadKey();
            var nextState = GetStateFromKey(_keyPressed);
            TransitionTo(nextState);
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
                    ApplicationStateName.Unshortening => new UnshortenState(_logger),
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
