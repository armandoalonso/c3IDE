using System.Collections.Generic;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Utilities.CodeCompletion.Bindings
{
    public interface ICompletionBindings
    {
        IList<ICompletionData> Completions { get; set; }
    }
}