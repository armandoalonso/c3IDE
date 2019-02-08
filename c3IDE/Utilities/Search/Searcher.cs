using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Utilities.Extentions;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;

namespace c3IDE.Utilities.Search
{
    public class Searcher : Singleton<Searcher>
    {
        public Dictionary<string, SortedList<int, SearchResult>> FileIndex = new Dictionary<string, SortedList<int, SearchResult>>();
        public string LastSearchedWord { get; set; }

        public async void GlobalFind(string text, IWindow window)
        {
            ParseAddon(AppData.Insatnce.CurrentAddon);
            LastSearchedWord = text;
            var results = await Task.Run(() =>
            {
                return FileIndex.Values.SelectMany(x => x.Values.Where(rec => rec.Line.Contains(text)));
            });
            var searchResults = results as SearchResult[] ?? results.ToArray();
            searchResults.ToList().ForEach(x => x.Selected = true);
            AppData.Insatnce.OpenFindAndReplace(searchResults, window);
        }

        public  void GlobalReplace(C3Addon addon, IEnumerable<SearchResult> replaceResults)
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
                UpdateFileIndex("addon.json", addon.AddonJson);
                UpdateFileIndex("edittime_plugin.js", addon.PluginEditTime);
                UpdateFileIndex("runtime_plugin.js", addon.PluginRunTime);
                UpdateFileIndex("edittime_instance.js", addon.InstanceEditTime);
                UpdateFileIndex("runtime_instance.js", addon.InstanceRunTime);
                UpdateFileIndex("edittime_type.js", addon.TypeEditTime);
                UpdateFileIndex("runtime_type.js", addon.TypeRunTime);

                foreach (var action in addon.Actions)
                {
                    UpdateFileIndex($"act_{action.Key}_ace", action.Value.Ace);
                    UpdateFileIndex($"act_{action.Key}_lang", action.Value.Language);
                    UpdateFileIndex($"act_{action.Key}_code", action.Value.Code);
                }

                foreach (var conditions in addon.Conditions)
                {
                    UpdateFileIndex($"cnd_{conditions.Key}_ace", conditions.Value.Ace);
                    UpdateFileIndex($"cnd_{conditions.Key}_lang", conditions.Value.Language);
                    UpdateFileIndex($"cnd_{conditions.Key}_code", conditions.Value.Code);
                }

                foreach (var expression in addon.Expressions)
                {
                    UpdateFileIndex($"exp_{expression.Key}_ace", expression.Value.Ace);
                    UpdateFileIndex($"exp_{expression.Key}_lang", expression.Value.Language);
                    UpdateFileIndex($"exp_{expression.Key}_code", expression.Value.Code);
                }

                UpdateFileIndex("lang_property.js", addon.LanguageProperties);
                UpdateFileIndex("lang_category.js", addon.LanguageCategories);
            }
            else
            {
                //handle effect specific files
            }
        }

        public void UpdateFileIndex(string filename, string content)
        {
            var sList = new SortedList<int, SearchResult>();
            var result = Regex.Split(content, "\r\n|\r|\n");
            for (int index = 0; index < result.Length; index++)
            {
                sList.Add(index, new SearchResult(filename, result[index], index));
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

            AppData.Insatnce.CurrentAddon = addon;
        }
    }
}
