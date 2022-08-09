using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqUrl.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights; //Not using for this demo, just added for extensibility

namespace SqUrl
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // In a live app, I would add ApplicationInsights to the service collection, i.e., `services.AddApplicationInsightsTelemetry()`:
            services.AddLogging();
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IUrlShortener, UrlShortener>();
        }
    }
}
