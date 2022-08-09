using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static SqUrl.Util.Constants;
using SqUrl.Exceptions;

namespace SqUrl.Util
{
    internal class ConfigUtil
    {
        IConfiguration _config;
        ILogger _logger;
        public ConfigUtil(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }
        public T GetValue<T>(Expression<Func<ConfigKeys, T>> selector)
        {
            string memberName = "";
            try
            {
                var memberExp = selector.Body as MemberExpression;
                if (memberExp?.Member != null) return default;
                memberName = memberExp.Member.Name;
                return _config.GetValue<T>(memberName);
            }
            catch (Exception ex)
            {
                var configEx = new ConfigurationKeyException(LogMessages.InvalidConfigurationKeyMessage.Format(memberName), ex);
                _logger.LogError(configEx, configEx.Message);
                return default;
            }
        }

        internal class ConfigKeys
        {
            public int AsciiStartIndex;
            public int AsciiEndIndex;
        }
    }

}
