using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.Managers;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Folding;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Search;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for InstanceWindow.xaml
    /// </summary>
    public partial class InstanceWindow : UserControl,IWindow
    {
        public string DisplayName { get; set; } = "Instance";
        private CompletionWindow completionWindow;
        private FoldingManager edittimeFoldingManager, runtimeFoldingManager;
        private BraceFoldingStrategy folding;
        private SearchPanel edittimePanel, runtimePanel;

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

            folding = new BraceFoldingStrategy();
            edittimeFoldingManager = FoldingManager.Install(EditTimeInstanceTextEditor.TextArea);
            runtimeFoldingManager = FoldingManager.Install(RunTimeInstanceTextEditor.TextArea);
            folding.UpdateFoldings(edittimeFoldingManager, EditTimeInstanceTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimeInstanceTextEditor.Document);

            //setip ctrl-f to single page code find
            runtimePanel = SearchPanel.Install(RunTimeInstanceTextEditor);
            edittimePanel = SearchPanel.Install(EditTimeInstanceTextEditor);
            
        }

        /// <summary>
        /// handles when the instance window gets focus
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(EditTimeInstanceTextEditor, Syntax.Javascript);
            ThemeManager.SetupTextEditor(RunTimeInstanceTextEditor, Syntax.Javascript);
            ThemeManager.SetupSearchPanel(edittimePanel, runtimePanel);

            if (AddonManager.CurrentAddon != null)
            {
                EditTimeInstanceTextEditor.Text = AddonManager.CurrentAddon.InstanceEditTime;
                RunTimeInstanceTextEditor.Text = AddonManager.CurrentAddon.InstanceRunTime;
            }

            folding.UpdateFoldings(edittimeFoldingManager, EditTimeInstanceTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimeInstanceTextEditor.Document);
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

            folding.UpdateFoldings(edittimeFoldingManager, EditTimeInstanceTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimeInstanceTextEditor.Document);
        }

        /// <summary>
        /// clears all inputs from instance window
        /// </summary>
        public void Clear()
        {
            EditTimeInstanceTextEditor.Text = string.Empty;
            RunTimeInstanceTextEditor.Text = string.Empty;
        }

        public void ChangeTab(string tab, int lineNum)
        {

        }

        /// <summary>
        /// handles auto completion and parsing edit time instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        data.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.Ordinal));
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
                        data.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.Ordinal));
                        ShowCompletion(EditTimeInstanceTextEditor.TextArea, data);
                    }
                }
            }

            folding.UpdateFoldings(edittimeFoldingManager, EditTimeInstanceTextEditor.Document);
        }

        /// <summary>
        /// handles auto completion and parsing run time instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        data.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.Ordinal));
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
                        data.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.Ordinal));
                        ShowCompletion(RunTimeInstanceTextEditor.TextArea, data);
                    }
                }
            }

            folding.UpdateFoldings(runtimeFoldingManager, RunTimeInstanceTextEditor.Document);
        }

        /// <summary>
        /// this handles when to insert the value being used by the auto completion window
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
                Searcher.Insatnce.UpdateFileIndex("edittime_instance.js", EditTimeInstanceTextEditor.Text, ApplicationWindows.InstanceWindow);
                Searcher.Insatnce.UpdateFileIndex("runtime_instance.js", RunTimeInstanceTextEditor.Text, ApplicationWindows.InstanceWindow);
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
        /// this formats the edit time type as javascript 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJavascriptEdittime_OnClick(object sender, RoutedEventArgs e)
        {
            EditTimeInstanceTextEditor.Text = FormatHelper.Insatnce.Javascript(EditTimeInstanceTextEditor.Text);
            folding.UpdateFoldings(edittimeFoldingManager, EditTimeInstanceTextEditor.Document);
        }

        /// <summary>
        /// this formats the run time type as javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJavascriptRuntime_OnClick(object sender, RoutedEventArgs e)
        {
            RunTimeInstanceTextEditor.Text = FormatHelper.Insatnce.Javascript(RunTimeInstanceTextEditor.Text);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimeInstanceTextEditor.Document);
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

        private void RuntimeFoldAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var fold in runtimeFoldingManager.AllFoldings)
            {
                fold.IsFolded = true;
            }
        }

        private void RunttimeUnFoldAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var fold in runtimeFoldingManager.AllFoldings)
            {
                fold.IsFolded = false;
            }
        }

        private void FindGlobal_Click(object sender, RoutedEventArgs e)
        {
            //AppData.Insatnce.GlobalSave(false);
            Searcher.Insatnce.UpdateFileIndex("edittime_instance.js", EditTimeInstanceTextEditor.Text, ApplicationWindows.InstanceWindow);
            Searcher.Insatnce.UpdateFileIndex("runtime_instance.js", RunTimeInstanceTextEditor.Text, ApplicationWindows.InstanceWindow);

            MenuItem mnu = sender as MenuItem;
            TextEditor editor = null;

            if (mnu != null)
            {
                editor = ((ContextMenu)mnu.Parent).PlacementTarget as TextEditor;
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }
    }
}
