using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using c3IDE.Models;
using c3IDE.Utilities.Extentions;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;

namespace c3IDE.Utilities.Search
{
    public class Searcher : Singleton<Searcher>
    {
        public Dictionary<string, SortedList<int, SearchResult>> FileIndex = new Dictionary<string, SortedList<int, SearchResult>>();

        public async void GlobalFind(string text)
        {
             var results = await Task.Run(() =>
            {
                return FileIndex.Values.SelectMany(x => x.Values.Where(rec => rec.Line.Contains(text)));
            });

            AppData.Insatnce.OpenFindAndReplace(results.ToList());
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
                    UpdateFileIndex($"action_{action.Key}_ace", action.Value.Ace);
                    UpdateFileIndex($"action_{action.Key}_lang", action.Value.Language);
                    UpdateFileIndex($"action_{action.Key}_code", action.Value.Code);
                }

                foreach (var conditions in addon.Conditions)
                {
                    UpdateFileIndex($"condition_{conditions.Key}_ace", conditions.Value.Ace);
                    UpdateFileIndex($"condition_{conditions.Key}_lang", conditions.Value.Language);
                    UpdateFileIndex($"condition_{conditions.Key}_code", conditions.Value.Code);
                }

                foreach (var expression in addon.Expressions)
                {
                    UpdateFileIndex($"expression_{expression.Key}_ace", expression.Value.Ace);
                    UpdateFileIndex($"expression_{expression.Key}_lang", expression.Value.Language);
                    UpdateFileIndex($"expression_{expression.Key}_code", expression.Value.Code);
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
                sList.Add(index, new SearchResult { Document = filename, Line = result[index], LineNumber = index });
            }

            FileIndex.AddOrUpdate(filename, sList);
        }
    }
}
