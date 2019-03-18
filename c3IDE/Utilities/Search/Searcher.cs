using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.DataAccess;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Utilities.Extentions;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Utilities.Search
{
    public class Searcher : Singleton<Searcher>
    {
        public Dictionary<string, SortedList<int, SearchResult>> FileIndex = new Dictionary<string, SortedList<int, SearchResult>>();
        public string LastSearchedWord { get; set; }

        public async void GlobalFind(string text, IWindow window)
        {
            LastSearchedWord = text;
            var results = await Task.Run(() =>
            {
                return FileIndex.Values.SelectMany(x => x.Values.Where(rec => rec.Line.Contains(text)));
            });
            var searchResults = results as SearchResult[] ?? results.ToArray();
            searchResults.ToList().ForEach(x => x.Selected = true);
            WindowManager.OpenFindAndReplace(searchResults, window);
        }

        public void GlobalReplace(C3Addon addon, IEnumerable<SearchResult> replaceResults)
        {
             var affectedFiles = new HashSet<string>(replaceResults.Select(x => x.Document));

             foreach (var affectedFile in affectedFiles)
             {
                 var fileReplacments = replaceResults.Where(x => x.Document.Equals(affectedFile));
                 foreach (var fileReplacment in fileReplacments)
                 {
                     FileIndex[affectedFile].AddOrUpdate(fileReplacment.LineNumber, fileReplacment);
                 }
             }

             ReconstructAdoon(addon);
        }

        public void ParseAddon(C3Addon addon)
        {
            FileIndex = new Dictionary<string, SortedList<int, SearchResult>>();
            if (addon.Type != PluginType.Effect)
            {
                UpdateFileIndex("addon.json", addon.AddonJson, ApplicationWindows.AddonWindow);
                UpdateFileIndex("edittime_plugin.js", addon.PluginEditTime, ApplicationWindows.PluginWindow);
                UpdateFileIndex("runtime_plugin.js", addon.PluginRunTime, ApplicationWindows.PluginWindow);
                UpdateFileIndex("edittime_instance.js", addon.InstanceEditTime, ApplicationWindows.InstanceWindow);
                UpdateFileIndex("runtime_instance.js", addon.InstanceRunTime, ApplicationWindows.InstanceWindow);
                UpdateFileIndex("edittime_type.js", addon.TypeEditTime, ApplicationWindows.TypeWindow);
                UpdateFileIndex("runtime_type.js", addon.TypeRunTime, ApplicationWindows.TypeWindow);

                foreach (var action in addon.Actions)
                {
                    UpdateFileIndex($"act_{action.Key}_ace", action.Value.Ace, ApplicationWindows.ActionWindow);
                    UpdateFileIndex($"act_{action.Key}_lang", action.Value.Language, ApplicationWindows.ActionWindow);
                    UpdateFileIndex($"act_{action.Key}_code", action.Value.Code, ApplicationWindows.ActionWindow);
                }

                foreach (var conditions in addon.Conditions)
                {
                    UpdateFileIndex($"cnd_{conditions.Key}_ace", conditions.Value.Ace, ApplicationWindows.ConditionWindow);
                    UpdateFileIndex($"cnd_{conditions.Key}_lang", conditions.Value.Language, ApplicationWindows.ConditionWindow);
                    UpdateFileIndex($"cnd_{conditions.Key}_code", conditions.Value.Code, ApplicationWindows.ConditionWindow);
                }

                foreach (var expression in addon.Expressions)
                {
                    UpdateFileIndex($"exp_{expression.Key}_ace", expression.Value.Ace, ApplicationWindows.ExpressionWindow);
                    UpdateFileIndex($"exp_{expression.Key}_lang", expression.Value.Language, ApplicationWindows.ExpressionWindow);
                    UpdateFileIndex($"exp_{expression.Key}_code", expression.Value.Code, ApplicationWindows.ExpressionWindow);
                }

                UpdateFileIndex("lang_property.js", addon.LanguageProperties, ApplicationWindows.LanguageWindow);
                UpdateFileIndex("lang_category.js", addon.LanguageCategories, ApplicationWindows.LanguageWindow);
            }
            else
            {
                UpdateFileIndex("fxcode.js", addon.Effect.Code, ApplicationWindows.EffectCodeWindow);

                foreach (var param in addon.Effect.Parameters)
                {
                    UpdateFileIndex($"fxparam_{param.Key}_json", addon.Effect.Parameters[param.Key].Json, ApplicationWindows.EffectParameterWindow);
                    UpdateFileIndex($"fxparam_{param.Key}_lang", addon.Effect.Parameters[param.Key].Lang, ApplicationWindows.EffectParameterWindow);
                }
            }
        }

        public void UpdateFileIndex(string filename, string content, IWindow window)
        {
            var sList = new SortedList<int, SearchResult>();
            if (content == null) return;
            var result = Regex.Split(content, "\r\n|\r|\n");
            for (int index = 0; index < result.Length; index++)
            {
                sList.Add(index, new SearchResult(filename, result[index], index, window));
            }

            FileIndex.AddOrUpdate(filename, sList);
        }

        public void ReconstructAdoon(C3Addon addon)
        {
            if (addon.Type != PluginType.Effect)
            {
                addon.AddonJson = string.Join("\n", FileIndex["addon.json"].Select(x => x.Value.Line));
                addon.PluginEditTime = string.Join("\n", FileIndex["edittime_plugin.js"].Select(x => x.Value.Line));
                addon.PluginRunTime = string.Join("\n", FileIndex["runtime_plugin.js"].Select(x => x.Value.Line));
                addon.InstanceEditTime = string.Join("\n", FileIndex["edittime_instance.js"].Select(x => x.Value.Line));
                addon.InstanceRunTime = string.Join("\n", FileIndex[ "runtime_instance.js"].Select(x => x.Value.Line));
                addon.TypeEditTime = string.Join("\n", FileIndex["edittime_type.js"].Select(x => x.Value.Line));
                addon.TypeRunTime = string.Join("\n", FileIndex["runtime_type.js"].Select(x => x.Value.Line));

                foreach (var act in addon.Actions)
                {
                    addon.Actions[act.Key].Ace = string.Join("\n", FileIndex[$"act_{act.Key}_ace"].Select(x => x.Value.Line));
                    addon.Actions[act.Key].Language = string.Join("\n", FileIndex[$"act_{act.Key}_lang"].Select(x => x.Value.Line));
                    addon.Actions[act.Key].Code = string.Join("\n", FileIndex[$"act_{act.Key}_code"].Select(x => x.Value.Line));
                }

                foreach (var cnd in addon.Conditions)
                {
                    addon.Conditions[cnd.Key].Ace = string.Join("\n", FileIndex[$"cnd_{cnd.Key}_ace"].Select(x => x.Value.Line));
                    addon.Conditions[cnd.Key].Language = string.Join("\n", FileIndex[$"cnd_{cnd.Key}_lang"].Select(x => x.Value.Line));
                    addon.Conditions[cnd.Key].Code = string.Join("\n", FileIndex[$"cnd_{cnd.Key}_code"].Select(x => x.Value.Line));
                }


                foreach (var exp in addon.Expressions)
                {
                    addon.Expressions[exp.Key].Ace = string.Join("\n", FileIndex[$"exp_{exp.Key}_ace"].Select(x => x.Value.Line));
                    addon.Expressions[exp.Key].Language = string.Join("\n", FileIndex[$"exp_{exp.Key}_lang"].Select(x => x.Value.Line));
                    addon.Expressions[exp.Key].Code = string.Join("\n", FileIndex[$"exp_{exp.Key}_code"].Select(x => x.Value.Line));
                }

                addon.LanguageProperties = string.Join("\n", FileIndex["lang_property.js"].Select(x => x.Value.Line));
                addon.LanguageCategories = string.Join("\n", FileIndex["lang_category.js"].Select(x => x.Value.Line));
            }
            else
            {
                addon.Effect.Code = string.Join("\n", FileIndex["fxcode.js"].Select(x => x.Value.Line));
                foreach (var param in addon.Effect.Parameters)
                {
                    addon.Effect.Parameters[param.Key].Json = string.Join("\n", FileIndex[$"fxparam_{param.Key}_json"].Select(x => x.Value.Line));
                    addon.Effect.Parameters[param.Key].Lang = string.Join("\n", FileIndex[$"fxparam_{param.Key}_lang"].Select(x => x.Value.Line));
                }
            }

            AddonManager.CurrentAddon = addon;
            DataAccessFacade.Insatnce.AddonData.Upsert(addon);
        }

        public void UpdateFileIndex(C3Addon addon, IWindow window)
        {
            switch (window.DisplayName)
            {
                case "Addon":
                    UpdateFileIndex("addon.json", addon.AddonJson, ApplicationWindows.AddonWindow);
                    break;
                case "Plugin":
                    UpdateFileIndex("edittime_plugin.js", addon.PluginEditTime, ApplicationWindows.PluginWindow);
                    UpdateFileIndex("runtime_plugin.js", addon.PluginRunTime, ApplicationWindows.PluginWindow);
                    break;
                case "Type":
                    UpdateFileIndex("edittime_type.js", addon.TypeEditTime, ApplicationWindows.TypeWindow);
                    UpdateFileIndex("runtime_type.js", addon.TypeRunTime, ApplicationWindows.TypeWindow);
                    break;
                case "Instance":
                    UpdateFileIndex("edittime_instance.js", addon.InstanceEditTime, ApplicationWindows.InstanceWindow);
                    UpdateFileIndex("runtime_instance.js", addon.InstanceRunTime, ApplicationWindows.InstanceWindow);
                    break;
                case "Actions":
                    //foreach (var act in addon.Actions)
                    //{
                    //    addon.Actions[act.Key].Ace = string.Join("\n", FileIndex[$"act_{act.Key}_ace"].Select(x => x.Value.Line));
                    //    addon.Actions[act.Key].Language = string.Join("\n", FileIndex[$"act_{act.Key}_lang"].Select(x => x.Value.Line));
                    //    addon.Actions[act.Key].Code = string.Join("\n", FileIndex[$"act_{act.Key}_code"].Select(x => x.Value.Line));
                    //}
                    foreach (var action in addon.Actions)
                    {
                        UpdateFileIndex($"act_{action.Key}_ace", action.Value.Ace, ApplicationWindows.ActionWindow);
                        UpdateFileIndex($"act_{action.Key}_lang", action.Value.Language, ApplicationWindows.ActionWindow);
                        UpdateFileIndex($"act_{action.Key}_code", action.Value.Code, ApplicationWindows.ActionWindow);
                    }
                    break;
                case "Conditions":
                    //foreach (var cnd in addon.Conditions)
                    //{
                    //    addon.Conditions[cnd.Key].Ace = string.Join("\n", FileIndex[$"cnd_{cnd.Key}_ace"].Select(x => x.Value.Line));
                    //    addon.Conditions[cnd.Key].Language = string.Join("\n", FileIndex[$"cnd_{cnd.Key}_lang"].Select(x => x.Value.Line));
                    //    addon.Conditions[cnd.Key].Code = string.Join("\n", FileIndex[$"cnd_{cnd.Key}_code"].Select(x => x.Value.Line));
                    //}
                    foreach (var conditions in addon.Conditions)
                    {
                        UpdateFileIndex($"cnd_{conditions.Key}_ace", conditions.Value.Ace, ApplicationWindows.ConditionWindow);
                        UpdateFileIndex($"cnd_{conditions.Key}_lang", conditions.Value.Language, ApplicationWindows.ConditionWindow);
                        UpdateFileIndex($"cnd_{conditions.Key}_code", conditions.Value.Code, ApplicationWindows.ConditionWindow);
                    }
                    break;
                case "Expressions":
                    //foreach (var expression in addon.Expressions)
                    //{
                    //    UpdateFileIndex($"exp_{expression.Key}_ace", expression.Value.Ace, ApplicationWindows.ExpressionWindow);
                    //    UpdateFileIndex($"exp_{expression.Key}_lang", expression.Value.Language, ApplicationWindows.ExpressionWindow);
                    //    UpdateFileIndex($"exp_{expression.Key}_code", expression.Value.Code, ApplicationWindows.ExpressionWindow);
                    //}
                    foreach (var expression in addon.Expressions)
                    {
                        UpdateFileIndex($"exp_{expression.Key}_ace", expression.Value.Ace, ApplicationWindows.ExpressionWindow);
                        UpdateFileIndex($"exp_{expression.Key}_lang", expression.Value.Language, ApplicationWindows.ExpressionWindow);
                        UpdateFileIndex($"exp_{expression.Key}_code", expression.Value.Code, ApplicationWindows.ExpressionWindow);
                    }
                    break;
                case "Language":
                    UpdateFileIndex("lang_property.js", addon.LanguageProperties, ApplicationWindows.LanguageWindow);
                    UpdateFileIndex("lang_category.js", addon.LanguageCategories, ApplicationWindows.LanguageWindow);
                    break;
            }
        }
    }
}
