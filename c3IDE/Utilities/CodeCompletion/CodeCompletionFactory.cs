using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Utilities.CodeCompletion.Bindings;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Utilities.CodeCompletion
{
    public class CodeCompletionFactory : Singleton<CodeCompletionFactory>
    {
        private readonly Dictionary<CodeType, IList<GenericCompletionItem>> _bindingCache = new Dictionary<CodeType, IList<GenericCompletionItem>>();
        private readonly Dictionary<string, IList<GenericCompletionItem>> _contextCache = new Dictionary<string, IList<GenericCompletionItem>>();
        private readonly HashSet<GenericCompletionItem> _globalEditTimeTokens = new HashSet<GenericCompletionItem>();
        private readonly HashSet<GenericCompletionItem> _globalRunTimeTokens = new HashSet<GenericCompletionItem>();
        private readonly HashSet<GenericCompletionItem> _globalJsonTokens = new HashSet<GenericCompletionItem>();

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
                case CodeType.EditTimeJavascript:
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
                case CodeType.EditTimeJavascript:
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
                if(token.Length < 2) continue;

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
                        switch (type)
                        {
                            case CodeType.Json:
                                _globalJsonTokens.Add(new GenericCompletionItem(token, "user defined", CompletionType.Misc));
                                break;
                            case CodeType.EditTimeJavascript:
                                _globalEditTimeTokens.Add(new GenericCompletionItem(token, "user defined", CompletionType.Misc));
                                break;
                            case CodeType.RuntimeJavascript:
                                _globalRunTimeTokens.Add(new GenericCompletionItem(token, "user defined", CompletionType.Misc));
                                break;
                        }

                    }
                }
            }

            switch (type)
            {
                case CodeType.Json:
                    completionList.AddRange(_globalJsonTokens);
                    break;
                case CodeType.EditTimeJavascript:
                    completionList.AddRange(_globalEditTimeTokens);
                    break;
                case CodeType.RuntimeJavascript:
                    completionList.AddRange(_globalRunTimeTokens);
                    break;
            }

            return completionList.ToList();
        }

        public void PopulateUserDefinedTokens(CodeType type, params string[] texts)
        {
            switch (type)
            {
                case CodeType.Json:
                    foreach (var text in texts)
                    {
                        _globalJsonTokens.AddRange(JavascriptParser.Insatnce.ParseJavascriptUserTokens(text)
                            .Select(x => new GenericCompletionItem(x, "user defined", CompletionType.Misc)));
                    }
                    break;
                case CodeType.EditTimeJavascript:
                    foreach (var text in texts)
                    {
                        _globalEditTimeTokens.AddRange(JavascriptParser.Insatnce.ParseJavascriptUserTokens(text)
                            .Select(x => new GenericCompletionItem(x, "user defined", CompletionType.Misc)));
                    }
                    break;
                case CodeType.RuntimeJavascript:
                    foreach (var text in texts)
                    {
                        _globalRunTimeTokens.AddRange(JavascriptParser.Insatnce.ParseJavascriptUserTokens(text)
                            .Select(x => new GenericCompletionItem(x, "user defined", CompletionType.Misc)));
                    }
                    break;
            }  
        }
    }

    public enum CodeType
    {
        Json,
        EditTimeJavascript,
        RuntimeJavascript
    }
}
