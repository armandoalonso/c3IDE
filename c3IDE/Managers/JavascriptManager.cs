using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using c3IDE.Utilities.Helpers;
using Newtonsoft.Json;

namespace c3IDE.Managers
{
    public static class JavascriptManager 
    {
        public static Dictionary<string, string> GetAllFunction(string source)
        {
            var formatted = FormatHelper.Insatnce.Javascript(source);
            var lines = formatted.Split('\n');
            var state = ParseState.Idle;
            var tmp = new StringBuilder();
            int open = 0, close = 0;
            var funcList = new Dictionary<string, string>();
            string name = String.Empty;
            int count = 0;

            foreach (var line in lines)
            {
                count++;
                //find function match
                var funcMatch = Regex.Match(line, @"(\w|\d)*\s?(\w|\d)*\s?(\(\)|\((\w|\d|,|\s)+\))", RegexOptions.ECMAScript);
                var hasFunction = funcMatch.Success;

                //if it is start of function
                if (hasFunction && state == ParseState.Idle)
                {
                    state = ParseState.InFunction;

                    name = Regex.Replace(line, @"\(.*\)", string.Empty).Trim();
                    name = Regex.Replace(name, @"(//.*|/[*].*)", string.Empty).Trim();
                    name = Regex.Replace(name, @":\s?function", string.Empty).Trim();
                    name = Regex.Replace(name, @"\s?async\s?", string.Empty).Trim();

                    tmp.AppendLine(Regex.Replace(line, @":\s?function", string.Empty));
                    continue;
                }

                if (state == ParseState.InFunction)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    open = open + line.Count(x => x.Equals('{'));
                    close = close + line.Count(x => x.Equals('}'));
                    tmp.AppendLine(line);

                    if (open == close)
                    {
                        state = ParseState.Idle;
                        if (funcList.ContainsKey(name))
                        {
                            var x = 1;
                        }

                        while (funcList.ContainsKey(name))
                        {
                            //todo: add some logging or report about import
                            continue;
                        }

                        funcList.Add(name, tmp.ToString().Trim().TrimEnd(','));
                        tmp = new StringBuilder();
                    }
                }
            }

            return funcList;
        }

        public enum ParseState
        {
            Idle,
            InFunction
        }
    }
}
