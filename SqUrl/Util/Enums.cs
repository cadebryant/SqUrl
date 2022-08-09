using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqUrl.Util
{
    public static class Enums
    {
        public enum ApplicationStateName
        {
            Home,
            Shortening,
            ShorteningManually,
            Unshortening,
            Quitting
        }

        public enum ResourceType
        {
            Strings
        }

        public enum ResourceName
        {
            SqUrlAsciiArt
        }
    }
}
