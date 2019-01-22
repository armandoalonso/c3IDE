using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.DataAccess;
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Helpers;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using Condition = c3IDE.Models.Condition;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for ConditionWindow.xaml
    /// </summary>
    public partial class ConditionWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Conditions";
        private Dictionary<string, Condition> _conditions;
        private Condition _selectedCondition;
        private CompletionWindow completionWindow;

        //ctor
        public ConditionWindow()
        {
            InitializeComponent();

            CodeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            CodeTextEditor.TextArea.TextEntered += CodeTextEditor_TextEntered;

            AceTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            AceTextEditor.TextArea.TextEntered += AceTextEditor_TextEntered;

            LanguageTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            LanguageTextEditor.TextArea.TextEntered += LanguageTextEditor_TextEntered;
        }

        //editor events
        private void LanguageTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJsonDocument(LanguageTextEditor.Text);

            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    LanguageTextEditor.Document.Insert(LanguageTextEditor.TextArea.Caret.Offset, "}");
                    LanguageTextEditor.TextArea.Caret.Offset--;
                    return;

                case "\"":
                    LanguageTextEditor.Document.Insert(LanguageTextEditor.TextArea.Caret.Offset, "\"");
                    LanguageTextEditor.TextArea.Caret.Offset--;
                    return;

                case "[":
                    LanguageTextEditor.Document.Insert(LanguageTextEditor.TextArea.Caret.Offset, "]");
                    LanguageTextEditor.TextArea.Caret.Offset--;
                    return;

                case "(":
                    LanguageTextEditor.Document.Insert(LanguageTextEditor.TextArea.Caret.Offset, ")");
                    LanguageTextEditor.TextArea.Caret.Offset--;
                    return;

                default:
                    //figure out word segment
                    var segment = LanguageTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = LanguageTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.Json).Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList(); ;
                    if (data.Any())
                    {
                        ShowCompletion(LanguageTextEditor.TextArea, data);
                    }
                    break;
            }
        }

        private void AceTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var allTokens = JavascriptParser.Insatnce.ParseJsonDocument(AceTextEditor.Text);

            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    AceTextEditor.Document.Insert(AceTextEditor.TextArea.Caret.Offset, "}");
                    AceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "\"":
                    AceTextEditor.Document.Insert(AceTextEditor.TextArea.Caret.Offset, "\"");
                    AceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "[":
                    AceTextEditor.Document.Insert(AceTextEditor.TextArea.Caret.Offset, "]");
                    AceTextEditor.TextArea.Caret.Offset--;
                    return;

                case "(":
                    AceTextEditor.Document.Insert(AceTextEditor.TextArea.Caret.Offset, ")");
                    AceTextEditor.TextArea.Caret.Offset--;
                    return;

                default:
                    //figure out word segment
                    var segment = AceTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = AceTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.Json).Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList(); ;
                    if (data.Any())
                    {
                        ShowCompletion(AceTextEditor.TextArea, data);
                    }
                    break;
            }
        }

        private void CodeTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;
            var tokenList = JavascriptParser.Insatnce.ParseJavascriptDocument(CodeTextEditor.Text, CodeType.RuntimeJavascript);
            var methodsTokens = JavascriptParser.Insatnce.ParseJavascriptUserTokens(CodeTextEditor.Text);
            var allTokens = JavascriptParser.Insatnce.DecorateMethodInterfaces(tokenList, methodsTokens, CodeType.RuntimeJavascript);

            //add matching closing symbol
            switch (e.Text)
            {
                case "{":
                    CodeTextEditor.Document.Insert(CodeTextEditor.TextArea.Caret.Offset, "}");
                    CodeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "\"":
                    CodeTextEditor.Document.Insert(CodeTextEditor.TextArea.Caret.Offset, "\"");
                    CodeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "[":
                    CodeTextEditor.Document.Insert(CodeTextEditor.TextArea.Caret.Offset, "]");
                    CodeTextEditor.TextArea.Caret.Offset--;
                    return;

                case "(":
                    CodeTextEditor.Document.Insert(CodeTextEditor.TextArea.Caret.Offset, ")");
                    CodeTextEditor.TextArea.Caret.Offset--;
                    return;
                case ".":
                    var methodsData = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.RuntimeJavascript)
                        .Where(x => x.Type == CompletionType.Methods || x.Type == CompletionType.Modules || x.Type == CompletionType.Misc);
                    ShowCompletion(CodeTextEditor.TextArea, methodsData.ToList());
                    break;
                default:
                    //figure out word segment
                    var segment = CodeTextEditor.TextArea.GetCurrentWord();
                    if (segment == null) return;

                    //get string from segment
                    var text = CodeTextEditor.Document.GetText(segment);
                    if (string.IsNullOrWhiteSpace(text)) return;

                    //filter completion list by string
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.RuntimeJavascript).Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
                    if (data.Any())
                    {
                        ShowCompletion(CodeTextEditor.TextArea, data);
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

        //button clicks
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
            var desc = DescriptionText.Text;

            if (_conditions.ContainsKey(id))
            {
                //TODO: error duplicate id
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

            //TODO: condition templates (using action templates)
            cnd.Ace = TemplateHelper.CndAce(cnd);
            cnd.Language = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionLanguage, cnd);
            cnd.Code = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionCode, cnd);

            _conditions.Add(id, cnd);
            ConditionListBox.Items.Refresh();
            ConditionListBox.SelectedIndex = _conditions.Count - 1;
            NewConditionWindow.IsOpen = false;
        }

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
                AppData.Insatnce.ErrorMessage("failed to remove condition, no condition selected");
            }
        }

        private void SaveParamButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ParamIdText.Text.ToLower().Replace(" ", "-");
            var type = ParamTypeDropdown.Text;
            var value = ParamValueText.Text;
            var name = ParamNameText.Text;
            var desc = ParamDescText.Text;

            //there is at least one param defined
            if (AceTextEditor.Text.Contains("\"params\": ["))
            {
                //ace param
                var aceTemplate = TemplateHelper.AceParam(id, type, value);
                AceTextEditor.Text = FormatHelper.Insatnce.Json(AceTextEditor.Text.Replace("    \"params\": [", aceTemplate));

                //language param
                var langTemplate = TemplateHelper.AceLang(id, type, name, desc);
                LanguageTextEditor.Text =LanguageTextEditor.Text.Replace(@"	""params"": {", langTemplate);

                //code param
                var codeTemplate = TemplateHelper.AceCode(id, _selectedCondition.ScriptName);
                CodeTextEditor.Text = CodeTextEditor.Text.Replace($"{_selectedCondition.ScriptName}(", codeTemplate);
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

            NewParamWindow.IsOpen = false;
        }

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

        //window states
        public void OnEnter()
        {
            AppData.Insatnce.SetupTextEditor(AceTextEditor);
            AppData.Insatnce.SetupTextEditor(LanguageTextEditor);
            AppData.Insatnce.SetupTextEditor(CodeTextEditor);

            if (AppData.Insatnce.CurrentAddon != null)
            {
                {
                    _conditions = AppData.Insatnce.CurrentAddon.Conditions;
                    ConditionListBox.ItemsSource = _conditions;

                    if (_conditions.Any())
                    {
                        ConditionListBox.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                ConditionListBox.ItemsSource = null;
                AceTextEditor.Text = string.Empty;
                LanguageTextEditor.Text = string.Empty;
                CodeTextEditor.Text = string.Empty;
            }

        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                //save the current selected action
                if (_selectedCondition != null)
                {
                    _selectedCondition.Ace = AceTextEditor.Text;
                    _selectedCondition.Language = LanguageTextEditor.Text;
                    _selectedCondition.Code = CodeTextEditor.Text;
                    _conditions[_selectedCondition.Id] = _selectedCondition;
                }

                AppData.Insatnce.CurrentAddon.Conditions = _conditions;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }

        }

        //list box events
        private void ConditionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //save current selection
            if (_selectedCondition != null)
            {
                _selectedCondition.Ace = AceTextEditor.Text;
                _selectedCondition.Language = LanguageTextEditor.Text;
                _selectedCondition.Code = CodeTextEditor.Text;
                _conditions[_selectedCondition.Id] = _selectedCondition;
            }

            //load new selection
            //TODO: error here when removing with 2 configured
            _selectedCondition = ((KeyValuePair<string, Condition>)ConditionListBox.SelectedItem).Value;
            AceTextEditor.Text = _selectedCondition.Ace;
            LanguageTextEditor.Text = _selectedCondition.Language;
           CodeTextEditor.Text = _selectedCondition.Code;
        }

        //context menu
        private void InsertNewParam_OnClick(object sender, RoutedEventArgs e)
        {
            ParamIdText.Text = "param-id";
            ParamTypeDropdown.Text = "number";
            ParamValueText.Text = string.Empty;
            ParamNameText.Text = "This is the parameters name";
            ParamDescText.Text = "This is the parameters description";
            NewParamWindow.IsOpen = true;
        }

        private void FormatJavascript_OnClick(object sender, RoutedEventArgs e)
        {
            CodeTextEditor.Text = FormatHelper.Insatnce.Javascript(CodeTextEditor.Text);
        }

        private void FormatJsonLang_OnClick(object sender, RoutedEventArgs e)
        {
            LanguageTextEditor.Text = FormatHelper.Insatnce.Json(LanguageTextEditor.Text, true);
        }

        private void FormatJsonAce_OnClick(object sender, RoutedEventArgs e)
        {
            AceTextEditor.Text = FormatHelper.Insatnce.Json(AceTextEditor.Text);
        }

        private async void ChangeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedCondition != null)
            {
                var cat = _selectedCondition.Category;
                var newCategory = await AppData.Insatnce.ShowInputDialog("Change Condition Category", "enter new condition category", cat);

                if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    _selectedCondition.Category = newCategory;
                }
            }
            else
            {
                AppData.Insatnce.ErrorMessage("failed to remove action, no action selected");
            }
        }

        //view buttons
        private void AceView_OnClick(object sender, RoutedEventArgs e)
        {
            CodePanel.Width = new GridLength(0);
            LangPanel.Width = new GridLength(0);
            AcePanel.Width = new GridLength(3, GridUnitType.Star);
        }

        private void DeafultView_OnClick(object sender, RoutedEventArgs e)
        {
            AcePanel.Width = new GridLength(3, GridUnitType.Star);
            CodePanel.Width = new GridLength(3, GridUnitType.Star);
            LangPanel.Width = new GridLength(3, GridUnitType.Star);
        }

        private void CodeView_OnClick(object sender, RoutedEventArgs e)
        {
            AcePanel.Width = new GridLength(0);
            LangPanel.Width = new GridLength(0);
            CodePanel.Width = new GridLength(3, GridUnitType.Star);
        }

        private void LangView_OnClick(object sender, RoutedEventArgs e)
        {
            AcePanel.Width = new GridLength(0);
            CodePanel.Width = new GridLength(0);
            LangPanel.Width = new GridLength(3, GridUnitType.Star);
        }

        //text box events
        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            tb?.SelectAll();
        }

        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }
    }
}
    