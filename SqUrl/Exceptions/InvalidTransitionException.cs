using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqUrl.Exceptions
{
    internal class InvalidTransitionException : Exception
    {
        public InvalidTransitionException(string message) : base(message)
        {
        }

        public InvalidTransitionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
