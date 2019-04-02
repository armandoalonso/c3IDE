using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Utilities.Helpers;

namespace c3IDE.Managers
{
    public static class NodeManager
    {
        public static string GetNodeVersion()
        {
            var outputInfo = ProcessHelper.Insatnce.ExecuteProcess("node -v");
            var match = Regex.Match(outputInfo, @"(?<version>v\d+[.]\d+[.]\d+)");
            var version = match.Groups["version"].ToString();
            return version;
        }
    }
}
