using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SqUrl.Service;
using System.Reflection;

namespace SqUrl.UnitTests
{
    [TestClass]
    public class ShortUrlTests
    {
        private ILogger _logger;
        private IUrlShortener _shortener;
        private IConfiguration _config;

        [TestInitialize]
        public void Initialize()
        {
            _logger = NullLogger.Instance;
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json")
                .Build();
            _config = configuration;
            _shortener = new UrlShortener(_logger, _config);

        }

        [TestMethod]
        public void TestShortenAndRetrieveUrl()
        {
            long reqCount = 0;
            var urls = new[] { "https://www.google.com", "https://www.microsoft.com", "https://www.apple.com" };
            var count = 10;
            for (var i = 0; i < count; i++)
            {
                foreach (var url in urls)
                {
                    var shortUrl = _shortener.Shorten(url);
                    Assert.IsNotNull(shortUrl.ShortenedUrl);

                    var origUrl = _shortener.Unshorten(shortUrl, out reqCount);
                    Assert.IsNotNull(origUrl.OriginalUrl);
                    Assert.AreEqual(reqCount, i + 1);
                }
            }
        }

        [TestMethod]
        public void TestCreateDeleteManualShortUrl()
        {
            var randStr = Guid.NewGuid().ToString().Replace("-", "");
            var origUrl = $"https://www.{randStr}.com";
            var userUrl = $"this_is_my_custom_url_{randStr[0..10]}";
            var result = _shortener.TryCreateManual(origUrl, userUrl);
            Assert.IsTrue(result);
            // This time it sould return false because of the identical record we just created:
            result = _shortener.TryCreateManual(origUrl, userUrl);
            Assert.IsFalse(result);
            // clean up:
            _shortener.DeleteByShortUrl(userUrl);
            var retrieveResult = _shortener.Unshorten(userUrl, out long reqCount);
            Assert.AreEqual(reqCount, 0);
            Assert.IsNull(retrieveResult.OriginalUrl);
        }
    }
}