using System;
using System.Collections.Generic;
using System.ComponentModel;
using c3IDE.DataAccess;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.Models;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using c3IDE.Utilities.ThemeEngine;
using ICSharpCode.AvalonEdit;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for PluginWindow.xaml
    /// </summary>
    public partial class PluginWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Plugin";
        private CompletionWindow completionWindow;

        //ctor
        public PluginWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            //handle auto completion when entering text
            EditTimePluginTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            EditTimePluginTextEditor.TextArea.TextEntered += EditTimePluginTextEditor_TextEntered;
            EditTimePluginTextEditor.Options.EnableHyperlinks = false;
            EditTimePluginTextEditor.Options.EnableEmailHyperlinks = false;

            RunTimePluginTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            RunTimePluginTextEditor.TextArea.TextEntered += RunTimePluginTextEditor_TextEntered;
            RunTimePluginTextEditor.Options.EnableHyperlinks = false;
            RunTimePluginTextEditor.Options.EnableEmailHyperlinks = false;
        }

        //editor events
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
                        ShowCompletion(EditTimePluginTextEditor.TextArea, data);
                    }
                }
            }
        }

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
                        ShowCompletion(RunTimePluginTextEditor.TextArea, data);
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
            else if(e.Key == Key.F1)
            {
                var editor = ((TextEditor) sender);
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
        public void OnEnter()
        {
            TitleTab.Header = AppData.Insatnce.CurrentAddon.Type == PluginType.Behavior ? "Behavior.js" : "Plugin.js";

            AppData.Insatnce.SetupTextEditor(EditTimePluginTextEditor, Syntax.Javascript);
            AppData.Insatnce.SetupTextEditor(RunTimePluginTextEditor, Syntax.Javascript);

            EditTimePluginTextEditor.Text = AppData.Insatnce.CurrentAddon?.PluginEditTime;
            RunTimePluginTextEditor.Text = AppData.Insatnce.CurrentAddon?.PluginRunTime;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.PluginEditTime = EditTimePluginTextEditor.Text;
                AppData.Insatnce.CurrentAddon.PluginRunTime = RunTimePluginTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
        }

        public void Clear()
        {
            EditTimePluginTextEditor.Text = string.Empty;
            RunTimePluginTextEditor.Text = string.Empty;
        }

        //context menu
        private void InsertNewProperty_OnClick(object sender, RoutedEventArgs e)
        {
            NewPropertyWindow.IsOpen = true;
            PropertyIdText.Text = "test-property";
            PropertyTypeDropdown.Text = "text";
        }

        private void GenerateFileDependency_OnClick(object sender, RoutedEventArgs e)
        {
            var content = string.Join(",\n", AppData.Insatnce.CurrentAddon.ThirdPartyFiles.Values.Select(x => x.PluginTemplate));
            var template = $@"this._info.AddFileDependency({content});";

            EditTimePluginTextEditor.Text =
                FormatHelper.Insatnce.Javascript(EditTimePluginTextEditor.Text.Replace("SDK.Lang.PopContext(); //.properties", $"{template}\n\nSDK.Lang.PopContext(); //.properties"));
        }

        private void FormatJavascriptEdittime_OnClick(object sender, RoutedEventArgs e)
        {
            EditTimePluginTextEditor.Text = FormatHelper.Insatnce.Javascript(EditTimePluginTextEditor.Text);
        }

        private void FormatJavascriptRuntime_OnClick(object sender, RoutedEventArgs e)
        {
           RunTimePluginTextEditor.Text = FormatHelper.Insatnce.Javascript(RunTimePluginTextEditor.Text);
        }

        //button clicks
        private void AddPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            var id = PropertyIdText.Text;
            var type = PropertyTypeDropdown.Text;
            string template;

            //check for duplicate property id
            var propertyRegex = new Regex(@"new SDK[.]PluginProperty\(\""(?<type>\w+)\""\W+\""(?<id>\w+[-]?\w+)\""");
            var propertyMatches = propertyRegex.Matches(EditTimePluginTextEditor.Text);
            var firstProperty = propertyMatches.Count == 0;

            foreach (Match propertyMatch in propertyMatches)
            {
                if (propertyMatch.Groups["id"].ToString() == id)
                {
                    AppData.Insatnce.ErrorMessage("cannot have duplicate property id.");
                    return;
                }
            }

            var comma = firstProperty ? string.Empty : ",";
            switch (type)
            {
                case "integer":
                case "float":
                case "percent":
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", 0){comma}";
                    break;

                case "check":
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", true){comma}";
                    break;

                case "color":
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", {{\"initialValue\": [1,0,0]}} ){comma}";
                    break;

                case "combo":
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", {{\"items\":[\"item1\", \"item2\", \"item3\"]}}, \"initialValue\": \"item1\"){comma}";
                    break;

                default:
                    template = $"this._info.SetProperties([\n\t\t\t\tnew SDK.PluginProperty(\"{type}\", \"{id}\", \"value\"){comma}";
                    break;
            }

            EditTimePluginTextEditor.Text = EditTimePluginTextEditor.Text.Replace("this._info.SetProperties([", template);
            NewPropertyWindow.IsOpen = false;
        }
    }
}