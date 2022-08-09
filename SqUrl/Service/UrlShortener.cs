using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqUrl.Model;
using SqUrl.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SqUrl.Util.Constants;
using static SqUrl.Util.Extensions;

namespace SqUrl.Service
{
    public class UrlShortener : IUrlShortener
    {
        private ILogger _logger;
        private IConfiguration _config;
        private UrlHelper _helper;
        private UriCreationOptions _options = new() { DangerousDisablePathAndQueryCanonicalization = false };

        public UrlShortener(ILogger logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _helper = UrlHelper.GetInstance(_config, _logger);
        }

        public SmallUrl Shorten(string originalUrl)
        {
            if (!Uri.TryCreate(originalUrl, _options, out Uri uri))
            {
                _logger.LogError(LogMessages.OriginalUrlInvalidMessage.Format(originalUrl));
                return SmallUrl.Empty;
            }
            return Shorten(uri);
        }
        
        public SmallUrl Shorten(Uri originalUri)
        {
            var result = _helper.Shorten(originalUri.OriginalString);
            if (result == null) return SmallUrl.Empty;
            return new SmallUrl(result, originalUri.OriginalString);
        }

        public bool TryCreateManual(string originalUrl, string userCreatedUrl)
        {
            return _helper.TryShortenManually(originalUrl, userCreatedUrl);
        }

        public bool TryCreateManual(Uri originalUri, string userCreatedUrl)
        {
            return TryCreateManual(originalUri.OriginalString, userCreatedUrl);
        }

        public SmallUrl Unshorten(SmallUrl smallUrl, out long requestCount)
        {
            return Unshorten(smallUrl.ShortenedUrl, out requestCount);
        }

        public SmallUrl Unshorten(string shortenedUrl, out long requestCount)
        {
            var result = _helper.Unshorten(shortenedUrl, out requestCount);
            if (result == null) return SmallUrl.Empty;
            return new SmallUrl(shortenedUrl, result);
        }

        public void DeleteByShortUrl(string shortenedUrl)
        {
            _helper.RemoveEntryByShortUrl(shortenedUrl);
        }

        public void DeleteByOriginalUrl(string originalUrl)
        {
            _helper.RemoveEntryByOriginalUrl(originalUrl);
        }
    }
}
