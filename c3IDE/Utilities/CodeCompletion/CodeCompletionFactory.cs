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
        private readonly Dictionary<string, HashSet<GenericCompletionItem>> _globalTokens = new Dictionary<string, HashSet<GenericCompletionItem>>();

        public IList<GenericCompletionItem> GetCompletionData(IEnumerable<string> tokenList, string key)
        {
            var completionList = new HashSet<GenericCompletionItem>();

            if (_globalTokens.ContainsKey(key))
            {
                _globalTokens[key] = new HashSet<GenericCompletionItem>(tokenList.Select(x =>
                    new GenericCompletionItem(x, string.Empty, CompletionType.Misc)));
            }
            else
            {
                _globalTokens.Add(key, new HashSet<GenericCompletionItem>(tokenList.Select(x =>
                    new GenericCompletionItem(x, string.Empty, CompletionType.Misc))));
            }

            //get user completion
            var hashSets = _globalTokens.Where(x => x.Key != key).SelectMany(x => x.Value);
            completionList.AddRange(hashSets.Select(x => x));

            //merge in custom completion info
            completionList.AddRange(CodeCompletionPipeline.Insatnce.Use("all_scripts"));

            return completionList.ToList();
        }

        public void PopulateUserDefinedTokens(string key, string text, bool wipe = false)
        {
            if (_globalTokens.ContainsKey(key))
            {
                if (wipe)
                {
                    _globalTokens.Remove(key);
                    _globalTokens.Add(key, new HashSet<GenericCompletionItem>());
                }

                _globalTokens[key].AddRange(JavascriptParser.Insatnce.ParseJavascriptUserTokens(text)
                    .Select(x => new GenericCompletionItem(x, string.Empty, CompletionType.Misc)));
            }
            else
            {
                _globalTokens.Add(key, new HashSet<GenericCompletionItem>());
                _globalTokens[key].AddRange(JavascriptParser.Insatnce.ParseJavascriptUserTokens(text)
                    .Select(x => new GenericCompletionItem(x, string.Empty, CompletionType.Misc)));
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
