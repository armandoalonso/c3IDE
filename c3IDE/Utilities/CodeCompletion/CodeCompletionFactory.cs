using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Utilities.CodeCompletion.Bindings;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Utilities.CodeCompletion
{
    public class CodeCompletionFactory : Singleton<CodeCompletionFactory>
    {
        private readonly Dictionary<CodeType, IList<GenericCompletionItem>> _bindingCache = new Dictionary<CodeType, IList<GenericCompletionItem>>();
        private readonly Dictionary<string, IList<GenericCompletionItem>> _contextCache = new Dictionary<string, IList<GenericCompletionItem>>();
        private readonly Dictionary<string, List<string>> _methodInterfaceCache = new Dictionary<string, List<string>>();
        
        private readonly Regex _tokenRegex = new Regex("SDK\\.(\\w+)");
        private readonly Regex _methodRegex = new Regex("(\\w+)\\(.*?\\)");

        public IList<GenericCompletionItem> GetCompletionData(CodeType type)
        {
            if (_bindingCache.ContainsKey(type))
            {
                return _bindingCache[type];
            }

            switch (type)
            {
                case CodeType.Json:
                    _bindingCache.Add(type, new JsonBindings().Completions);
                    return _bindingCache[type];
                case CodeType.EdittimeJavascript:
                    _bindingCache.Add(type, new EditorJavascriptBinding().Completions);
                    return _bindingCache[type];
                default:
                    return null;
            }
        }

        public IList<GenericCompletionItem> GetCompletionData(IEnumerable<string> tokenList, CodeType type)
        {
            var completionList = new List<GenericCompletionItem>();
            var allList = GetCompletionData(type);

            foreach (var token in tokenList)
            {
                if (_contextCache.ContainsKey(token))
                {
                    completionList.AddRange(_contextCache[token]);
                }
                else
                {
                    _contextCache[token] = allList.Where(x => x.Container.Contains(token)).ToList();
                    completionList.AddRange(_contextCache[token]);
                }
            }

            completionList.AddRange(allList.Where(x => x.Container.Contains("Javascript")));
            return completionList;
        }

        public List<string> ParseJavascriptDocument(string text)
        {
            var mathes = _tokenRegex.Matches(text);
            var hashset = new HashSet<string>();

            foreach (Match match in mathes)
            {
                //do not include strings 
                if(!match.Value[0].Equals('\"') && !match.Value[0].Equals('\''))
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
            if (_methodInterfaceCache.Count == 0)
            {
                var mapping = new MethodInterfaceMapping();
                foreach (var map in mapping.Interfaces)
                {
                    if (!_methodInterfaceCache.ContainsKey(map.Method))
                    {
                        _methodInterfaceCache.Add(map.Method, map.Interface);
                    }
                }
            }

            switch (type)
            {
                case CodeType.EdittimeJavascript:
                    foreach (var m in methods)
                    {
                        if (_methodInterfaceCache.ContainsKey(m))
                        {
                            results.AddRange(_methodInterfaceCache[m]);
                        }
                    }

                    return results.ToList();
            }

            return null;
        }
    }

    public enum CodeType
    {
        Json,
        EdittimeJavascript,
        RuntimeJavascript
    }
}
