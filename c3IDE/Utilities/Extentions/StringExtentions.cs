using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace c3IDE.Utilities.Extentions
{
    public static class StringExtentions
    {
        public static string Base64Encode(this string text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string SplitCamelCase(this string str, string rep = " ")
        {
            if (str == null) return string.Empty;
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    $"$1{rep}$2"
                ),
                @"(\p{Ll})(\P{Ll})",
                $"$1{rep}$2"
            );
        }
    }
}
