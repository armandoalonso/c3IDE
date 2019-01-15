using System.Collections.Generic;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Utilities.CodeCompletion.Bindings
{
    public interface ICompletionBindings
    {
        IList<GenericCompletionItem> Completions { get; set; }
    }
}