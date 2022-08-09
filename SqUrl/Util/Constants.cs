using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqUrl.Util
{
    public static class Constants
    {
        public static class LogMessages
        {
            public const string NullValueMessage = "Variable '{0}' of type '{1}' cannot be null or default.";
            public const string InvalidTransitionKeyMessage = "Key '{0}' does not map to a valid transition state for state '{1}'.  Please try again.";
            public const string InvalidTransitionMessage = "Cannot transition from state '{0}' to state '{1}'.  Please try again.";
            public const string InvalidConfigurationKeyMessage = "Could not retrieve configuration setting with key '{0}'.";
            public const string UserShortUrlAlreadyExistsMessage = "The short URL '{0}' has been taken already. Please try again.";
            public const string OriginalUrlDoesntExistMessage = "No full URL for short URL '{0}' exists. Please try again.";
            public const string OriginalUrlInvalidMessage = "Input '{0}' is not a valid URL string. Please try again.";
            public const string ShortUrlCreationFailureMessage = "Failed to create short URL for '{0}'. Please try again.";
            public const string UrlUnshorteningFailureMessage = "Failed to retrieve unshortened (original) URL for '{0}'. Please try again.";
        }
    }
}
