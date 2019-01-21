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
                case CodeType.RuntimeJavascript:
                    _bindingCache.Add(type, new RunTimeJavascriptBinding().Completions);
                    return _bindingCache[type];
                default:
                    return null;
            }
        }

        public IList<GenericCompletionItem> GetCompletionData(IEnumerable<string> tokenList, CodeType type)
        {
            var completionList = new HashSet<GenericCompletionItem>();
            var allList = GetCompletionData(type);

            switch (type)
            {
                case CodeType.Json:
                    break;
                case CodeType.EdittimeJavascript:
                    completionList.AddRange(allList.Where(x => x.Container.Contains("Javascript")));
                    completionList.AddRange(allList.Where(x => x.Container.Contains("SDK")));
                    break;
                case CodeType.RuntimeJavascript:
                    completionList.AddRange(allList.Where(x => x.Container.Contains("Javascript")));
                    completionList.AddRange(allList.Where(x => x.Container.Contains("C3")));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            foreach (var token in tokenList)
            {
                if (_contextCache.ContainsKey(token))
                {
                    completionList.AddRange(_contextCache[token]);
                }
                else
                {
                    var definedCompletions = allList.Where(x => x.Container.Contains(token)).ToList();
                    if (definedCompletions.Any())
                    {
                        _contextCache[token] = definedCompletions;
                        completionList.AddRange(_contextCache[token]);
                    }
                    else
                    {
                        completionList.Add(new GenericCompletionItem(token, "user defined", CompletionType.Misc));
                    }
                }
            }

            return completionList.ToList();
        }
    }

    public enum CodeType
    {
        Json,
        EdittimeJavascript,
        RuntimeJavascript
    }
}
