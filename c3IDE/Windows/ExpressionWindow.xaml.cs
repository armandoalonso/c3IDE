using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
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
using Expression = c3IDE.Models.Expression;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for ExpressionWindow.xaml
    /// </summary>
    public partial class ExpressionWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Expressions";
        private Dictionary<string, Expression> _expressions;
        private Expression _selectedExpression;
        private CompletionWindow completionWindow;

        /// <summary>
        /// expression window constructor
        /// </summary>
        public ExpressionWindow()
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
        /// handles the expression window gettign focus
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(AceTextEditor, Syntax.Json);
            ThemeManager.SetupTextEditor(LanguageTextEditor, Syntax.Json);
            ThemeManager.SetupTextEditor(CodeTextEditor, Syntax.Javascript);

            if (AddonManager.CurrentAddon != null)
            {
                _expressions = AddonManager.CurrentAddon.Expressions;
                ExpressionListBox.ItemsSource = _expressions;

                if (_expressions.Any())
                {
                    ExpressionListBox.SelectedIndex = 0;
                    _selectedExpression = _expressions.Values.First();
                    AceTextEditor.Text = _selectedExpression.Ace;
                    LanguageTextEditor.Text = _selectedExpression.Language;
                    CodeTextEditor.Text = _selectedExpression.Code;
                    Category.Text = _selectedExpression.Category;
                }
            }
            else
            {
                ExpressionListBox.ItemsSource = null;
                AceTextEditor.Text = string.Empty;
                LanguageTextEditor.Text = string.Empty;
                CodeTextEditor.Text = string.Empty;
            }
        }

        /// <summary>
        /// handles expression window losinf focus
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                //save the current selected expression
                if (_selectedExpression != null)
                {
                    _selectedExpression.Ace = AceTextEditor.Text;
                    _selectedExpression.Language = LanguageTextEditor.Text;
                    _selectedExpression.Code = CodeTextEditor.Text;
                    _selectedExpression.Category = Category.Text;
                    _expressions[_selectedExpression.Id] = _selectedExpression;
                }

                AddonManager.CurrentAddon.Expressions = _expressions;
                AddonManager.SaveCurrentAddon();
            }
        }

        /// <summary>
        /// clears all inputs on expression window
        /// </summary>
        public void Clear()
        {
            _expressions = new Dictionary<string, Expression>();
            _selectedExpression = null;
            ExpressionListBox.ItemsSource = null;
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
                var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedExpression.Id}_lang_json").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
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
                var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedExpression.Id}_ace_json").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList(); ;
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
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedExpression.Id}_code_script").ToList();
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
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, $"{_selectedExpression.Id}_code_script").Where(x => x.Text.ToLower().StartsWith(text.ToLower())).ToList();
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
                Searcher.Insatnce.UpdateFileIndex($"exp_{_selectedExpression.Id}_ace", AceTextEditor.Text, ApplicationWindows.ExpressionWindow);
                Searcher.Insatnce.UpdateFileIndex($"exp_{_selectedExpression.Id}_lang", LanguageTextEditor.Text, ApplicationWindows.ExpressionWindow);
                Searcher.Insatnce.UpdateFileIndex($"exp_{_selectedExpression.Id}_code", CodeTextEditor.Text, ApplicationWindows.ExpressionWindow);
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
        /// shows the add expression child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddExpression_OnClick(object sender, RoutedEventArgs e)
        {
            ExpressionIdText.Text = "expression-id";
            ExpressionCategoryText.Text = "custom";
            ReturnTypeDropdown.Text = "any";
            TranslatedName.Text = "Expression";
            DescriptionText.Text = "This is the expression description";
            NewExpressionWindow.IsOpen = true;
        }

        /// <summary>
        /// removes selected expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveExpression_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedExpression != null)
            {
                _expressions.Remove(_selectedExpression.Id);
                ExpressionListBox.ItemsSource = _expressions;
                ExpressionListBox.Items.Refresh();

                //clear editors
                AceTextEditor.Text = string.Empty;
                LanguageTextEditor.Text = string.Empty;
                CodeTextEditor.Text = string.Empty;
                _selectedExpression = null;
            }
            else
            {
                NotificationManager.PublishErrorNotification("failed to remove condition, no condition selected");
            }
        }

        /// <summary>
        /// duplicates selected expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DuplicateAce_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedExpression != null)
            {
                var newId = await WindowManager.ShowInputDialog("New Expression ID", "enter new expression id", string.Empty);
                if(newId == null) return;

                if (_expressions.ContainsKey(newId))
                {
                    NotificationManager.PublishErrorNotification("failed to duplicate expression, expression id already exists");
                    return;
                }

                _selectedExpression.Ace = AceTextEditor.Text;
                _selectedExpression.Language = LanguageTextEditor.Text;
                _selectedExpression.Code = CodeTextEditor.Text;
                _selectedExpression.Category = Category.Text;

                if (!string.IsNullOrWhiteSpace(newId))
                {
                    var newExpression = _selectedExpression.Copy(newId.Replace(" ", "-"));
                    _expressions.Add(newExpression.Id, newExpression);
                    ExpressionListBox.Items.Refresh();
                    ExpressionListBox.SelectedIndex = _expressions.Count - 1;
                }
                else
                {
                    NotificationManager.PublishErrorNotification("failed to duplicate expression, no expression id entered");
                }
            }
            else
            {
                NotificationManager.PublishErrorNotification("failed to duplicate expression, no expression selected");
            }
        }

        /// <summary>
        /// handles the save button in add expression child window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveExpressionButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ExpressionIdText.Text.ToLower().Replace(" ", "-");
            var category = ExpressionCategoryText.Text;
            var returntype = ReturnTypeDropdown.Text;
            var translatedname = TranslatedName.Text;
            var isvariadic = TranslatedName.Text;
            var desc = DescriptionText.Text;

            if (_expressions.ContainsKey(id))
            {
                NotificationManager.PublishErrorNotification("expression id already exists");
                return;
            }

            var expression = new Expression
            {
                Id = id.Trim().ToLower(),
                Category = category.Trim().ToLower(),
                ReturnType = returntype,
                IsVariadicParameters = isvariadic,
                TranslatedName = translatedname,
                Description = desc
            };

            expression.Ace = TemplateHelper.ExpAces(expression);
            expression.Language = TemplateCompiler.Insatnce.CompileTemplates(AddonManager.CurrentAddon.Template.ExpressionLanguage, expression);
            expression.Code = TemplateCompiler.Insatnce.CompileTemplates(AddonManager.CurrentAddon.Template.ActionCode, expression);

            _expressions.Add(id, expression);
            ExpressionListBox.Items.Refresh();
            ExpressionListBox.SelectedIndex = _expressions.Count - 1;
            AddonManager.CurrentAddon.Expressions = _expressions;
            NewExpressionWindow.IsOpen = false;
        }

        /// <summary>
        /// shows the add new parameter window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertNewParam_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedExpression == null) return;
            ParamIdText.Text = "param-id";
            ParamTypeDropdown.Text = "number";
            ParamValueText.Text = string.Empty;
            ParamNameText.Text = "This is the parameters name";
            ParamDescText.Text = "This is the parameters description";
            NewParamWindow.IsOpen = true;
        }

        /// <summary>
        /// handles the save button in the add new parameter window
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
                var expressionId = _selectedExpression.Id;

                //there is at least one param defined
                if (AceTextEditor.Text.Contains("\"params\": ["))
                {
                    //ace param
                    var aceTemplate = TemplateHelper.AceParam(id, type, value);

                    //lang param
                    var langTemplate = TemplateHelper.AceLang(id, type, name, desc);
                    var newProperty = JObject.Parse(langTemplate);
                    var langJson = JObject.Parse($"{{ {LanguageTextEditor.Text} }}")[expressionId];
                    var langParams = langJson["params"];
                    langParams.Last.AddAfterSelf(newProperty.Property(id));
                    langJson["params"] = langParams;


                    //code param
                    var func = Regex.Match(CodeTextEditor.Text, @"(?:\()(?<param>.*)(?:\))");
                    var declaration = Regex.Match(CodeTextEditor.Text, @".*(?:\()(?<param>.*)(?:\))").Value;
                    var paramList = func.Groups["param"].Value.Split(',');
                    var codeTemplate = TemplateHelper.AceCode(id, _selectedExpression.ScriptName, paramList);

                    //updates
                    LanguageTextEditor.Text = $"\"{expressionId}\": {langJson.ToString(formatting: Formatting.Indented)} ";
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
                    var codeTemplate = TemplateHelper.AceCodeFirst(id, _selectedExpression.ScriptName);
                    CodeTextEditor.Text = CodeTextEditor.Text.Replace($"{_selectedExpression.ScriptName}()", codeTemplate);
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
            }

            NewParamWindow.IsOpen = false;
        }

        /// <summary>
        /// handles switching expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpressionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExpressionListBox.SelectedIndex == -1)
            {
                //ignore
                return;
            }

            //save current selection
            if (_selectedExpression != null)
            {
                _selectedExpression.Ace = AceTextEditor.Text;
                _selectedExpression.Language = LanguageTextEditor.Text;
                _selectedExpression.Code = CodeTextEditor.Text;
                _selectedExpression.Category = Category.Text;
                _expressions[_selectedExpression.Id] = _selectedExpression;
                AddonManager.CurrentAddon.Expressions = _expressions;
                DataAccessFacade.Insatnce.AddonData.Upsert(AddonManager.CurrentAddon);
            }

           var selectedKey = ((KeyValuePair<string, Expression>)ExpressionListBox.SelectedItem).Key;
           _selectedExpression = _expressions[selectedKey];

           Category.Text = _selectedExpression.Category;
           AceTextEditor.Text = _selectedExpression.Ace;
           LanguageTextEditor.Text = _selectedExpression.Language;
           CodeTextEditor.Text = _selectedExpression.Code;
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
            if (_selectedExpression != null)
            {
                var cat = _selectedExpression.Category;
                var newCategory = await WindowManager.ShowInputDialog("Change Expression Category", "enter new expression category", cat);

                if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    _selectedExpression.Category = newCategory;
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
            if (_selectedExpression != null)
            {
                _selectedExpression.Category = Category.Text;
            }
        }

        /// <summary>
        /// lints the selected expression javascript
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LintJavascript_OnClick(object sender, RoutedEventArgs e)
        {
            LintingManager.Lint(CodeTextEditor.Text);
        }
    }
}
