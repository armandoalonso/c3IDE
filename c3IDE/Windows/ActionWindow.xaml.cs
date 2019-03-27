using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.Managers;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Windows.Interfaces;
using c3IDE.Templates;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = c3IDE.Models.Action;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for ActionWindow.xaml
    /// </summary>
    public partial class ActionWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Actions";
        private Dictionary<string, Action> _actions;
        private Action _selectedAction;
        private CompletionWindow completionWindow;

        /// <summary>
        /// action window constructor
        /// </summary>
        public ActionWindow()
        {
            InitializeComponent();

            CodeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            CodeTextEditor.TextArea.TextEntered += CodeTextEditor_TextEntered;
            AceTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            AceTextEditor.TextArea.TextEntered += AceTextEditor_TextEntered;
            LanguageTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            LanguageTextEditor.TextArea.TextEntered += LanguageTextEditor_TextEntered;

            AceTextEditor.Options.EnableEmailHyperlinks = false;
            AceTextEditor.Options.EnableHyperlinks = false;
            CodeTextEditor.Options.EnableEmailHyperlinks = false;
            CodeTextEditor.Options.EnableHyperlinks = false;
            LanguageTextEditor.Options.EnableEmailHyperlinks = false;
            LanguageTextEditor.Options.EnableHyperlinks = false;
        }

        /// <summary>
        /// handles the action window getting focus
        /// </summary>
        public void OnEnter()
        {
           ThemeManager.SetupTextEditor(AceTextEditor, Syntax.Json);
           ThemeManager.SetupTextEditor(LanguageTextEditor, Syntax.Json);
           ThemeManager.SetupTextEditor(CodeTextEditor, Syntax.Javascript);

            if (AddonManager.CurrentAddon != null)
            {
                _actions = AddonManager.CurrentAddon.Actions;
                ActionListBox.ItemsSource = _actions;

                if (_actions.Any())
                {
                    ActionListBox.SelectedIndex = 0;
                    _selectedAction = _actions.Values.First();
                    AceTextEditor.Text = _selectedAction.Ace;
                    LanguageTextEditor.Text = _selectedAction.Language;
                    CodeTextEditor.Text = _selectedAction.Code;
                    Category.Text = _selectedAction.Category;
                }
            }
            else
            {
                ActionListBox.ItemsSource = null;
                AceTextEditor.Text = string.Empty;
                LanguageTextEditor.Text = string.Empty;
                CodeTextEditor.Text = string.Empty;
                Category.Text = string.Empty;
            }
        }

        /// <summary>
        /// handles when action window loses focus
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                //save the current selected action
                if (_selectedAction != null)
                {
                    _selectedAction.Ace = AceTextEditor.Text;
                    _selectedAction.Language = LanguageTextEditor.Text;
                    _selectedAction.Code = CodeTextEditor.Text;
                    _selectedAction.Category = Category.Text;
                    _actions[_selectedAction.Id] = _selectedAction;
                }

                AddonManager.CurrentAddon.Actions = _actions;
                AddonManager.SaveCurrentAddon();
            }
        }

        /// <summary>
        /// clears all input in action window
        /// </summary>
        public void Clear()
        {
            _actions = new Dictionary<string, Action>();
            _selectedAction = null;
            ActionListBox.ItemsSource = null;
            AceTextEditor.Text = string.Empty;
            CodeTextEditor.Text = string.Empty;
            LanguageTextEditor.Text = string.Empty;
            Category.Text = string.Empty;
        }

        /// <summary>
        /// handles auto completion and parsing language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LanguageTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJsonDocument(LanguageTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(LanguageTextEditor, e.Text))
            {
                //figure out word segment
                var segment = LanguageTextEditor.TextArea.GetCurrentWord();
                if (segment == null) return;

                //get string from segment
                var text = LanguageTextEditor.Document.GetText(segment);
                if (string.IsNullOrWhiteSpace(text)) return;

                //filter completion list by string
                var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedAction.Id}_lang_json").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                if (data.Any())
                {
                    ShowCompletion(LanguageTextEditor.TextArea, data);
                }
            }
        }

        /// <summary>
        /// handles auto completion and parsing language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJsonDocument(AceTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(AceTextEditor, e.Text))
            {
                //figure out word segment
                var segment = AceTextEditor.TextArea.GetCurrentWord();
                if (segment == null) return;

                //get string from segment
                var text = AceTextEditor.Document.GetText(segment);
                if (string.IsNullOrWhiteSpace(text)) return;

                //filter completion list by string
                var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedAction.Id}_ace_json").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList(); ;
                if (data.Any())
                {
                    ShowCompletion(AceTextEditor.TextArea, data);
                }
            }
        }

        /// <summary>
        /// handles auto completion and parsing code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(CodeTextEditor.Text);

            //add matching closing symbol
            if (!TextEditorHelper.Insatnce.MatchSymbol(CodeTextEditor, e.Text))
            {
                if (e.Text == ".")
                {
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedAction.Id}_code_script").ToList();
                    if (data.Any())
                    {
                        ShowCompletion(CodeTextEditor.TextArea, data);
                    }
                }
                else
                {
                    //figure out word segment
                    var segment = CodeTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = CodeTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedAction.Id}_code_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(CodeTextEditor.TextArea, data);
                    }
                }   
            }
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
                if (!char.IsLetterOrDigit(e.Text[0]) && !char.IsWhiteSpace(e.Text[0]))
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
                Searcher.Insatnce.UpdateFileIndex($"act_{_selectedAction.Id}_ace", AceTextEditor.Text, ApplicationWindows.ActionWindow);
                Searcher.Insatnce.UpdateFileIndex($"act_{_selectedAction.Id}_lang", LanguageTextEditor.Text, ApplicationWindows.ActionWindow);
                Searcher.Insatnce.UpdateFileIndex($"act_{_selectedAction.Id}_code", CodeTextEditor.Text, ApplicationWindows.ActionWindow);

                var editor = ((TextEditor)sender);
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }

        /// <summary>
        /// handles auto completion in the display text textbox for bbc codes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayText_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var currentIndex = DisplayText.CaretIndex;
            if (e.Key == Key.OemOpenBrackets && (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) != 0)
            {
                e.Handled = true;
                DisplayText.Text = DisplayText.Text.Insert(DisplayText.CaretIndex, "{}");
                DisplayText.CaretIndex = currentIndex + 1;

            }
            else if (e.Key == Key.OemOpenBrackets)
            {
                e.Handled = true;
                DisplayText.Text = DisplayText.Text.Insert(DisplayText.CaretIndex, "[]");
                DisplayText.CaretIndex = currentIndex + 1;
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
        /// shows the add action child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAction_OnClick(object sender, RoutedEventArgs e)
        {
            ActionIdText.Text = "action-id";
            ActionCategoryText.Text = "custom";
            ActionListNameText.Text = "Execute Action";
            HighlightDropdown.Text = "false";
            AsyncDropdown.Text = "no";
            DisplayText.Text = "this is the display text";
            DescriptionText.Text = "this is the description";
            NewActionWindow.IsOpen = true;
        }

        /// <summary>
        /// removes the selected action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveAction_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedAction != null)
            {
                _actions.Remove(_selectedAction.Id);
                ActionListBox.ItemsSource = _actions;
                ActionListBox.Items.Refresh();

                //clear editors
                AceTextEditor.Text = string.Empty;
                LanguageTextEditor.Text = string.Empty;
                CodeTextEditor.Text = string.Empty;
                _selectedAction = null;
            }
            else
            {
                 NotificationManager.PublishErrorNotification("failed to remove action, no action selected");
            }
        }

        /// <summary>
        /// handles creating a duplicate action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DuplicateAce_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedAction != null)
            {
                var newId = await WindowManager.ShowInputDialog("New Action ID", "enter new action id", string.Empty);
                if (newId == null) return;

                if (_actions.ContainsKey(newId))
                {
                    NotificationManager.PublishErrorNotification("failed to duplicate action, action id already exists");
                    return;
                }

                _selectedAction.Ace = AceTextEditor.Text;
                _selectedAction.Language = LanguageTextEditor.Text;
                _selectedAction.Code = CodeTextEditor.Text;
                _selectedAction.Category = Category.Text;

                if (!string.IsNullOrWhiteSpace(newId))
                {
                    var newAction = _selectedAction.Copy(newId.Replace(" ", "-"));
                    _actions.Add(newAction.Id, newAction);
                    ActionListBox.Items.Refresh();
                    ActionListBox.SelectedIndex = _actions.Count - 1;
                }
                else
                {
                    NotificationManager.PublishErrorNotification("failed to duplicate action, no action id entered");
                }
            }
            else
            {
                NotificationManager.PublishErrorNotification("failed to duplicate action, no action selected");
            }
        }

        /// <summary>
        /// handles the save button on the add new action child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveActionButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ActionIdText.Text.ToLower().Replace(" ", "-");
            var category = ActionCategoryText.Text;
            var list = ActionListNameText.Text;
            var highlight = HighlightDropdown.Text;
            var async = AsyncDropdown.Text == "yes" ? true : false;
            var displayText = DisplayText.Text;
            var desc = DescriptionText.Text;

            if (_actions.ContainsKey(id))
            {
                NotificationManager.PublishErrorNotification("action id already exists");
                return;
            }

            var action = new Action
            {
                Id = id.Trim().ToLower(),
                Category = category.Trim().ToLower(),
                Highlight = highlight,
                DisplayText = displayText,
                Description = desc,
                ListName = list
            };

            action.Ace = TemplateCompiler.Insatnce.CompileTemplates(AddonManager.CurrentAddon.Template.ActionAces, action);
            action.Language = TemplateCompiler.Insatnce.CompileTemplates(AddonManager.CurrentAddon.Template.ActionLanguage, action);
            action.Code = TemplateCompiler.Insatnce.CompileTemplates(AddonManager.CurrentAddon.Template.ActionCode, action);

            if (async) action.Code = $"async {action.Code}";

            _actions.Add(id, action);
            ActionListBox.Items.Refresh();
            ActionListBox.SelectedIndex = _actions.Count - 1;

            AddonManager.CurrentAddon.Actions = _actions;
            NewActionWindow.IsOpen = false;
        }

        /// <summary>
        /// opens the add new parameter child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertNewParam_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedAction == null) return;
            ParamIdText.Text = "param-id";
            ParamTypeDropdown.Text = "any";
            ParamValueText.Text = string.Empty;
            ParamNameText.Text = "This is the parameters name";
            ParamDescText.Text = "This is the parameters description";
            NewParamWindow.IsOpen = true;
        }

        /// <summary>
        /// handles the save button on the add new parameter window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveParamButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var id = ParamIdText.Text.ToLower().Replace(" ", "-");
                var type = ParamTypeDropdown.Text;
                var value = ParamValueText.Text;
                var name = ParamNameText.Text;
                var desc = ParamDescText.Text;
                var isVariadic = type == "variadic";

                //there is at least one param defined
                if (AceTextEditor.Text.Contains("\"params\": ["))
                {
                    //ace param
                    var aceTemplate = TemplateHelper.AceParam(id, type, value);

                    //AceTextEditor.Text = FormatHelper.Insatnce.Json(AceTextEditor.Text.Replace("\"params\": [", aceTemplate));
                    //AceTextEditor.Text = FormatHelper.Insatnce.Json(AceTextEditor.Text.Replace("}\r\n ]", aceTemplate));
                    AceTextEditor.Text = FormatHelper.Insatnce.Json(Regex.Replace(AceTextEditor.Text, @"}(\r\n?|\n|\s*)]", $"{aceTemplate}\r\n]"));

                    //todo insert extra param usign jtoken.parse
                    //language param
                    var langTemplate = TemplateHelper.AceLang(id, type, name, desc);
                    //LanguageTextEditor.Text = LanguageTextEditor.Text.Replace(@"""params"": {", langTemplate);

                    //todo: this throws exception
                    var langJson = JObject.Parse($"{{ {LanguageTextEditor.Text} }}").Values()["params"].First().Parent;
                    langJson.Add(langTemplate);


                    //code param
                    var codeTemplate = TemplateHelper.AceCode(id, _selectedAction.ScriptName, isVariadic);
                    CodeTextEditor.Text = CodeTextEditor.Text.Replace($"{_selectedAction.ScriptName}(", codeTemplate);
                }
                //this will be the first param
                else
                {
                    //ace param
                    var aceTemplate = TemplateHelper.AceParamFirst(id, type, value);
                    AceTextEditor.Text = FormatHelper.Insatnce.Json(AceTextEditor.Text.Replace("}", aceTemplate));

                    //language param
                    var langTemplate = TemplateHelper.AceLangFirst(id, type, name, desc);
                    LanguageTextEditor.Text = LanguageTextEditor.Text.Replace(@"""
}", langTemplate);

                    //code param
                    var codeTemplate = TemplateHelper.AceCodeFirst(id, _selectedAction.ScriptName, isVariadic);
                    CodeTextEditor.Text = CodeTextEditor.Text.Replace($"{_selectedAction.ScriptName}()", codeTemplate);
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
            }

            NewParamWindow.IsOpen = false;
        }

        /// <summary>
        /// handles switching actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActionListBox.SelectedIndex == -1)
            {
                return;
            }

            //save current selection
            if (_selectedAction != null)
            {
                _selectedAction.Ace = AceTextEditor.Text;
                _selectedAction.Language = LanguageTextEditor.Text;
                _selectedAction.Code = CodeTextEditor.Text;
                _selectedAction.Category = Category.Text;
                _actions[_selectedAction.Id] = _selectedAction;
                AddonManager.CurrentAddon.Actions = _actions;
                AddonManager.SaveCurrentAddon();
            }

            //load new selection
            var selectedKey = ((KeyValuePair<string, Action>)ActionListBox.SelectedItem).Key;
            _selectedAction = _actions[selectedKey];

            Category.Text = _selectedAction.Category;
            AceTextEditor.Text = _selectedAction.Ace;
            LanguageTextEditor.Text = _selectedAction.Language;
            CodeTextEditor.Text = _selectedAction.Code;
        }

        /// <summary>
        /// handles formatting the code as javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJavascript_OnClick(object sender, RoutedEventArgs e)
        {
            CodeTextEditor.Text = FormatHelper.Insatnce.Javascript(CodeTextEditor.Text);
        }

        /// <summary>
        /// handles formatting the language as json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJsonLang_OnClick(object sender, RoutedEventArgs e)
        {
            LanguageTextEditor.Text = FormatHelper.Insatnce.Json(LanguageTextEditor.Text, true);
        }

        /// <summary>
        /// handles formatting the ace as json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJsonAce_OnClick(object sender, RoutedEventArgs e)
        {
            AceTextEditor.Text = FormatHelper.Insatnce.Json(AceTextEditor.Text);
        }

        /// <summary>
        /// beings up the change category dialog 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedAction != null)
            {
                var cat = _selectedAction.Category;
                var newCategory = await WindowManager.ShowInputDialog("Change Action Category", "enter new action category", cat);

                if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    _selectedAction.Category = newCategory;
                }
            }
            else
            {
                NotificationManager.PublishErrorNotification("failed to remove action, no action selected");
            }
        }

        /// <summary>
        /// resets view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultView_OnClick(object sender, RoutedEventArgs e)
        {
            AcePanel.Width = new GridLength(3, GridUnitType.Star);
            CodePanel.Width = new GridLength(3, GridUnitType.Star);
            LangPanel.Width = new GridLength(3, GridUnitType.Star);
        }

        /// <summary>
        /// handles expanding the ace json section
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceView_OnClick(object sender, RoutedEventArgs e)
        {
            CodePanel.Width = new GridLength(0);
            LangPanel.Width = new GridLength(0);
            AcePanel.Width = new GridLength(3, GridUnitType.Star);
        }

        /// <summary>
        /// handles expanding the code section
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeView_OnClick(object sender, RoutedEventArgs e)
        {
            AcePanel.Width = new GridLength(0);
            LangPanel.Width = new GridLength(0);
            CodePanel.Width = new GridLength(3, GridUnitType.Star);
        }

        /// <summary>
        /// handles expanding the language section
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LangView_OnClick(object sender, RoutedEventArgs e)
        {
            AcePanel.Width = new GridLength(0);
            CodePanel.Width = new GridLength(0);
            LangPanel.Width = new GridLength(3, GridUnitType.Star);
        }

        /// <summary>
        /// handles selecting all text in text box when text box gets focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            tb?.SelectAll();
        }

        /// <summary>
        /// focus on the textbox when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }

        /// <summary>
        /// handles changing the current actions category by changing the text in the category text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Category_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedAction != null)
            {
                _selectedAction.Category = Category.Text;
            }
        }

        /// <summary>
        /// lints the selected actions javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void LintJavascript_OnClick(object sender, RoutedEventArgs e)
        {
            LintingManager.Lint(CodeTextEditor.Text);
        }
    }
}
