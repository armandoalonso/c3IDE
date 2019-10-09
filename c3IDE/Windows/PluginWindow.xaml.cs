using System;
using System.Collections.Generic;
using System.ComponentModel;
using c3IDE.DataAccess;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Folding;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Search;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for PluginWindow.xaml
    /// </summary>
    public partial class PluginWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Plugin";
        private CompletionWindow completionWindow;
        private FoldingManager edittimeFoldingManager, runtimeFoldingManager;
        private BraceFoldingStrategy folding;
        private SearchPanel edittimePanel, runtimePanel;

        /// <summary>
        /// plugin window constuctor
        /// </summary>
        public PluginWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            EditTimePluginTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            EditTimePluginTextEditor.TextArea.TextEntered += EditTimePluginTextEditor_TextEntered;
            RunTimePluginTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            RunTimePluginTextEditor.TextArea.TextEntered += RunTimePluginTextEditor_TextEntered;

            EditTimePluginTextEditor.Options.EnableHyperlinks = false;
            EditTimePluginTextEditor.Options.EnableEmailHyperlinks = false;
            RunTimePluginTextEditor.Options.EnableHyperlinks = false;
            RunTimePluginTextEditor.Options.EnableEmailHyperlinks = false;

            folding = new BraceFoldingStrategy();
            edittimeFoldingManager = FoldingManager.Install(EditTimePluginTextEditor.TextArea);
            runtimeFoldingManager = FoldingManager.Install(RunTimePluginTextEditor.TextArea);
            folding.UpdateFoldings(edittimeFoldingManager, EditTimePluginTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimePluginTextEditor.Document);

            //setip ctrl-f to single page code find
            edittimePanel = SearchPanel.Install(EditTimePluginTextEditor);
            runtimePanel = SearchPanel.Install(RunTimePluginTextEditor);
        }

        /// <summary>
        /// handles when the plugin window gets the focus
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(EditTimePluginTextEditor, Syntax.Javascript);
            ThemeManager.SetupTextEditor(RunTimePluginTextEditor, Syntax.Javascript);
            ThemeManager.SetupSearchPanel(edittimePanel, runtimePanel);

            if (AddonManager.CurrentAddon != null)
            {
                TitleTab.Header = AddonManager.CurrentAddon.Type == PluginType.Behavior ? "Behavior.js" : "Plugin.js";
                EditTimePluginTextEditor.Text = AddonManager.CurrentAddon.PluginEditTime;
                RunTimePluginTextEditor.Text = AddonManager.CurrentAddon.PluginRunTime;
            }

            folding.UpdateFoldings(edittimeFoldingManager, EditTimePluginTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimePluginTextEditor.Document);
        }

        /// <summary>
        /// handles when the plugin windows loses focus
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.PluginEditTime = EditTimePluginTextEditor.Text;
                AddonManager.CurrentAddon.PluginRunTime = RunTimePluginTextEditor.Text;
                AddonManager.SaveCurrentAddon();
            }

            folding.UpdateFoldings(edittimeFoldingManager, EditTimePluginTextEditor.Document);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimePluginTextEditor.Document);
        }

        /// <summary>
        /// clears all the input
        /// </summary>
        public void Clear()
        {
            EditTimePluginTextEditor.Text = string.Empty;
            RunTimePluginTextEditor.Text = string.Empty;
        }

        public void ChangeTab(string tab, int lineNum)
        {

        }

        /// <summary>
        /// handles auto completion and parsing of edit time plugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTimePluginTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(EditTimePluginTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(EditTimePluginTextEditor, e.Text))
            {
                if (e.Text == ".")
                {
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"edittime_plugin_script").ToList();
                    if (data.Any())
                    {
                        data.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.Ordinal));
                        ShowCompletion(EditTimePluginTextEditor.TextArea, data);
                    }
                }
                else
                {
                    //figure out word segment
                    var segment = EditTimePluginTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = EditTimePluginTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"edittime_plugin_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        data.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.Ordinal));
                        ShowCompletion(EditTimePluginTextEditor.TextArea, data);
                    }
                }
            }

            folding.UpdateFoldings(edittimeFoldingManager, EditTimePluginTextEditor.Document);
        }

        /// <summary>
        /// handles auto completion and parsing of run time plugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunTimePluginTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(RunTimePluginTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(RunTimePluginTextEditor, e.Text))
            {
                if (e.Text == ".")
                {
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"runtime_plugin_script").ToList();
                    if (data.Any())
                    {
                        data.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.Ordinal));
                        ShowCompletion(RunTimePluginTextEditor.TextArea, data);
                    }
                }
                else
                {
                    //figure out word segment
                    var segment = RunTimePluginTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = RunTimePluginTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"runtime_plugin_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        data.Sort((x, y) => string.Compare(x.Text, y.Text, StringComparison.Ordinal));
                        ShowCompletion(RunTimePluginTextEditor.TextArea, data);
                    }
                }
            }

            folding.UpdateFoldings(runtimeFoldingManager, RunTimePluginTextEditor.Document);
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
        /// handles keyboard shortcuts
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
            else if(e.Key == Key.F1)
            {
                //AppData.Insatnce.GlobalSave(false);
                Searcher.Insatnce.UpdateFileIndex("edittime_plugin.js", EditTimePluginTextEditor.Text, ApplicationWindows.PluginWindow);
                Searcher.Insatnce.UpdateFileIndex("runtime_plugin.js", RunTimePluginTextEditor.Text, ApplicationWindows.PluginWindow);
                var editor = ((TextEditor) sender);
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
            else if (e.Key == Key.F5)
            {
                WindowManager.MainWindow.Save(true, true);
            }
        }

        /// <summary>
        /// this builds the autocompletion window and displays it
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
        /// display the new property child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertNewProperty_OnClick(object sender, RoutedEventArgs e)
        {
            NewPropertyWindow.IsOpen = true;
            PropertyIdText.Text = "test-property";
            PropertyTypeDropdown.Text = "text";
            EditTimePluginTab.IsSelected = true;
        }

        /// <summary>
        /// handles saving and creating new properties from the new property child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            var id = PropertyIdText.Text.Replace(" ", "-");
            var type = PropertyTypeDropdown.Text;
            string template;

            //check for duplicate property id
            var propertyRegex = new Regex(@"\n?\t?\t?\t?\t?new SDK[.]PluginProperty\(\""(?<type>\w+)\"",(\s|\"")+(?<id>(\w|[-])+)\"",.*\)[,]?");
            var propertyMatches = propertyRegex.Matches(EditTimePluginTextEditor.Text);

            foreach (Match propertyMatch in propertyMatches)
            {
                if (propertyMatch.Groups["id"].ToString() == id)
                {
                    NotificationManager.PublishErrorNotification("cannot have duplicate property id.");
                    return;
                }
            }

            //removed existing properties
            EditTimePluginTextEditor.Text = propertyRegex.Replace(EditTimePluginTextEditor.Text, string.Empty);

            //get template
            switch (type)
            {
                case "integer":
                case "float":
                case "percent":
                    template = $"\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", 0)";
                    break;

                case "check":
                    template = $"\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", true)";
                    break;

                case "color":
                    template = $"\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", {{\"initialValue\": [1,0,0]}} )";
                    break;

                case "combo":
                    template = $"\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", {{\"items\":[\"item1\", \"item2\", \"item3\"], \"initialValue\": \"item1\"}})";
                    break;

                case "group":
                    template = $"\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"value\")";
                    break;

                default:
                    template = $"\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", \"value\")";
                    break;
            }

            var propList = new List<string>();
            foreach (Match propertyMatch in propertyMatches)
            {
                propList.Add($"{propertyMatch.Value.TrimEnd(',')}");
            }
            propList.Add(template);

            EditTimePluginTextEditor.Text = EditTimePluginTextEditor.Text.Replace("this._info.SetProperties([", $"this._info.SetProperties([{string.Join(",", propList)}");
            NewPropertyWindow.IsOpen = false;
        }

        /// <summary>
        /// context menu generates a new file dependency section based on included 3rd party files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateFileDependency_OnClick(object sender, RoutedEventArgs e)
        {
            EditTimePluginTab.IsSelected = true;

            var content = string.Join("\n", AddonManager.CurrentAddon.ThirdPartyFiles.Values.Where(x => x.FileType != "dom-side-script").Select(x => x.PluginTemplate));
            if (string.IsNullOrWhiteSpace(content))
            {
                //NotificationManager.PublishErrorNotification("no file dependecies found");
            }
            else
            {
                //var template = $@"this._info.AddFileDependency({content});";
                EditTimePluginTextEditor.Text =
                    FormatHelper.Insatnce.Javascript(EditTimePluginTextEditor.Text.Replace("SDK.Lang.PopContext(); //.properties", $"{content}\n\nSDK.Lang.PopContext(); //.properties"));
            }

            //add dom side scripts
            var domSideScripts = string.Join("\n", AddonManager.CurrentAddon.ThirdPartyFiles.Values.Where(x => x.FileType == "dom-side-script").Select(x => x.PluginTemplate));
            if(string.IsNullOrWhiteSpace(domSideScripts))
            {
               //NotificationManager.PublishErrorNotification("no dom side scripts found");
            }
            else
            {
                EditTimePluginTextEditor.Text =
                    FormatHelper.Insatnce.Javascript(EditTimePluginTextEditor.Text.Replace("SDK.Lang.PushContext(\".properties\");", $"{domSideScripts}\n\nSDK.Lang.PushContext(\".properties\");"));
            }
        }

        /// <summary>
        /// formats the edit time plugin as javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJavascriptEdittime_OnClick(object sender, RoutedEventArgs e)
        {
            EditTimePluginTextEditor.Text = FormatHelper.Insatnce.Javascript(EditTimePluginTextEditor.Text);
            folding.UpdateFoldings(edittimeFoldingManager, EditTimePluginTextEditor.Document);
        }

        /// <summary>
        /// formats the run time plugin as javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJavascriptRuntime_OnClick(object sender, RoutedEventArgs e)
        {
           RunTimePluginTextEditor.Text = FormatHelper.Insatnce.Javascript(RunTimePluginTextEditor.Text);
            folding.UpdateFoldings(runtimeFoldingManager, RunTimePluginTextEditor.Document);
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
            Searcher.Insatnce.UpdateFileIndex("edittime_plugin.js", EditTimePluginTextEditor.Text, ApplicationWindows.PluginWindow);
            Searcher.Insatnce.UpdateFileIndex("runtime_plugin.js", RunTimePluginTextEditor.Text, ApplicationWindows.PluginWindow);

            MenuItem mnu = sender as MenuItem;
            TextEditor editor = null;

            if (mnu != null)
            {
                editor = ((ContextMenu)mnu.Parent).PlacementTarget as TextEditor;
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }

        private void CommentSelection(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            TextEditor editor = null;

            if (mnu != null)
            {
                editor = ((ContextMenu)mnu.Parent).PlacementTarget as TextEditor;
                editor.CommentSelectedLines();
            }
        }

        private void UncommentSelection(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            TextEditor editor = null;

            if (mnu != null)
            {
                editor = ((ContextMenu)mnu.Parent).PlacementTarget as TextEditor;
                editor.UncommentSelectedLines();
            }
        }

        private void Compile_OnClick(object sender, RoutedEventArgs e)
        {
            WindowManager.MainWindow.Save(true, true);
        }
    }
}