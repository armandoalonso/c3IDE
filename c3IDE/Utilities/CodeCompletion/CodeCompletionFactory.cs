using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Models;
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

        public void ParseAddon(C3Addon addon)
        {
            if (addon.Type != PluginType.Effect)
            {
                PopulateUserDefinedTokens("addon_json", addon.AddonJson);
                PopulateUserDefinedTokens("runtime_plugin_script", addon.PluginRunTime);
                PopulateUserDefinedTokens("edittime_plugin_script", addon.PluginEditTime);
                PopulateUserDefinedTokens("runtime_type_script", addon.TypeRunTime);
                PopulateUserDefinedTokens("edittime_type_script", addon.TypeEditTime);
                PopulateUserDefinedTokens("runtime_instance_script", addon.InstanceRunTime);
                PopulateUserDefinedTokens("edittime_instance_script", addon.InstanceEditTime);

                foreach (var act in addon.Actions)
                {
                    PopulateUserDefinedTokens($"{act.Value.Id}_code_script", act.Value.Code);
                    PopulateUserDefinedTokens($"{act.Value.Id}_lang_json", act.Value.Language);
                    PopulateUserDefinedTokens($"{act.Value.Id}_ace_json", act.Value.Ace);
                }

                foreach (var cnd in addon.Conditions)
                {
                    PopulateUserDefinedTokens($"{cnd.Value.Id}_code_script", cnd.Value.Code);
                    PopulateUserDefinedTokens($"{cnd.Value.Id}_lang_json", cnd.Value.Language);
                    PopulateUserDefinedTokens($"{cnd.Value.Id}_ace_json", cnd.Value.Ace);
                }

                foreach (var exp in addon.Expressions)
                {
                    PopulateUserDefinedTokens($"{exp.Value.Id}_code_script", exp.Value.Code);
                    PopulateUserDefinedTokens($"{exp.Value.Id}_lang_json", exp.Value.Language);
                    PopulateUserDefinedTokens($"{exp.Value.Id}_ace_json", exp.Value.Ace);
                }

                //foreach (var value in addon.ThirdPartyFiles.Values)
                //{
                //    PopulateUserDefinedTokens(value.FileName, value.Content);
                //}
            }
            else
            {
                //handle auto completion in effect code
            }
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
