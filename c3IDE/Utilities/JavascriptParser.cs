using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.CodeCompletion.Bindings;

namespace c3IDE.Utilities
{
    public class JavascriptParser : Singleton<JavascriptParser>
    {
        private readonly Dictionary<string, List<string>> _editorMethodCache = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<string>> _runtimeMethodcache = new Dictionary<string, List<string>>();

        private readonly Regex _tokenRegex = new Regex("SDK\\.(\\w+)");
        private readonly Regex _methodRegex = new Regex("(\\w+)\\(.*?\\)");

        public List<string> ParseJavascriptDocument(string text)
        {
            var mathes = _tokenRegex.Matches(text);
            var hashset = new HashSet<string>();

            foreach (Match match in mathes)
            {
                //do not include strings 
                if (!match.Value[0].Equals('\"') && !match.Value[0].Equals('\''))
                {
                    hashset.Add(match.Groups[1].ToString());
                }
            }

            return hashset.ToList();
        }

        public List<string> ParseJavascriptMethodCalls(string text)
        {
            var mathes = _methodRegex.Matches(text);
            var hashset = new HashSet<string>();

            foreach (Match match in mathes)
            {
                hashset.Add(match.Groups[1].ToString());
            }

            return hashset.ToList();
        }

        public List<string> DecorateMethodInterfaces(IEnumerable<string> tokens, IEnumerable<string> methods, CodeType type)
        {
            var results = new HashSet<string>(tokens.ToList());

            //populate dictionary
           

            switch (type)
            {
                case CodeType.EdittimeJavascript:
                    if (_editorMethodCache.Count == 0)
                    {
                        var mapping = new MethodInterfaceMapping();
                        foreach (var map in mapping.Interfaces)
                        {
                            if (!_editorMethodCache.ContainsKey(map.Method))
                            {
                                _editorMethodCache.Add(map.Method, map.Interface);
                            }
                        }
                    }

                    foreach (var m in methods)
                    {
                        if (_editorMethodCache.ContainsKey(m))
                        {
                            results.AddRange(_editorMethodCache[m]);
                        }
                    }

                    return results.ToList();
            }

            return null;
        }
    }
}
