using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using c3IDE.DataAccess;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Windows.Interfaces;
using c3IDE.Models;
using c3IDE.Templates;
using Action = c3IDE.Models.Action;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for ActionWindow.xaml
    /// </summary>
    public partial class ActionWindow : UserControl, IWindow
    {
        private Dictionary<string, Action> _actions;
        private Action _selectedAction;

        public string DisplayName { get; set; } = "Actions";

        public ActionWindow()
        {
            InitializeComponent();
        }

        private void AddAction_OnClick(object sender, RoutedEventArgs e)
        {
            ActionIdText.Text = "action-id";
            ActionCategoryText.Text = "custom";
            HighlightDropdown.Text = "false";
            DisplayText.Text = "This is the actions display text {0}";
            DescriptionText.Text = "This is the actions description";
            NewActionWindow.IsOpen = true;
        }

        private void RemoveAction_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedAction != null)
            {
                _actions.Remove(_selectedAction.Id);
                ActionListBox.Items.Refresh();
            }
            else
            {
                //todo: display notification
            }
        }

        public void OnEnter()
        {
            _actions = AppData.Insatnce.CurrentAddon.Actions;
            ActionListBox.ItemsSource = _actions;
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
                    _actions[_selectedAction.Id] = _selectedAction;
                }

                AppData.Insatnce.CurrentAddon.Actions = _actions;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }          
        }

        private void ActionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //save current selection
            if (_selectedAction != null)
            {
                _selectedAction.Ace = AceTextEditor.Text;
                _selectedAction.Language = LanguageTextEditor.Text;
                _selectedAction.Code = CodeTextEditor.Text;
                _actions[_selectedAction.Id] = _selectedAction;
            }

            //load new selection
            _selectedAction = ((KeyValuePair<string, Action>)ActionListBox.SelectedItem).Value;
            AceTextEditor.Text = _selectedAction.Ace;
            LanguageTextEditor.Text = _selectedAction.Language;
            CodeTextEditor.Text = _selectedAction.Code;
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

        private void SaveActionButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ActionIdText.Text;
            var category = ActionCategoryText.Text;
            var highlight = HighlightDropdown.Text;
            var displayText = DisplayText.Text;
            var desc = DescriptionText.Text;

            if (_actions.ContainsKey(id))
            {
                //todo: error duplicate id
            }

            var action = new Action
            {
                Id = id.Trim().ToLower(),
                Category = category.Trim().ToLower(),
                Highlight = highlight,
                DisplayText = displayText,
                Description = desc
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
                var codeTemplate = TemplateHelper.AceCode(id, _selectedAction.ScriptName);
                CodeTextEditor.Text =CodeTextEditor.Text.Replace($"{_selectedAction.ScriptName}(", codeTemplate);
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
                var codeTemplate = TemplateHelper.AceCodeFirst(id, _selectedAction.ScriptName);
               CodeTextEditor.Text = CodeTextEditor.Text.Replace($"{_selectedAction.ScriptName}()", codeTemplate);
            }

            NewParamWindow.IsOpen = false;
        }
    }
}
