using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities.CodeCompletion.Bindings;
using c3IDE.Utilities.Extentions;

namespace c3IDE.Utilities.CodeCompletion
{
    public class CodeCompletionPipeline : Singleton<CodeCompletionPipeline>
    {
        private readonly Dictionary<string, HashSet<GenericCompletionItem>> _globalDefinitions = new Dictionary<string, HashSet<GenericCompletionItem>>();

        public IEnumerable<GenericCompletionItem> Use(string key)
        {
            var list = new List<GenericCompletionItem>();
            if (_globalDefinitions.ContainsKey(key))
            {
                return _globalDefinitions[key];
            }

            switch (key)
            {
                case "runtime_script":
                    _globalDefinitions.Add(key, new HashSet<GenericCompletionItem>(new RunTimeJavascriptBinding().Completions));
                    break;
                case "edittime_script":
                    _globalDefinitions.Add(key, new HashSet<GenericCompletionItem>(new EditorJavascriptBinding().Completions));
                    break;
                case "json":
                    _globalDefinitions.Add(key, new HashSet<GenericCompletionItem>(new JsonBindings().Completions));
                    break;
                case "all_scripts":
                    _globalDefinitions.Add(key, new HashSet<GenericCompletionItem>(new RunTimeJavascriptBinding().Completions));
                    _globalDefinitions[key].AddRange(new EditorJavascriptBinding().Completions);
                    _globalDefinitions[key].AddRange(new JavascriptBinding().Completions);
                    break;
                default:
                    throw new ArgumentException("key is not defined");
            }

            list.AddRange(_globalDefinitions[key]);
            return list;
        }

    }
}
