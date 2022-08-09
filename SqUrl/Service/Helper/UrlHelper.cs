using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqUrl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqUrl.Util.Constants;
using static SqUrl.Util.Extensions;

namespace SqUrl.Service.Helper
{
    internal class UrlHelper
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly Dictionary<string, UrlStatWrapper> _shortToLongUrls = new();
        private readonly Dictionary<string, string> _longToShortUrls = new();
        private static UrlHelper _instance { get; set; }

        public static UrlHelper GetInstance(IConfiguration config, ILogger logger)
        {
            _instance ??= new UrlHelper(config, logger);
            return _instance;
        }

        public UrlHelper(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Provides the option for users to create their own shortened/vanity URLs.
        /// </summary>
        /// <returns>true if available, false if taken</returns>
        public bool TryShortenManually(string originalUrl, string shortUrl)
        {
            if (_shortToLongUrls.ContainsKey(shortUrl)) // This one's already taken, sorry
            {
                _logger.LogWarning(LogMessages.UserShortUrlAlreadyExistsMessage.Format(shortUrl));
                return false;
            }
            AddEntry(originalUrl, shortUrl);
            return true;
        }

        public string Shorten(string originalUrl)
        {
            // First check if we already have it:
            if (_longToShortUrls.TryGetValue(originalUrl, out var shortUrl))
                return shortUrl;

            shortUrl = GenerateShortUrl();
            AddEntry(originalUrl, shortUrl);
            return shortUrl;
        }

        /// <summary>
        /// Retrieves the original full URL mapped to the given short URL.
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <param name="requestCount">Out parameter - stores the number of times this URL has been requested.</param>
        /// <returns></returns>
        public string Unshorten(string shortUrl, out long requestCount)
        {
            requestCount = 0;
            if (_shortToLongUrls.TryGetValue(shortUrl, out var origUrlWrapper))
            {
                _shortToLongUrls[shortUrl].Increment();
                requestCount = origUrlWrapper.RequestCount;
                return origUrlWrapper.Url;
            }
            _logger.LogWarning(LogMessages.OriginalUrlDoesntExistMessage.Format(shortUrl));
            return null;
        }

        public void RemoveEntryByShortUrl(string shortUrl)
        {
            if (_shortToLongUrls.TryGetValue(shortUrl, out UrlStatWrapper origWrapper))
                _longToShortUrls.Remove(origWrapper.Url);

            _shortToLongUrls.Remove(shortUrl);
        }

        public void RemoveEntryByOriginalUrl(string originalUrl)
        {
            if (_longToShortUrls.TryGetValue(originalUrl, out string shortUrl))
                _shortToLongUrls.Remove(shortUrl);

            _longToShortUrls.Remove(originalUrl);
        }

        private void AddEntry(string originalUrl, string shortUrl)
        {
            _longToShortUrls[originalUrl] = shortUrl;
            _shortToLongUrls[shortUrl] = new UrlStatWrapper(originalUrl);
        }

        private static string GenerateShortUrl()
        {
            var seed = new Random(Guid.NewGuid().GetHashCode()).Next(0, int.MaxValue);
            var bytes = BitConverter.GetBytes(seed);
            return WebEncoders.Base64UrlEncode(bytes);
        }
    }
}
