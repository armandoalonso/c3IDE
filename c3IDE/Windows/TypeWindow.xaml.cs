using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using c3IDE.DataAccess;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for TypeWindow.xaml
    /// </summary>
    public partial class TypeWindow : UserControl, IWindow
    {
        private CompletionWindow completionWindow;
        public TypeWindow()
        {
            InitializeComponent();
            EditTimeTypeTextEditor.TextArea.TextEntering += EditTimeTypeTextEditor_TextEntering;
            EditTimeTypeTextEditor.TextArea.TextEntered += EditTimeTypeTextEditor_TextEntered;
        }

        private void EditTimeTypeTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var tokenList = JavascriptParser.Insatnce.ParseJavascriptDocument(EditTimeTypeTextEditor.Text);
            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    EditTimeTypeTextEditor.Document.Insert(EditTimeTypeTextEditor.TextArea.Caret.Offset, "}");
                    EditTimeTypeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "\"":
                    EditTimeTypeTextEditor.Document.Insert(EditTimeTypeTextEditor.TextArea.Caret.Offset, "\"");
                    EditTimeTypeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "[":
                    EditTimeTypeTextEditor.Document.Insert(EditTimeTypeTextEditor.TextArea.Caret.Offset, "]");
                    EditTimeTypeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "(":
                    EditTimeTypeTextEditor.Document.Insert(EditTimeTypeTextEditor.TextArea.Caret.Offset, ")");
                    EditTimeTypeTextEditor.TextArea.Caret.Offset--;
                    return;
                case ".":
                    var methodsData = CodeCompletionFactory.Insatnce.GetCompletionData(tokenList, CodeType.EdittimeJavascript).Where(x => x.Type == CompletionType.Methods);
                    ShowCompletion(EditTimeTypeTextEditor.TextArea, methodsData.ToList());
                    break;
                default:
                    //figure out word segment
                    var segment = EditTimeTypeTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = EditTimeTypeTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(tokenList, CodeType.EdittimeJavascript).Where(x => x.Text.ToLower().Contains(text)).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(EditTimeTypeTextEditor.TextArea, data);
                    }
                    break;
            }
        }

        private void EditTimeTypeTextEditor_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        private void ShowCompletion(TextArea textArea, List<GenericCompletionItem> completionList)
        {
            //if any data matches show completion list
            completionWindow = new CompletionWindow(textArea)
            {
                //overwrite color due to global style
                Foreground = new SolidColorBrush(Colors.Black)
            };

            var completionData = completionWindow.CompletionList.CompletionData;
            CodeCompletionDecorator.Insatnce.Decorate(ref completionData, completionList); ;
            completionWindow.Width = 250;

            completionWindow.Show();
            completionWindow.Closed += delegate { completionWindow = null; };
        }


        public string DisplayName { get; set; } = "Type";
        public void OnEnter()
        {
            EditTimeTypeTextEditor.Text = AppData.Insatnce.CurrentAddon?.TypeEditTime;
            RunTimeTypeTextEditor.Text = AppData.Insatnce.CurrentAddon?.TypeRunTime;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.TypeEditTime = EditTimeTypeTextEditor.Text;
                AppData.Insatnce.CurrentAddon.TypeRunTime = RunTimeTypeTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
        }
    }
}
