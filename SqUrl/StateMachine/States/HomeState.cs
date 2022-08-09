using SqUrl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using static SqUrl.Util.Enums;
using Microsoft.Extensions.Logging;
using SqUrl.Exceptions;

namespace SqUrl.StateMachine.States
{
    internal class HomeState : BaseState
    {
        private bool _shouldClearData;
        public HomeState(ILogger logger) : base(logger)
        {
            Initialize(true);
        }

        public HomeState(ILogger logger, bool clearData) : base(logger)
        {
            _shouldClearData = clearData;
            Initialize(true);
        }

        public override void Initialize(bool showDisplay = true)
        {
            StateName = ApplicationStateName.Home.ToString();
            if (showDisplay)
                ShowDisplay();
            Console.WriteLine("Press [S] to shorten a normal URL, [U] to unshorten a previously-shortened URL, or [Q] to quit: ");
            _keyPressed = Console.ReadKey();
            var nextState = GetStateFromKey(_keyPressed);
            TransitionTo(nextState);
        }

        public override void ShowDisplay()
        {
            DisplayText = Resources.SqUrlAsciiArt;
            Console.WriteLine(DisplayText);
            Console.WriteLine(Resources.SqUrlTitle);
            Console.WriteLine("");
        }

        public override ApplicationStateName GetStateFromKey(ConsoleKeyInfo keyInfo)
        {
            try
            {
                return keyInfo.Key switch
                {
                    ConsoleKey.S => ApplicationStateName.Shortening,
                    ConsoleKey.U => ApplicationStateName.Unshortening,
                    ConsoleKey.Q => ApplicationStateName.Quitting,
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

        public override IApplicationState TransitionTo(ApplicationStateName nextState)
        {
            try
            {
                return nextState switch
                {
                    ApplicationStateName.Quitting => new QuitState(),
                    ApplicationStateName.Shortening => new ShortenState(_logger),
                    ApplicationStateName.Unshortening => new UnshortenState(_logger),
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
