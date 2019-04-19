using System;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace c3IDE.Utilities.Extentions
{
    public static class DocumentExtensions
    {
        private static Regex _wordRegex = new Regex(@"[^\W\d][\w]*(?<=\w)", RegexOptions.Compiled);
        private static Regex _previousWordRegx = new Regex(@".*(?=\.)");

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
                //TODO: just return word not segment, see the impact of returning string here
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

        public static string GetPreviousWord(this TextArea textArea)
        {
            DocumentLine line = textArea.Document.GetLineByNumber(textArea.Caret.Line);

            if (line.Length == 0)
                return null;

            MatchCollection matches = _previousWordRegx.Matches(textArea.Document.GetText(line)); int lineCaretPosition = textArea.Caret.Offset - line.Offset;
            foreach (Match match in matches)
            {
                var previousStatement = Regex.Replace(match.Captures[0].ToString(), @"\t|\n|\r", "");
                var statementArray = previousStatement.Split(new [] {"."}, StringSplitOptions.RemoveEmptyEntries);
                return statementArray[statementArray.Length - 1];
            }

            return null;
        }

        public static void CommentSelectedLines(this TextEditor editor)
        {
            TextDocument document = editor.Document;
            DocumentLine start = document.GetLineByOffset(editor.SelectionStart);
            DocumentLine end = document.GetLineByOffset(editor.SelectionStart + editor.SelectionLength);

            using (document.RunUpdate())
            {
                var line = start;
                while (line.LineNumber <= end.LineNumber)
                {
                    document.Insert(line.Offset, "//");
                    if (line.NextLine == null) break;
                    line = line.NextLine;
                }
            }
        }

        public static void UncommentSelectedLines(this TextEditor editor)
        {
            TextDocument document = editor.Document;
            DocumentLine start = document.GetLineByOffset(editor.SelectionStart);
            DocumentLine end = document.GetLineByOffset(editor.SelectionStart + editor.SelectionLength);

            using (document.RunUpdate())
            {
                var line = start;
                while (line.LineNumber <= end.LineNumber)
                {
                    if (document.GetText(line.Offset, 2) == "//")
                    {
                        document.Remove(line.Offset, 2);
                    }
                    if (line.NextLine == null) break;
                    line = line.NextLine;
                }
            }
        }
    }
}
