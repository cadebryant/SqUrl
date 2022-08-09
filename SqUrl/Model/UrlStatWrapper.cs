using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqUrl.Model
{
    public class UrlStatWrapper
    {
        public UrlStatWrapper(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
        public long RequestCount { get; set; }

        public void Increment()
        {
            RequestCount++;
        }
    }
}
