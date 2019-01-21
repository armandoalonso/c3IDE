using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using c3IDE.DataAccess;
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using Expression = c3IDE.Models.Expression;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for ExpressionWindow.xaml
    /// </summary>
    public partial class ExpressionWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Expressions";
        private Dictionary<string, Expression> _expressions;
        private Expression _selectedExpression;
        private CompletionWindow completionWindow;

        //ctor
        public ExpressionWindow()
        {
            InitializeComponent();

            CodeTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            CodeTextEditor.TextArea.TextEntered += CodeTextEditor_TextEntered;

            AceTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            AceTextEditor.TextArea.TextEntered += AceTextEditor_TextEntered;

            LanguageTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            LanguageTextEditor.TextArea.TextEntered += LanguageTextEditor_TextEntered;
        }

        //editor event 
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
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.Json).Where(x => x.Text.ToLower().Contains(text)).ToList();
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
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.Json).Where(x => x.Text.ToLower().Contains(text)).ToList();
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
            var methodsTokens = JavascriptParser.Insatnce.ParseJavascriptMethodCalls(CodeTextEditor.Text);
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
                    var data = CodeCompletionFactory.Insatnce.GetCompletionData(allTokens, CodeType.RuntimeJavascript).Where(x => x.Text.ToLower().Contains(text)).ToList();
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
        private void SaveExpressionButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ExpressionIdText.Text;
            var category = ExpressionCategoryText.Text;
            var returntype = ReturnTypeDropdown.Text;
            var translatedname = TranslatedName.Text;
            var isvariadic = TranslatedName.Text;
            var desc = DescriptionText.Text;

            if (_expressions.ContainsKey(id))
            {
                //TODO: error duplicate id
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
            expression.Language = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ExpressionLanguage, expression);
            expression.Code = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionCode, expression);

            _expressions.Add(id, expression);
            ExpressionListBox.Items.Refresh();
            ExpressionListBox.SelectedIndex = _expressions.Count - 1;
            NewExpressionWindow.IsOpen = false;
        }

        private void SaveParamButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ParamIdText.Text;
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
                LanguageTextEditor.Text = LanguageTextEditor.Text.Replace(@"	""params"": {", langTemplate);

                //code param
                var codeTemplate = TemplateHelper.AceCode(id, _selectedExpression.ScriptName);
                CodeTextEditor.Text = CodeTextEditor.Text.Replace($"{_selectedExpression.ScriptName}(", codeTemplate);
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

            NewParamWindow.IsOpen = false;
        }

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
                AppData.Insatnce.ErrorMessage("failed to remove condition, no condition selected");
            }
        }

        private void AddExpression_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedExpression == null) return;
            ExpressionIdText.Text = "expression-id";
            ExpressionCategoryText.Text = "custom";
            ReturnTypeDropdown.Text = "any";
            TranslatedName.Text = "Expression";
            DescriptionText.Text = "This is the expression description";
            NewExpressionWindow.IsOpen = true;
        }

        //list box events
        private void ExpressionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //save current selection
            if (_selectedExpression != null)
            {
                _selectedExpression.Ace = AceTextEditor.Text;
                _selectedExpression.Language = LanguageTextEditor.Text;
                _selectedExpression.Code = CodeTextEditor.Text;
                _expressions[_selectedExpression.Id] = _selectedExpression;
            }

            //load new selection
            _selectedExpression = ((KeyValuePair<string, Expression>)ExpressionListBox.SelectedItem).Value;
            AceTextEditor.Text = _selectedExpression.Ace;
            LanguageTextEditor.Text = _selectedExpression.Language;
            CodeTextEditor.Text = _selectedExpression.Code;
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
            throw new NotImplementedException();
        }

        private void FormatJsonLang_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FormatJsonAce_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //window states
        public void OnEnter()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                _expressions = AppData.Insatnce.CurrentAddon.Expressions;
                ExpressionListBox.ItemsSource = _expressions;

                if (_expressions.Any())
                {
                    ExpressionListBox.SelectedIndex = 0;
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

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                //save the current selected expression
                if (_selectedExpression != null)
                {
                    _selectedExpression.Ace = AceTextEditor.Text;
                    _selectedExpression.Language = LanguageTextEditor.Text;
                    _selectedExpression.Code = CodeTextEditor.Text;
                    _expressions[_selectedExpression.Id] = _selectedExpression;
                }

                AppData.Insatnce.CurrentAddon.Expressions = _expressions;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
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
    }
}
