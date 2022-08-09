using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqUrl.Exceptions
{
    internal class ConfigurationKeyException : Exception
    {
        public ConfigurationKeyException(string message) : base(message)
        {
        }

        public ConfigurationKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
