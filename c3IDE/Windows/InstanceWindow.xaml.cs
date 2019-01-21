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
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for InstanceWindow.xaml
    /// </summary>
    public partial class InstanceWindow : UserControl,IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Instance";
        private CompletionWindow completionWindow;

        //ctor
        public InstanceWindow()
        {
            InitializeComponent();
            EditTimeInstanceTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            EditTimeInstanceTextEditor.TextArea.TextEntered += EditTimeInstanceTextEditor_TextEntered;

            RunTimeInstanceTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            RunTimeInstanceTextEditor.TextArea.TextEntered += RunTimeInstanceTextEditor_TextEntered;
        }

        //editor events
        private void EditTimeInstanceTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var tokenList = JavascriptParser.Insatnce.ParseJavascriptDocument(EditTimeInstanceTextEditor.Text, CodeType.EdittimeJavascript);
            var methodsTokens = JavascriptParser.Insatnce.ParseJavascriptMethodCalls(EditTimeInstanceTextEditor.Text);
            var allTokens = JavascriptParser.Insatnce.DecorateMethodInterfaces(tokenList, methodsTokens, CodeType.EdittimeJavascript);

            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    EditTimeInstanceTextEditor.Document.Insert(EditTimeInstanceTextEditor.TextArea.Caret.Offset, "}");
                    EditTimeInstanceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "\"":
                    EditTimeInstanceTextEditor.Document.Insert(EditTimeInstanceTextEditor.TextArea.Caret.Offset, "\"");
                    EditTimeInstanceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "[":
                    EditTimeInstanceTextEditor.Document.Insert(EditTimeInstanceTextEditor.TextArea.Caret.Offset, "]");
                    EditTimeInstanceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "(":
                    EditTimeInstanceTextEditor.Document.Insert(EditTimeInstanceTextEditor.TextArea.Caret.Offset, ")");
                    EditTimeInstanceTextEditor.TextArea.Caret.Offset--;
                    return;
                case ".":
                    var methodsData = CodeCompletionFactory.Insatnce
                        .GetCompletionData(allTokens, CodeType.EdittimeJavascript)
                        .Where(x => x.Type == CompletionType.Methods || x.Type == CompletionType.Modules || x.Type == CompletionType.Misc);
                    ShowCompletion(EditTimeInstanceTextEditor.TextArea, methodsData.ToList());
                    break;
                default:
                    //figure out word segment
                    var segment = EditTimeInstanceTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = EditTimeInstanceTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.EdittimeJavascript)
                        .Where(x => x.Text.ToLower().Contains(text)).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(EditTimeInstanceTextEditor.TextArea, data);
                    }
                    break;
            }
        }

        private void RunTimeInstanceTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var tokenList = JavascriptParser.Insatnce.ParseJavascriptDocument(RunTimeInstanceTextEditor.Text, CodeType.RuntimeJavascript);
            var methodsTokens = JavascriptParser.Insatnce.ParseJavascriptMethodCalls(RunTimeInstanceTextEditor.Text);
            var allTokens = JavascriptParser.Insatnce.DecorateMethodInterfaces(tokenList, methodsTokens, CodeType.RuntimeJavascript);

            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    RunTimeInstanceTextEditor.Document.Insert(RunTimeInstanceTextEditor.TextArea.Caret.Offset, "}");
                    RunTimeInstanceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "\"":
                    RunTimeInstanceTextEditor.Document.Insert(RunTimeInstanceTextEditor.TextArea.Caret.Offset, "\"");
                    RunTimeInstanceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "[":
                    RunTimeInstanceTextEditor.Document.Insert(RunTimeInstanceTextEditor.TextArea.Caret.Offset, "]");
                    RunTimeInstanceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "(":
                    RunTimeInstanceTextEditor.Document.Insert(RunTimeInstanceTextEditor.TextArea.Caret.Offset, ")");
                    RunTimeInstanceTextEditor.TextArea.Caret.Offset--;
                    return;
                case ".":
                    var methodsData = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.RuntimeJavascript)
                        .Where(x => x.Type == CompletionType.Methods || x.Type == CompletionType.Modules || x.Type == CompletionType.Misc);
                    ShowCompletion(RunTimeInstanceTextEditor.TextArea, methodsData.ToList());
                    break;
                default:
                    //figure out word segment
                    var segment = RunTimeInstanceTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = RunTimeInstanceTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.RuntimeJavascript).Where(x => x.Text.ToLower().Contains(text)).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(RunTimeInstanceTextEditor.TextArea, data);
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
            EditTimeInstanceTextEditor.Text = AppData.Insatnce.CurrentAddon?.InstanceEditTime;
            RunTimeInstanceTextEditor.Text = AppData.Insatnce.CurrentAddon?.InstanceRunTime;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.InstanceEditTime = EditTimeInstanceTextEditor.Text;
                AppData.Insatnce.CurrentAddon.InstanceRunTime = RunTimeInstanceTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
        }

        //context menu
        private void FormatJavascriptEdittime_OnClick(object sender, RoutedEventArgs e)
        {
            EditTimeInstanceTextEditor.Text = FormatHelper.Insatnce.Javascript(EditTimeInstanceTextEditor.Text);
        }

        private void FormatJavascriptRuntime_OnClick(object sender, RoutedEventArgs e)
        {
            RunTimeInstanceTextEditor.Text = FormatHelper.Insatnce.Javascript(RunTimeInstanceTextEditor.Text);
        }
    }
}
