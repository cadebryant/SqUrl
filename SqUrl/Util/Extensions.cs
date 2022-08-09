using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqUrl.Util
{
    public static class Extensions
    {
        public static string Format(this string input, params object[] args)
        {
            if (input.IsNullOrWhiteSpace() || args.IsEmpty()) return input;
            return string.Format(input, args);
        }

        public static bool IsEmpty(this IEnumerable<object> collection)
        {
            return collection == null || !collection.Any();
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
    }
}
