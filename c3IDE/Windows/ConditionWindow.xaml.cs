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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using c3IDE.DataAccess;
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Windows.Interfaces;
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

        public ConditionWindow()
        {
            InitializeComponent();
        }

        private void SaveConditionButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ConditionIdText.Text;
            var category = ConditionCategoryText.Text;
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
                //todo: error duplicate id
            }

            var cnd = new Condition
            {
                Id = id.Trim().ToLower(),
                Category = category.Trim().ToLower(),
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

            //todo: condition templates (using action templates)
            cnd.Ace = TemplateHelper.CndAce(cnd);
            cnd.Language = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionLanguage, cnd);
            cnd.Code = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionCode, cnd);

            _conditions.Add(id, cnd);
            ConditionListBox.Items.Refresh();
            ConditionListBox.SelectedIndex = _conditions.Count - 1;
            NewConditionWindow.IsOpen = false;
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

        public void OnEnter()
        {
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

        private void AddCondition_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedCondition == null) return;
            ConditionIdText.Text = "condition-id";
            ConditionCategoryText.Text = "custom";
            DisplayText.Text = "This is the conditions display text {0}";
            DescriptionText.Text = "This is the conditions description";
            NewConditionWindow.IsOpen = true;
        }

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
            //todo: error here when removing with 2 configured
            _selectedCondition = ((KeyValuePair<string, Condition>)ConditionListBox.SelectedItem).Value;
            AceTextEditor.Text = _selectedCondition.Ace;
            LanguageTextEditor.Text = _selectedCondition.Language;
           CodeTextEditor.Text = _selectedCondition.Code;
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
    }
}
    