using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace c3IDE.Framework
{
    public class TextCompiler : Singleton<TextCompiler>
    {
        public string CompileTemplates(string templates, Dictionary<string, string> variables)
        {
            var regex = new Regex("<@(?<variable>.+?)@>");
            string result = regex.Replace(templates, m =>
            {
                string name = m.Groups["variable"].Value;

                if (variables.TryGetValue(name.ToLower().Trim(), out var value))
                {
                    return value;
                }

                return string.Empty;
            });

            return result;
        }
    }
}
