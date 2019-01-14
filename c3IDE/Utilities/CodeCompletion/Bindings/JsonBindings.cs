using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Utilities.CodeCompletion.Bindings
{
    public class JsonBindings : ICompletionBindings
    {
        public IList<ICompletionData> Completions { get; set; }

        public JsonBindings()
        {
            Completions = new List<ICompletionData>
            {
                new GenericCompletionItem("true", "true", CompletionType.Keywords),
                new GenericCompletionItem("false", "false", CompletionType.Keywords),
            };
        }

    }
}
