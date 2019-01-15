using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Newtonsoft.Json;

namespace c3IDE.Utilities.CodeCompletion
{
    public class GenericCompletionItem : ICompletionData
    {
        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            ISegment currentWord = textArea.GetCurrentWord();
            textArea.Document.Replace(currentWord ?? completionSegment, this.Text);
        }

        public GenericCompletionItem(string text, string description, CompletionType type = CompletionType.Misc)
        {
            this.Text = text;
            this.Content = text;
            this.Description = description;
            this.Type = type;
            this.Image = CompletionTypeFactory.Insatnce.GetIcon(this.Type);
        }

        [JsonIgnore]
        public ImageSource Image { get; }
        public string Text { get; }
        public object Content {get; }
        public CompletionType Type { get; }
        public object Description { get; }
        [JsonIgnore]
        public double Priority => 1.0;
    }
}
