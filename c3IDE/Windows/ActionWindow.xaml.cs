using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using c3IDE.DataAccess;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Windows.Interfaces;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using c3IDE.Utilities.ThemeEngine;
using ControlzEx.Standard;
using Action = c3IDE.Models.Action;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for ActionWindow.xaml
    /// </summary>
    public partial class ActionWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Actions";
        private Dictionary<string, Action> _actions;
        private Action _selectedAction;
        private CompletionWindow completionWindow;

        //ctor
        public ActionWindow()
        {
            InitializeComponent();

            CodeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            CodeTextEditor.TextArea.TextEntered += CodeTextEditor_TextEntered;
            CodeTextEditor.Options.EnableEmailHyperlinks = false;
            CodeTextEditor.Options.EnableHyperlinks = false;

            AceTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            AceTextEditor.TextArea.TextEntered += AceTextEditor_TextEntered;
            AceTextEditor.Options.EnableEmailHyperlinks = false;
            AceTextEditor.Options.EnableHyperlinks = false;

            LanguageTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            LanguageTextEditor.TextArea.TextEntered += LanguageTextEditor_TextEntered;
            LanguageTextEditor.Options.EnableEmailHyperlinks = false;
            LanguageTextEditor.Options.EnableHyperlinks = false;
        }

        //editor events
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
        private void AddAction_OnClick(object sender, RoutedEventArgs e)
        {
            ActionIdText.Text = "action-id";
            ActionCategoryText.Text = "custom";
            ActionListNameText.Text = "Execute Action";
            HighlightDropdown.Text = "false";
            DisplayText.Text = "this is the display text";
            DescriptionText.Text = "this is the description";
            NewActionWindow.IsOpen = true;
        }

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
                AppData.Insatnce.ErrorMessage("failed to remove action, no action selected");
            }
        }

        private void SaveActionButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ActionIdText.Text.ToLower().Replace(" ", "-");
            var category = ActionCategoryText.Text;
            var list = ActionListNameText.Text;
            var highlight = HighlightDropdown.Text;
            var displayText = DisplayText.Text;
            var desc = DescriptionText.Text;

            if (_actions.ContainsKey(id))
            {
                AppData.Insatnce.ErrorMessage("action id already exists");
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

            action.Ace = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionAces, action);
            action.Language = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionLanguage, action);
            action.Code = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionCode, action);

            _actions.Add(id, action);
            ActionListBox.Items.Refresh();
            ActionListBox.SelectedIndex = _actions.Count - 1;
            NewActionWindow.IsOpen = false;
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
                AceTextEditor.Text = FormatHelper.Insatnce.Json(AceTextEditor.Text.Replace("\"params\": [", aceTemplate));

                //language param
                var langTemplate = TemplateHelper.AceLang(id, type, name, desc);
                LanguageTextEditor.Text = LanguageTextEditor.Text.Replace(@"""params"": {", langTemplate);

                //code param
                var codeTemplate = TemplateHelper.AceCode(id, _selectedAction.ScriptName);
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
                var codeTemplate = TemplateHelper.AceCodeFirst(id, _selectedAction.ScriptName);
                CodeTextEditor.Text = CodeTextEditor.Text.Replace($"{_selectedAction.ScriptName}()", codeTemplate);
            }

            NewParamWindow.IsOpen = false;
        }

        //window states
        public void OnEnter()
        {
            AppData.Insatnce.SetupTextEditor(AceTextEditor, Syntax.Json);
            AppData.Insatnce.SetupTextEditor(LanguageTextEditor, Syntax.Json);
            AppData.Insatnce.SetupTextEditor(CodeTextEditor, Syntax.Javascript);

            if (AppData.Insatnce.CurrentAddon != null)
            {
                _actions = AppData.Insatnce.CurrentAddon.Actions;
                ActionListBox.ItemsSource = _actions;

                if (_actions.Any())
                {
                    ActionListBox.SelectedIndex = 0;
                }
            }
            else
            {
                ActionListBox.ItemsSource = null;
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
                if (_selectedAction != null)
                {
                    _selectedAction.Ace = AceTextEditor.Text;
                    _selectedAction.Language = LanguageTextEditor.Text;
                    _selectedAction.Code = CodeTextEditor.Text;
                    _selectedAction.Category = Category.Text;
                    _actions[_selectedAction.Id] = _selectedAction;
                }

                AppData.Insatnce.CurrentAddon.Actions = _actions;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
                AppData.Insatnce.CurrentAddon =
                    DataAccessFacade.Insatnce.AddonData.Get(x => x.Id.Equals(AppData.Insatnce.CurrentAddon.Id)).FirstOrDefault();
            }          
        }

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

        public void SetupTheme(Theme t)
        {
            
        }

        //list box events
        private void ActionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActionListBox.SelectedIndex == -1)
            {
                //ignore
                Category.Text = string.Empty;
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
            }

            //load new selection
            _selectedAction = ((KeyValuePair<string, Action>)ActionListBox.SelectedItem).Value;
            Category.Text = _selectedAction.Category;
            AceTextEditor.Text = _selectedAction.Ace;
            LanguageTextEditor.Text = _selectedAction.Language;
            CodeTextEditor.Text = _selectedAction.Code;
        }

        //context menu
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
            if (_selectedAction != null)
            {
                var cat = _selectedAction.Category;
                var newCategory = await AppData.Insatnce.ShowInputDialog("Change Action Category", "enter new action category", cat);

                if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    _selectedAction.Category = newCategory;
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

        private async void DuplicateAce_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedAction != null)
            {
                var newId = await AppData.Insatnce.ShowInputDialog("New Action ID", "enter new action id", string.Empty);

                if (_actions.ContainsKey(newId))
                {
                    AppData.Insatnce.ErrorMessage("failed to duplicate action, action id already exists");
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
                    AppData.Insatnce.ErrorMessage("failed to duplicate action, no action id entered");
                }
            }
            else
            {
                AppData.Insatnce.ErrorMessage("failed to duplicate action, no action selected");
            }
        }
    }
}
