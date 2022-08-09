using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SqUrl.Exceptions;
using SqUrl.Service;
using SqUrl.StateMachine;
using System;
using static SqUrl.Util.Constants;
using static SqUrl.Util.Extensions;

namespace SqUrl
{
    public class Program
    {
        private static IConfiguration _config;
        private static IServiceCollection _services;
        private static Startup _startup;
        private static IServiceProvider _serviceProvider;
        private static ILogger _logger;
        private static Machine _stateMachine;

        public static void Main(string[] args)
        {
            Initialize();
        }

        private static void Initialize()
        {
            try
            {
                using (var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole()))
                {
                    _logger = loggerFactory.CreateLogger<Startup>();
                }
                _services = new ServiceCollection();
                _startup = new Startup();
                _startup.ConfigureServices(_services);
                _serviceProvider = _services.BuildServiceProvider();
                if (_serviceProvider == null)
                {
                    throw new InitializationException(
                       LogMessages.NullValueMessage.Format(nameof(_serviceProvider), typeof(IServiceProvider).Name));
                }
                _config = _serviceProvider.GetService<IConfiguration>();
                _stateMachine = new Machine(_logger);
                _stateMachine.ReturnHome();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Environment.Exit(1);
            }
        }
    }
}