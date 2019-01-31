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
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using c3IDE.Utilities.ThemeEngine;

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
            EditTimeTypeTextEditor.Options.EnableHyperlinks = false;
            EditTimeTypeTextEditor.Options.EnableEmailHyperlinks = false;

            RunTimeTypeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            RunTimeTypeTextEditor.TextArea.TextEntered += RunTimeTypeTextEditor_TextEntered;
            RunTimeTypeTextEditor.Options.EnableHyperlinks = false;
            RunTimeTypeTextEditor.Options.EnableEmailHyperlinks = false;
        }

        //editor events
        private void EditTimeTypeTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(EditTimeTypeTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(EditTimeTypeTextEditor, e.Text))
            {
                if (e.Text == ".")
                {
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"edittime_type_script").ToList();
                    if (data.Any())
                    {
                        ShowCompletion(EditTimeTypeTextEditor.TextArea, data);
                    }
                }
                else
                {
                    //figure out word segment
                    var segment = EditTimeTypeTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = EditTimeTypeTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"edittime_type_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(EditTimeTypeTextEditor.TextArea, data);
                    }
                }
            }
        }

        private void RunTimeTypeTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(RunTimeTypeTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(RunTimeTypeTextEditor, e.Text))
            {
                if (e.Text == ".")
                {
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"runtime_type_script").ToList();
                    if (data.Any())
                    {
                        ShowCompletion(RunTimeTypeTextEditor.TextArea, data);
                    }
                }
                else
                {
                    //figure out word segment
                    var segment = RunTimeTypeTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = RunTimeTypeTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"runtime_type_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(RunTimeTypeTextEditor.TextArea, data);
                    }
                }
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
            AppData.Insatnce.SetupTextEditor(EditTimeTypeTextEditor, Syntax.Javascript);
            AppData.Insatnce.SetupTextEditor(RunTimeTypeTextEditor, Syntax.Javascript);

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

        public void Clear()
        {
            EditTimeTypeTextEditor.Text = string.Empty;
             RunTimeTypeTextEditor.Text = string.Empty;
        }

        public void SetupTheme(Theme t)
        {

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
