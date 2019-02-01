using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;

namespace c3IDE.Utilities.Search
{
    public class Searcher : Singleton<Searcher>
    {
        private int lastUsedIndex = 0;
        private int lastSearchIndex = 0;

        public void Find(TextEditor textEditor, string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                lastUsedIndex = 0;
                return;
            }

            string editorText = textEditor.Text;

            if (string.IsNullOrEmpty(editorText))
            {
                lastUsedIndex = 0;
                return;
            }

            if (lastUsedIndex >= searchQuery.Count())
            {
                lastUsedIndex = 0;
            }

            int nIndex = editorText.IndexOf(searchQuery, lastUsedIndex);
            if (nIndex != -1)
            {
                var area = textEditor.TextArea;
                textEditor.Select(nIndex, searchQuery.Length);
                lastUsedIndex = nIndex + searchQuery.Length;
            }
            else
            {
                lastUsedIndex = 0;
            }
        }

        public void Replace(TextEditor textEditor, string s, string replacement, bool selectedonly)
        {
            int nIndex = -1;
            if (selectedonly)
            {
                nIndex = textEditor.Text.IndexOf(s, textEditor.SelectionStart, textEditor.SelectionLength);
            }
            else
            {
                nIndex = textEditor.Text.IndexOf(s);
            }

            if (nIndex != -1)
            {
                textEditor.Document.Replace(nIndex, s.Length, replacement);


                textEditor.Select(nIndex, replacement.Length);
            }
            else
            {
                lastSearchIndex = 0;
            }
        }
    }
}
