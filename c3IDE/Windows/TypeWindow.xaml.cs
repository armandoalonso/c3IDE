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
using c3IDE.Models;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Folding;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using c3IDE.Utilities.ThemeEngine;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for TypeWindow.xaml
    /// </summary>
    public partial class TypeWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Type";
        private CompletionWindow completionWindow;
        private FoldingManager edittimeFoldingManager, runtimeFoldingManager;
        private BraceFoldingStrategy folding;

        /// <summary>
        /// constructor for type window
        /// </summary>
        public TypeWindow()
        {
            InitializeComponent();

            EditTimeTypeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            EditTimeTypeTextEditor.TextArea.TextEntered += EditTimeTypeTextEditor_TextEntered;
            RunTimeTypeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            RunTimeTypeTextEditor.TextArea.TextEntered += RunTimeTypeTextEditor_TextEntered;

            EditTimeTypeTextEditor.Options.EnableHyperlinks = false;
            EditTimeTypeTextEditor.Options.EnableEmailHyperlinks = false;
            RunTimeTypeTextEditor.Options.EnableHyperlinks = false;
            RunTimeTypeTextEditor.Options.EnableEmailHyperlinks = false;

            folding = new BraceFoldingStrategy();
            edittimeFoldingManager = FoldingManager.Install(EditTimeTypeTextEditor.TextArea);
            runtimeFoldingManager = FoldingManager.Install(RunTimeTypeTextEditor.TextArea);
            folding.UpdateFoldings(edittimeFoldingManager, EditTimeTypeTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimeTypeTextEditor.Document);
        }

        /// <summary>
        /// handles the type window getting focus
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(EditTimeTypeTextEditor, Syntax.Javascript);
            ThemeManager.SetupTextEditor(RunTimeTypeTextEditor, Syntax.Javascript);

            if (AddonManager.CurrentAddon != null)
            {
                EditTimeTypeTextEditor.Text = AddonManager.CurrentAddon.TypeEditTime;
                RunTimeTypeTextEditor.Text = AddonManager.CurrentAddon.TypeRunTime;
            }

            folding.UpdateFoldings(edittimeFoldingManager, EditTimeTypeTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimeTypeTextEditor.Document);
        }

        /// <summary>
        /// handles the type window losing focus
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.TypeEditTime = EditTimeTypeTextEditor.Text;
                AddonManager.CurrentAddon.TypeRunTime = RunTimeTypeTextEditor.Text;
                AddonManager.SaveCurrentAddon();
            }

            folding.UpdateFoldings(edittimeFoldingManager, EditTimeTypeTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimeTypeTextEditor.Document);
        }

        /// <summary>
        /// clears all the inputs in the type window
        /// </summary>
        public void Clear()
        {
            EditTimeTypeTextEditor.Text = string.Empty;
            RunTimeTypeTextEditor.Text = string.Empty;
        }

        public void ChangeTab(string tab, int lineNum)
        {

        }

        /// <summary>
        /// handles auto completion and parsing edit time type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            folding.UpdateFoldings(edittimeFoldingManager, EditTimeTypeTextEditor.Document);
        }

        /// <summary>
        /// handles autocompletiona dn parsing of run time type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            folding.UpdateFoldings(runtimeFoldingManager, RunTimeTypeTextEditor.Document);
        }

        /// <summary>
        /// this handles when to insert the value being used by the autocompletion window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditor_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]) && !char.IsWhiteSpace(e.Text[0]) && !char.IsSymbol(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }

        /// <summary>
        /// this handles keyboard shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Searcher.Insatnce.UpdateFileIndex("edittime_type.js", EditTimeTypeTextEditor.Text, ApplicationWindows.InstanceWindow);
                Searcher.Insatnce.UpdateFileIndex("runtime_type.js", RunTimeTypeTextEditor.Text, ApplicationWindows.InstanceWindow);
                var editor = ((TextEditor)sender);
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }

        /// <summary>
        /// this shows the suto completion window
        /// </summary>
        /// <param name="textArea"></param>
        /// <param name="completionList"></param>
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
    
        /// <summary>
        /// this formats the run time type as javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJavascriptRuntime_OnClick(object sender, RoutedEventArgs e)
        {
            EditTimeTypeTextEditor.Text = FormatHelper.Insatnce.Javascript(EditTimeTypeTextEditor.Text);
            folding.UpdateFoldings(edittimeFoldingManager, EditTimeTypeTextEditor.Document);
        }

        /// <summary>
        /// this formats the edit time type as javascript 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJavascriptEdittime_OnClick(object sender, RoutedEventArgs e)
        {
            RunTimeTypeTextEditor.Text = FormatHelper.Insatnce.Javascript(RunTimeTypeTextEditor.Text);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimeTypeTextEditor.Document);
        }

        private void EditTimeFoldAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var fold in edittimeFoldingManager.AllFoldings)
            {
                fold.IsFolded = true;
            }
        }

        private void EditTimeUnFoldAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var fold in edittimeFoldingManager.AllFoldings)
            {
                fold.IsFolded = false;
            }
        }

        private void RunTimeFoldAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var fold in runtimeFoldingManager.AllFoldings)
            {
                fold.IsFolded = true;
            }
        }

        private void RunTimeUnFoldAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var fold in runtimeFoldingManager.AllFoldings)
            {
                fold.IsFolded = false;
            }
        }
    }
}
