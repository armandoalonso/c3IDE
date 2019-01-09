using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Windows.Interfaces;
using c3IDE.Models;
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

        private async void AddAction_OnClick(object sender, RoutedEventArgs e)
        {
            var id = await AppData.Insatnce.ShowInputDialog("Action ID", "insert action id", "do-alert");
            var category = await AppData.Insatnce.ShowInputDialog("Action Category", "insert action category", "custom");

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine(@"Invalid id or category for action");
                return;
            }

            var action = new Action
            {
                Id = id.Trim().ToLower(),
                Category = category.Trim().ToLower()
            };

            action.Ace = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionAces, action);
            action.Language = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionLanguage, action);
            action.Code = TemplateCompiler.Insatnce.CompileTemplates(AppData.Insatnce.CurrentAddon.Template.ActionCode, action);

            _actions.Add(id, action);
            ActionListBox.Items.Refresh();
        }

        private void RemoveAction_OnClick(object sender, RoutedEventArgs e)
        {

        }

        public void OnEnter()
        {
            _actions = AppData.Insatnce.CurrentAddon.Actions;
            ActionListBox.ItemsSource = _actions;
        }

        public void OnExit()
        {
            //save the current selected action
            if (_selectedAction != null)
            {
                _selectedAction.Ace = ActionAceTextEditor.Text;
                _selectedAction.Language = ActionLanguageTextEditor.Text;
                _selectedAction.Code = ActionCodeTextEditor.Text;
                _actions[_selectedAction.Id] = _selectedAction;
            }

            AppData.Insatnce.CurrentAddon.Actions = _actions;
        }

        private void ActionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //save current selection
            if (_selectedAction != null)
            {
                _selectedAction.Ace = ActionAceTextEditor.Text;
                _selectedAction.Language = ActionLanguageTextEditor.Text;
                _selectedAction.Code = ActionCodeTextEditor.Text;
                _actions[_selectedAction.Id] = _selectedAction;
            }

            //load new selection
            _selectedAction = ((KeyValuePair<string, Action>)ActionListBox.SelectedItem).Value;
            ActionAceTextEditor.Text = _selectedAction.Ace;
            ActionLanguageTextEditor.Text = _selectedAction.Language;
            ActionCodeTextEditor.Text = _selectedAction.Code;
        }


        //todo: streamline this whole process, create new dialog box for this, do the same for properties 
        //todo: look into avalon dock or sometype of document docking solution with tabs (sync fusion maybe...)
        private async void InsertNewParam(object sender, RoutedEventArgs e)
        {
            var id = await AppData.Insatnce.ShowInputDialog("Parameter ID", "insert param id", "param-id");

            if (ActionAceTextEditor.Text.Contains("\"params\": ["))
            {
                //there is at least one param defined
                var aceTemplate = $@"    ""params"": [
        {{
            ""id"": ""{id}"",
            ""type"": ""number""
        }},";

                ActionAceTextEditor.Text = ActionAceTextEditor.Text.Replace("    \"params\": [", aceTemplate);

                var langTemplate = $@"    ""params"": {{
        ""{id}"": {{
            ""name"": ""param name"",
            ""desc"": ""param description""
        }},";

                ActionLanguageTextEditor.Text = ActionLanguageTextEditor.Text.Replace(@"	""params"": {", langTemplate);

                var ti = new CultureInfo("en-US", false).TextInfo;
                var param = ti.ToTitleCase(id.Replace("-", " ").ToLower()).Replace(" ", string.Empty);
                param = char.ToLowerInvariant(param[0]) + param.Substring(1);

                var codeTemplate = $"{_selectedAction.ScriptName}({param}, ";

                ActionCodeTextEditor.Text = ActionCodeTextEditor.Text.Replace($"{_selectedAction.ScriptName}(", codeTemplate);
            }
            else
            {
                //this will be the first param

                var aceTemplate = $@"    ""params"": [
        {{
            ""id"": ""{id}"",
            ""type"": ""number""
        }}
    ]
}}";
                ActionAceTextEditor.Text = ActionAceTextEditor.Text.Replace("}", aceTemplate);

                var langTemplate = $@""",
	""params"": {{
        ""{id}"": {{
            ""name"": ""param name"",
            ""desc"": ""param description""
        }}
    }}
}}";

                ActionLanguageTextEditor.Text = ActionLanguageTextEditor.Text.Replace(@"""
}", langTemplate);

                var ti = new CultureInfo("en-US", false).TextInfo;
                var param = ti.ToTitleCase(id.Replace("-", " ").ToLower()).Replace(" ", string.Empty);
                param = char.ToLowerInvariant(param[0]) + param.Substring(1);

                var codeTemplate = $"{_selectedAction.ScriptName}({param})";

                ActionCodeTextEditor.Text = ActionCodeTextEditor.Text.Replace($"{_selectedAction.ScriptName}()", codeTemplate);
            }
        }

        
    }
}
