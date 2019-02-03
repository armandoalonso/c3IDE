using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities.Helpers;
using Newtonsoft.Json;

namespace c3IDE.Utilities.CodeCompletion.Bindings
{
    public class JavascriptBinding : ICompletionBindings
    {
        public IList<GenericCompletionItem> Completions { get; set; }
        public JavascriptBinding()
        {
            var completionJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Utilities.CodeCompletion.Bindings.code_javascript.json");
            Completions = JsonConvert.DeserializeObject<List<GenericCompletionItem>>(completionJson).ToList();
        }

    }
}
