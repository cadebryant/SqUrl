using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqUrl.Model
{
    public class SmallUrl
    {
        public SmallUrl(string shortenedUrl, string originalUrl)
        {
            ShortenedUrl = shortenedUrl;
            OriginalUrl = originalUrl;
        }

        public static SmallUrl Empty => new(null, null);

        public string ShortenedUrl { get; set; }
        public string OriginalUrl { get; set; }
    }
}
