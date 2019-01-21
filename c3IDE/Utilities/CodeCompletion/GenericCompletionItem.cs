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
    public class GenericCompletionItem : ICompletionData, IEquatable<GenericCompletionItem>
    {
        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            ISegment currentWord = textArea.GetCurrentWord();
            textArea.Document.Replace(currentWord ?? completionSegment, this.Text);
        }

        public GenericCompletionItem(string text, string description, CompletionType type = CompletionType.Misc)
        {
            this.Text = text;
            this.DescriptionText = description;
            this.Type = type;
            this.Image = CompletionTypeFactory.Insatnce.GetIcon(this.Type);
            this.Container = string.Empty;
        }

        [JsonIgnore]
        public ImageSource Image { get; }
        public string Text { get; }

        [JsonIgnore]
        public object Content => $"{Text}";
        public CompletionType Type { get; }
        public string Container { get; set; }
        public string DescriptionText { get; set; }
        [JsonIgnore]
        public object Description => $"{Container} : {DescriptionText}";
        [JsonIgnore]
        public double Priority => 1.0;

        public bool Equals(GenericCompletionItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GenericCompletionItem) obj);
        }

        public override int GetHashCode()
        {
            return (Text != null ? Text.GetHashCode() : 0);
        }
    }
}
