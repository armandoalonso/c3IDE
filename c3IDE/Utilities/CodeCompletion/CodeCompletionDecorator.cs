using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace c3IDE.Utilities.CodeCompletion
{
    public class CodeCompletionDecorator : Singleton<CodeCompletionDecorator>
    {
        //decorates the editor completion data with the a set of completion data
        public void Decorate(ref IList<ICompletionData> editorData, IList<ICompletionData> completionData)
        {
            foreach (var cd in completionData)
            {
                editorData.Add(cd);
            }
        }
    }
}
