using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;

namespace c3IDE.Utilities.Helpers
{
    public class TextEditorHelper : Singleton<TextEditorHelper>
    {
        public bool MatchSymbol(TextEditor editor, string symbol)
        {
            switch (symbol)
            {
                case "{":
                    editor.Document.Insert(editor.TextArea.Caret.Offset, "}");
                    editor.TextArea.Caret.Offset--;
                    return true;

                case "\"":
                    editor.Document.Insert(editor.TextArea.Caret.Offset, "\"");
                    editor.TextArea.Caret.Offset--;
                    return true;

                case "[":
                    editor.Document.Insert(editor.TextArea.Caret.Offset, "]");
                    editor.TextArea.Caret.Offset--;
                    return true;

                case "(":
                    editor.Document.Insert(editor.TextArea.Caret.Offset, ")");
                    editor.TextArea.Caret.Offset--;
                    return true;

                case "'":
                    editor.Document.Insert(editor.TextArea.Caret.Offset, "'");
                    editor.TextArea.Caret.Offset--;
                    return true;
                default:
                    return false;
            }
        }
    }
}
