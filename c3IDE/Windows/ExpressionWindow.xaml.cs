using System;
using System.Collections.Generic;
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
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Windows.Interfaces;
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

        public ExpressionWindow()
        {
            InitializeComponent();
        }

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
                //todo: error duplicate id
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
                AceTextEditor.Text = AceTextEditor.Text.Replace("    \"params\": [", aceTemplate);

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
                AceTextEditor.Text = AceTextEditor.Text.Replace("}", aceTemplate);

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
                ExpressionListBox.Items.Refresh();
            }
            else
            {
                //todo: display notification
            }
        }

        private void AddExpression_OnClick(object sender, RoutedEventArgs e)
        {
            ExpressionIdText.Text = "expression-id";
            ExpressionCategoryText.Text = "custom";
            ReturnTypeDropdown.Text = "any";
            TranslatedName.Text = "Expression";
            DescriptionText.Text = "This is the expression description";
            NewExpressionWindow.IsOpen = true;
        }

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

        private void InsertNewParam(object sender, RoutedEventArgs e)
        {
            ParamIdText.Text = "param-id";
            ParamTypeDropdown.Text = "number";
            ParamValueText.Text = string.Empty;
            ParamNameText.Text = "This is the parameters name";
            ParamDescText.Text = "This is the parameters description";
            NewParamWindow.IsOpen = true;
        }

        public void OnEnter()
        {
            _expressions = AppData.Insatnce.CurrentAddon.Expressions;
            ExpressionListBox.ItemsSource = _expressions;
        }

        public void OnExit()
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
        }
    }
}
