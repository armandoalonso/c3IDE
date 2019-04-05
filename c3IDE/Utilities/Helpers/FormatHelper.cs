using System;
using System.Text;
using System.Text.RegularExpressions;
using c3IDE.Utilities.JsBeautifier;

namespace c3IDE.Utilities.Helpers
{
    public class FormatHelper : Singleton<FormatHelper>
    {
        private Beautifier jsBeautifier = new Beautifier();
        private JsonBeautifier jsonBeautifier = new JsonBeautifier();

        public string Json(string json, bool wrap = false)
        {
            if (wrap)
            {
                json = $"{{{json}}}";
                var returnString = jsonBeautifier.GetResults(json).Trim();
                returnString = returnString.Remove(0, 1);
                returnString = returnString.Remove(returnString.Length - 1, 1);
                return returnString.Trim();
            }
            else
            {
                return jsonBeautifier.GetResults(json).Trim();
            }
        }

        public string JsonCondensed(string json)
        {
            return jsonBeautifier.GetResultsCompressed(json).Trim();
        }

        public string JsonCompress(string json, bool wrap = false)
        {
            if (wrap)
            {
                var returnString = $"{{{json}}}";
                returnString = returnString.Remove(0, 1);
                returnString = returnString.Remove(returnString.Length - 1, 1);
                var sb = new StringBuilder(returnString);
                sb.Replace("\t", string.Empty);
                sb.Replace("\n", string.Empty);
                sb.Replace("\r", string.Empty);
                sb.Replace(" ", string.Empty);
                return sb.ToString();
            }
            else
            {
                var sb = new StringBuilder(json);
                sb.Replace("\t", string.Empty);
                sb.Replace("\n", string.Empty);
                sb.Replace("\r", string.Empty);
                sb.Replace(" ", string.Empty);
                return sb.ToString();
            }
        }

        public string Javascript(string js)
        {
            return jsBeautifier.Beautify(js).Trim();
        }

        public string JavascriptCompress(string js)
        {
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";

            string noComments = Regex.Replace(js,
                blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                me => {
                    if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                        return me.Value.StartsWith("//") ? Environment.NewLine : "";
                    // Keep the literal strings
                    return me.Value;
                },
                RegexOptions.Singleline);

            var result = Regex.Replace(noComments, @"\r\n?|\n", string.Empty);
            return Regex.Replace(result, @"\s{2,}", " ");
        }
    }
}
