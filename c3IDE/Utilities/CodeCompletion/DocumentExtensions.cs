using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace c3IDE.Utilities.CodeCompletion
{
    public static class DocumentExtensions
    {
        private static Regex _wordRegex = new Regex(@"[^\W\d][\w]*(?<=\w)", RegexOptions.Compiled);

        public static ISegment GetCurrentWord(this TextArea textArea)
        {
            DocumentLine line = textArea.Document.GetLineByNumber(textArea.Caret.Line);

            if (line.Length == 0)
                return null;

            int lineCaretPosition = textArea.Caret.Offset - line.Offset;

            int startOffset = -1;
            int length = -1;

            MatchCollection matches = _wordRegex.Matches(textArea.Document.GetText(line));
            foreach (Match match in matches)
            {
                if (match.Index <= lineCaretPosition && (match.Index + match.Length) >= lineCaretPosition)
                {
                    startOffset = line.Offset + match.Index;
                    length = match.Length;
                    break;
                }
            }

            if (startOffset != -1)
                return new AnchorSegment(textArea.Document, startOffset, length);

            return null;
        }
    }
}
