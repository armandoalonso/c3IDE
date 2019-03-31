using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.DataAccess;
using c3IDE.Managers;
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Condition = c3IDE.Models.Condition;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for ConditionWindow.xaml
    /// </summary>
    public partial class ConditionWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Conditions";
        private Dictionary<string, Condition> _conditions;
        private Condition _selectedCondition;
        private CompletionWindow completionWindow;

        /// <summary>
        /// condition window constructor 
        /// </summary>
        public ConditionWindow()
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
        /// handles the condition window getting focus
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(AceTextEditor, Syntax.Json);
            ThemeManager.SetupTextEditor(LanguageTextEditor, Syntax.Json);
            ThemeManager.SetupTextEditor(CodeTextEditor, Syntax.Javascript);

            if (AddonManager.CurrentAddon != null)
            {

                _conditions = AddonManager.CurrentAddon.Conditions;
                ConditionListBox.ItemsSource = _conditions;

                if (_conditions.Any())
                {
                    ConditionListBox.SelectedIndex = 0;
                    _selectedCondition = _conditions.Values.First();
                    AceTextEditor.Text = _selectedCondition.Ace;
                    LanguageTextEditor.Text = _selectedCondition.Language;
                    CodeTextEditor.Text = _selectedCondition.Code;
                    Category.Text = _selectedCondition.Category;
                }

            }
            else
            {
                ConditionListBox.ItemsSource = null;
                AceTextEditor.Text = string.Empty;
                LanguageTextEditor.Text = string.Empty;
                CodeTextEditor.Text = string.Empty;
                Category.Text = string.Empty;
            }

        }

        /// <summary>
        /// handles when condition window loses focus
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                //save the current selected action
                if (_selectedCondition != null)
                {
                    _selectedCondition.Ace = AceTextEditor.Text;
                    _selectedCondition.Language = LanguageTextEditor.Text;
                    _selectedCondition.Code = CodeTextEditor.Text;
                    _selectedCondition.Category = Category.Text;
                    _conditions[_selectedCondition.Id] = _selectedCondition;
                }

                AddonManager.CurrentAddon.Conditions = _conditions;
                AddonManager.SaveCurrentAddon();
            }

        }

        /// <summary>
        /// clears all input in action window
        /// </summary>
        public void Clear()
        {
            _conditions = new Dictionary<string, Condition>();
            _selectedCondition = null;
            ConditionListBox.ItemsSource = null;
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
                var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedCondition.Id}_lang_json").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
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
                var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedCondition.Id}_ace_json").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList(); ;
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
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedCondition.Id}_code_script").ToList();
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
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedCondition.Id}_code_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
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
                Searcher.Insatnce.UpdateFileIndex($"cnd_{_selectedCondition.Id}_ace", AceTextEditor.Text, ApplicationWindows.ConditionWindow);
                Searcher.Insatnce.UpdateFileIndex($"cnd_{_selectedCondition.Id}_lang", LanguageTextEditor.Text, ApplicationWindows.ConditionWindow);
                Searcher.Insatnce.UpdateFileIndex($"cnd_{_selectedCondition.Id}_code", CodeTextEditor.Text, ApplicationWindows.ConditionWindow);
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
        /// shows the add condition child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCondition_OnClick(object sender, RoutedEventArgs e)
        {
            ConditionIdText.Text = "condition-id";
            ConditionCategoryText.Text = "custom";
            ConditionListNameText.Text = "On Condition";
            HighlightDropdown.SelectedIndex = 0;
            TriggerDropdown.SelectedIndex = 0;
            FakeTriggerDropdown.SelectedIndex = 0;
            LoopingDropdown.SelectedIndex = 0;
            StaticDropdown.SelectedIndex = 0;
            CompatibleWithTriggersDropdown.SelectedIndex = 0;
            InvertibleDropdown.SelectedIndex = 0;
            DisplayText.Text = "this is the display text";
            DescriptionText.Text = "this is the description";
            NewConditionWindow.IsOpen = true;
        }

        /// <summary>
        /// removes the selected condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveCondition_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedCondition != null)
            {
                _conditions.Remove(_selectedCondition.Id);
                ConditionListBox.ItemsSource = _conditions;
                ConditionListBox.Items.Refresh();

                //clear editors
                AceTextEditor.Text = string.Empty;
                LanguageTextEditor.Text = string.Empty;
                CodeTextEditor.Text = string.Empty;
                _selectedCondition = null;
            }
            else
            {
                NotificationManager.PublishErrorNotification("failed to remove condition, no condition selected");
            }
        }

        /// <summary>
        /// duplicates the selected condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DuplicateAce_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedCondition != null)
            {
                var newId = await WindowManager.ShowInputDialog("New Condition ID", "enter new condition id", string.Empty);
                if (newId == null) return;

                if (_conditions.ContainsKey(newId))
                {
                    NotificationManager.PublishErrorNotification("failed to duplicate condition, condition id already exists");
                    return;
                }

                _selectedCondition.Ace = AceTextEditor.Text;
                _selectedCondition.Language = LanguageTextEditor.Text;
                _selectedCondition.Code = CodeTextEditor.Text;
                _selectedCondition.Category = Category.Text;

                if (!string.IsNullOrWhiteSpace(newId))
                {
                    var newCondition = _selectedCondition.Copy(newId.Replace(" ", "-"));
                    _conditions.Add(newCondition.Id, newCondition);
                    ConditionListBox.Items.Refresh();
                    ConditionListBox.SelectedIndex = _conditions.Count - 1;
                }
                else
                {
                    NotificationManager.PublishErrorNotification("failed to duplicate condition, no condition id entered");
                }
            }
            else
            {
                NotificationManager.PublishErrorNotification("failed to duplicate condition, no condition selected");
            }
        }

        /// <summary>
        /// handles the save button on the add condition child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveConditionButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ConditionIdText.Text.ToLower().Replace(" ", "-");
            var category = ConditionCategoryText.Text;
            var list = ConditionListNameText.Text;
            var highlight = HighlightDropdown.Text;
            var trigger = TriggerDropdown.Text;
            var faketrigger = FakeTriggerDropdown.Text;
            var isstatic = StaticDropdown.Text;
            var looping = LoopingDropdown.Text;
            var invertible = InvertibleDropdown.Text;
            var triggercompatible = CompatibleWithTriggersDropdown.Text;
            var displayText = DisplayText.Text;
            var desc = DescriptionText.Text.Replace("\"", "\\\"");

            if (_conditions.ContainsKey(id))
            {
                NotificationManager.PublishErrorNotification("condition id already exists");
                return;
            }

            var cnd = new Condition
            {
                Id = id.Trim().ToLower(),
                Category = category.Trim().ToLower(),
                ListName = list,
                Highlight = highlight,
                Trigger = trigger,
                FakeTrigger = faketrigger,
                Static = isstatic,
                Looping = looping,
                Invertible = invertible,
                TriggerCompatible = triggercompatible,
                DisplayText = displayText,
                Description = desc
            };

            cnd.Ace = TemplateHelper.CndAce(cnd);
            cnd.Language = TemplateCompiler.Insatnce.CompileTemplates(AddonManager.CurrentAddon.Template.ActionLanguage, cnd);
            cnd.Code = TemplateCompiler.Insatnce.CompileTemplates(AddonManager.CurrentAddon.Template.ActionCode, cnd);

            _conditions.Add(id, cnd);
            ConditionListBox.Items.Refresh();
            ConditionListBox.SelectedIndex = _conditions.Count - 1;
            AddonManager.CurrentAddon.Conditions = _conditions;
            NewConditionWindow.IsOpen = false;
        }

        /// <summary>
        /// shows the add new parameter window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertNewParam_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedCondition == null) return;
            ParamIdText.Text = "param-id";
            ParamTypeDropdown.Text = "number";
            ParamValueText.Text = string.Empty;
            ParamNameText.Text = "This is the parameters name";
            ParamDescText.Text = "This is the parameters description";
            NewParamWindow.IsOpen = true;
        }

        /// <summary>
        /// handles teh save button on the new parameter window
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
                var desc = ParamDescText.Text.Replace("\"", "\\\"");
                var conditionId = _selectedCondition.Id;

                //there is at least one param defined
                if (AceTextEditor.Text.Contains("\"params\": ["))
                {
                    //ace param
                    var aceTemplate = TemplateHelper.AceParam(id, type, value);

                    //lang param
                    var langTemplate = TemplateHelper.AceLang(id, type, name, desc);
                    var newProperty = JObject.Parse(langTemplate);
                    var langJson = JObject.Parse($"{{ {LanguageTextEditor.Text} }}")[conditionId];
                    var langParams = langJson["params"];
                    langParams.Last.AddAfterSelf(newProperty.Property(id));
                    langJson["params"] = langParams;


                    //code param
                    var func = Regex.Match(CodeTextEditor.Text, @"(?:\()(?<param>.*)(?:\))");
                    var declaration = Regex.Match(CodeTextEditor.Text, @".*(?:\()(?<param>.*)(?:\))").Value;
                    var paramList = func.Groups["param"].Value.Split(',');
                    var codeTemplate = TemplateHelper.AceCode(id, _selectedCondition.ScriptName, paramList);

                    //updates
                    LanguageTextEditor.Text = $"\"{conditionId}\": {langJson.ToString(formatting: Formatting.Indented)} ";
                    AceTextEditor.Text = FormatHelper.Insatnce.Json(Regex.Replace(AceTextEditor.Text, @"}(\r\n?|\n|\s*)]", $"{aceTemplate}\r\n]"));
                    CodeTextEditor.Text = CodeTextEditor.Text.Replace(declaration, codeTemplate);
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
                    var codeTemplate = TemplateHelper.AceCodeFirst(id, _selectedCondition.ScriptName);
                    CodeTextEditor.Text = CodeTextEditor.Text.Replace($"{_selectedCondition.ScriptName}()", codeTemplate);
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
            }

            NewParamWindow.IsOpen = false;
        }

        /// <summary>
        /// handles changing switching conditions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConditionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ConditionListBox.SelectedIndex == -1)
            {
                return;
            }

            //save current selection
            if (_selectedCondition != null)
            {
                _selectedCondition.Ace = AceTextEditor.Text;
                _selectedCondition.Language = LanguageTextEditor.Text;
                _selectedCondition.Code = CodeTextEditor.Text;
                _selectedCondition.Category = Category.Text;
                _conditions[_selectedCondition.Id] = _selectedCondition;
                AddonManager.CurrentAddon.Conditions = _conditions;
                AddonManager.SaveCurrentAddon();
            }

            //load new selection
            var selectedKey = ((KeyValuePair<string, Condition>)ConditionListBox.SelectedItem).Key;
            _selectedCondition = _conditions[selectedKey];

            Category.Text = _selectedCondition.Category;
            AceTextEditor.Text = _selectedCondition.Ace;
            LanguageTextEditor.Text = _selectedCondition.Language;
            CodeTextEditor.Text = _selectedCondition.Code;
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
        /// brings up the change category dialog 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedCondition != null)
            {
                var cat = _selectedCondition.Category;
                var newCategory = await WindowManager.ShowInputDialog("Change Condition Category", "enter new condition category", cat);

                if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    _selectedCondition.Category = newCategory;
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
            if (_selectedCondition != null)
            {
                _selectedCondition.Category = Category.Text;
            }
        }

        /// <summary>
        /// when the id changes set the list name 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConditionId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConditionIdText.Text)) return;
            var ti = new CultureInfo("en-US", false).TextInfo;
            var listName = ti.ToTitleCase(ConditionIdText.Text.Replace("-", " ").ToLower());
            ConditionListNameText.Text = listName;
        }

        /// <summary>
        /// updates param name when id changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parameter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ParamIdText.Text)) return;
            var ti = new CultureInfo("en-US", false).TextInfo;
            var listName = ti.ToTitleCase(ParamIdText.Text.Replace("-", " ").ToLower());
            ParamNameText.Text = listName;
        }
    }
}
    