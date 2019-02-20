using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.DataAccess;
using c3IDE.Managers;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for InstanceWindow.xaml
    /// </summary>
    public partial class InstanceWindow : UserControl,IWindow
    {
        public string DisplayName { get; set; } = "Instance";
        private CompletionWindow completionWindow;

        /// <summary>
        /// instance window constructor
        /// </summary>
        public InstanceWindow()
        {
            InitializeComponent();

            EditTimeInstanceTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            EditTimeInstanceTextEditor.TextArea.TextEntered += EditTimeInstanceTextEditor_TextEntered;
            RunTimeInstanceTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            RunTimeInstanceTextEditor.TextArea.TextEntered += RunTimeInstanceTextEditor_TextEntered;

            EditTimeInstanceTextEditor.Options.EnableHyperlinks = false;
            EditTimeInstanceTextEditor.Options.EnableEmailHyperlinks = false;
            RunTimeInstanceTextEditor.Options.EnableHyperlinks = false;
            RunTimeInstanceTextEditor.Options.EnableEmailHyperlinks = false;
        }

        /// <summary>
        /// handles when the instance window gets focus
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(EditTimeInstanceTextEditor, Syntax.Javascript);
            ThemeManager.SetupTextEditor(RunTimeInstanceTextEditor, Syntax.Javascript);

            if (AddonManager.CurrentAddon != null)
            {
                EditTimeInstanceTextEditor.Text = AddonManager.CurrentAddon.TypeEditTime;
                RunTimeInstanceTextEditor.Text = AddonManager.CurrentAddon.TypeRunTime;
            }
        }

        /// <summary>
        /// handles when the instance window loses focus
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.InstanceEditTime = EditTimeInstanceTextEditor.Text;
                AddonManager.CurrentAddon.InstanceRunTime = RunTimeInstanceTextEditor.Text;
                AddonManager.SaveCurrentAddon();
            }
        }

        //todo:finish here
        /// <summary>
        /// clears all inputs from instance window
        /// </summary>
        public void Clear()
        {
            EditTimeInstanceTextEditor.Text = string.Empty;
            RunTimeInstanceTextEditor.Text = string.Empty;
        }

        //editor events
        private void EditTimeInstanceTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(EditTimeInstanceTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(EditTimeInstanceTextEditor, e.Text))
            {
                if (e.Text == ".")
                {
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"edittime_instance_script").ToList();
                    if (data.Any())
                    {
                        ShowCompletion(EditTimeInstanceTextEditor.TextArea, data);
                    }
                }
                else
                {
                    //figure out word segment
                    var segment = EditTimeInstanceTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = EditTimeInstanceTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"edittime_instance_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(EditTimeInstanceTextEditor.TextArea, data);
                    }
                }
            }
        }

        private void RunTimeInstanceTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(RunTimeInstanceTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(RunTimeInstanceTextEditor, e.Text))
            {
                if (e.Text == ".")
                {
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"runtime_instance_script").ToList();
                    if (data.Any())
                    {
                        ShowCompletion(RunTimeInstanceTextEditor.TextArea, data);
                    }
                }
                else
                {
                    //figure out word segment
                    var segment = RunTimeInstanceTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = RunTimeInstanceTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"runtime_instance_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(RunTimeInstanceTextEditor.TextArea, data);
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
            else if (e.Key == Key.F1)
            {
                //AppData.Insatnce.GlobalSave(false);
                Searcher.Insatnce.UpdateFileIndex("edittime_instance.js", EditTimeInstanceTextEditor.Text, AppData.Insatnce.MainWindow._instanceWindow);
                Searcher.Insatnce.UpdateFileIndex("runtime_instance.js", RunTimeInstanceTextEditor.Text, AppData.Insatnce.MainWindow._instanceWindow);
                var editor = ((TextEditor)sender);
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
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
