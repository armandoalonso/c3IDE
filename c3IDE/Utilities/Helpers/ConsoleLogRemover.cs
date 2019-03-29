using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Managers;

namespace c3IDE.Utilities.Helpers
{
    public class ConsoleLogRemover : Singleton<ConsoleLogRemover>
    {
        private readonly Regex _regex = new Regex(@"(?<statement>console\.log\(.+\);)");

        //todo: does not remove from 3rd party files
        public string CommentOut(string script)
        {
            if (!OptionsManager.CurrentOptions.RemoveConsoleLogsOnCompile) return script;

            var output = _regex.Replace(script, m =>
            {
                var line = m.Groups["statement"].Value;
                return $"//{line}\n";
            });

            return output;
        }
    }
}
