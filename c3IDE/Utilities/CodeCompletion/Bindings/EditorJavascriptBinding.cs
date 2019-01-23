using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Collections.Generic;
using System.Linq;
using c3IDE.Utilities.Helpers;
using Newtonsoft.Json;

namespace c3IDE.Utilities.CodeCompletion.Bindings
{
    public class EditorJavascriptBinding : ICompletionBindings
    {
        public IList<GenericCompletionItem> Completions { get; set; }

        public EditorJavascriptBinding()
        {
            var completionJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Utilities.CodeCompletion.Bindings.code_editor_javascript.json");
            Completions = JsonConvert.DeserializeObject<List<GenericCompletionItem>>(completionJson).ToList();
        }
    }
}