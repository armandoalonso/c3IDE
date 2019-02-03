using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.CodeCompletion.Bindings;
using c3IDE.Utilities.Extentions;

namespace c3IDE.Utilities.Helpers
{
    public class JavascriptParser : Singleton<JavascriptParser>
    {
        private readonly Regex _jsonRegex = new Regex("\"(\\S+)\""); 
        private readonly Regex _userRegex = new Regex("(\\w+)");

        public List<string> ParseJsonDocument(string text)
        {
            var matches = _jsonRegex.Matches(text);
            var hashset = new HashSet<string>();

            foreach (Match match in matches)
            {
                hashset.Add(match.Groups[1].ToString()); 
            }

            return hashset.ToList();
        }

        public List<string> ParseJavascriptUserTokens(string text)
        {
            var mathes = _userRegex.Matches(text);
            var hashset = new HashSet<string>();

            foreach (Match match in mathes)
            {
                hashset.Add(match.Groups[1].ToString());
            }

            return hashset.ToList();
        }
    }
}
