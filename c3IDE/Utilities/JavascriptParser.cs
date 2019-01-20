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

        private readonly Regex _tokenSDKRegex = new Regex("SDK\\.(\\w+)");
        private readonly Regex _tokenC3Regex = new Regex("C3\\.(\\w+).?(\\w+)?");
        private readonly Regex _methodRegex = new Regex("(\\w+)\\(.*?\\)");

        public List<string> ParseJavascriptDocument(string text, CodeType type)
        {
            var matches = type == CodeType.RuntimeJavascript ? _tokenC3Regex.Matches(text) : _tokenSDKRegex.Matches(text);
            var hashset = new HashSet<string>();

            foreach (Match match in matches)
            {
                //do not include strings 
                if (!match.Value[0].Equals('\"') && !match.Value[0].Equals('\''))
                {
                    hashset.Add(match.Groups[1].ToString());

                    if (match.Groups.Count > 2 && !string.IsNullOrWhiteSpace(match.Groups[2].ToString()))
                    {
                        hashset.Add(match.Groups[2].ToString());
                    }
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
                case CodeType.RuntimeJavascript:
                    if (_runtimeMethodcache.Count == 0)
                    {
                        var mapping = new MethodInterfaceMapping();
                        foreach (var map in mapping.Interfaces)
                        {
                            if (!_runtimeMethodcache.ContainsKey(map.Method))
                            {
                                _runtimeMethodcache.Add(map.Method, map.Interface);
                            }
                        }
                    }
                    foreach (var m in methods)
                    {
                        if (_runtimeMethodcache.ContainsKey(m))
                        {
                            results.AddRange(_runtimeMethodcache[m]);
                        }
                    }

                    return results.ToList();
            }

            return null;
        }
    }
}
