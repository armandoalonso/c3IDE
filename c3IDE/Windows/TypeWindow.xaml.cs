using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using c3IDE.Utilities.Helpers;
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
        //properties
        public string DisplayName { get; set; } = "Type";
        private CompletionWindow completionWindow;

        //ctor
        public TypeWindow()
        {
            InitializeComponent();
            EditTimeTypeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            EditTimeTypeTextEditor.TextArea.TextEntered += EditTimeTypeTextEditor_TextEntered;
            RunTimeTypeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            RunTimeTypeTextEditor.TextArea.TextEntered += RunTimeTypeTextEditor_TextEntered;
        }

        //editor events
        private void EditTimeTypeTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var tokenList = JavascriptParser.Insatnce.ParseJavascriptDocument(EditTimeTypeTextEditor.Text, CodeType.EditTimeJavascript);
            var methodsTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(EditTimeTypeTextEditor.Text);
            var allTokens =
                JavascriptParser.Insatnce.DecorateMethodInterfaces(tokenList, methodsTokens,
                    CodeType.EditTimeJavascript);

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
                    var methodsData = CodeCompletionFactory.Insatnce
                        .GetCompletionData(allTokens, CodeType.EditTimeJavascript)
                        .Where(x => x.Type == CompletionType.Methods || x.Type == CompletionType.Modules || x.Type == CompletionType.Misc);
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
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.EditTimeJavascript).Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(EditTimeTypeTextEditor.TextArea, data);
                    }
                    break;
            }
        }

        private void RunTimeTypeTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var tokenList = JavascriptParser.Insatnce.ParseJavascriptDocument(RunTimeTypeTextEditor.Text, CodeType.RuntimeJavascript);
            var methodsTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(RunTimeTypeTextEditor.Text);
            var allTokens = JavascriptParser.Insatnce.DecorateMethodInterfaces(tokenList, methodsTokens, CodeType.RuntimeJavascript);

            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    RunTimeTypeTextEditor.Document.Insert(RunTimeTypeTextEditor.TextArea.Caret.Offset, "}");
                    RunTimeTypeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "\"":
                    RunTimeTypeTextEditor.Document.Insert(RunTimeTypeTextEditor.TextArea.Caret.Offset, "\"");
                    RunTimeTypeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "[":
                    RunTimeTypeTextEditor.Document.Insert(RunTimeTypeTextEditor.TextArea.Caret.Offset, "]");
                    RunTimeTypeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "(":
                    RunTimeTypeTextEditor.Document.Insert(RunTimeTypeTextEditor.TextArea.Caret.Offset, ")");
                    RunTimeTypeTextEditor.TextArea.Caret.Offset--;
                    return;
                case ".":
                    var methodsData = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.RuntimeJavascript)
                        .Where(x => x.Type == CompletionType.Methods || x.Type == CompletionType.Modules || x.Type == CompletionType.Misc);
                    ShowCompletion(RunTimeTypeTextEditor.TextArea, methodsData.ToList());
                    break;
                default:
                    //figure out word segment
                    var segment = RunTimeTypeTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = RunTimeTypeTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.RuntimeJavascript).Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(RunTimeTypeTextEditor.TextArea, data);
                    }
                    break;
            }
        }

        private void TextEditor_TextEntering(object sender, TextCompositionEventArgs e)
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

        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && completionWindow != null && completionWindow.CompletionList.SelectedItem == null)
            {
                e.Handled = true;
                completionWindow.CompletionList.ListBox.SelectedIndex = 0;
                completionWindow.CompletionList.RequestInsertion(EventArgs.Empty);
            }
            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AppData.Insatnce.GlobalSave();
            }
        }

        //completion window
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
            completionWindow.CompletionList.ListBox.Items.SortDescriptions.Add(new SortDescription("Type", ListSortDirection.Ascending));

            completionWindow.Show();
            completionWindow.Closed += delegate { completionWindow = null; };
        }

        //window states
        public void OnEnter()
        {
            AppData.Insatnce.SetupTextEditor(EditTimeTypeTextEditor);
            AppData.Insatnce.SetupTextEditor(RunTimeTypeTextEditor);

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



        //context menu
        private void FormatJavascriptRuntime_OnClick(object sender, RoutedEventArgs e)
        {
            EditTimeTypeTextEditor.Text = FormatHelper.Insatnce.Javascript(EditTimeTypeTextEditor.Text);
        }

        private void FormatJavascriptEdittime_OnClick(object sender, RoutedEventArgs e)
        {
            RunTimeTypeTextEditor.Text = FormatHelper.Insatnce.Javascript(RunTimeTypeTextEditor.Text);
        }
    }
}
