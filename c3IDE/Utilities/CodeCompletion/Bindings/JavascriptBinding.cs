using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Utilities.CodeCompletion.Bindings
{
    public class JavascriptBinding : ICompletionBindings
    {
        public IList<ICompletionData> Completions { get; set; }

        public JavascriptBinding()
        {
            Completions = new List<ICompletionData>
            {
                new GenericCompletionItem("true", "true", CompletionType.Keywords),
                new GenericCompletionItem("false", "false", CompletionType.Keywords),

                //plugin js
                new GenericCompletionItem("data-and-storage", "plugin_category", CompletionType.Misc),
                new GenericCompletionItem("form-controls", "plugin_category", CompletionType.Misc),
                new GenericCompletionItem("general", "plugin_category", CompletionType.Misc),
                new GenericCompletionItem("input", "plugin_category", CompletionType.Misc),
                new GenericCompletionItem("media", "plugin_category", CompletionType.Misc),
                new GenericCompletionItem("monetisation", "plugin_category", CompletionType.Misc),
                new GenericCompletionItem("platform-specific", "plugin_category", CompletionType.Misc),
                new GenericCompletionItem("web", "plugin_category", CompletionType.Misc),
                new GenericCompletionItem("other", "plugin_category", CompletionType.Misc),

                //classes
                new GenericCompletionItem("this._info", "IPluginInfo", CompletionType.Classes),

                //iplugininfo
                new GenericCompletionItem("SetName(name)", "Set the name of the addon. Typically this is read from the language file.", CompletionType.Methods),
                new GenericCompletionItem("SetDescription(description)", "Set the description of the addon. Typically this is read from the language file.", CompletionType.Methods)
            };
        }
    }
}
