using SqUrl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqUrl.Service
{
    public interface IUrlShortener
    {
        SmallUrl Shorten(Uri originalUri);
        SmallUrl Shorten(string originalUrl);
        SmallUrl Unshorten(SmallUrl smallUrl, out long requestCount);
        SmallUrl Unshorten(string smallUrl, out long requestCount);
        bool TryCreateManual(Uri originalUri, string userCreatedUrl);
        bool TryCreateManual(string originalUrl, string userCreatedUrl);
        void DeleteByShortUrl(string shortenedUrl);
        void DeleteByOriginalUrl(string originalUrl);
    }
}
