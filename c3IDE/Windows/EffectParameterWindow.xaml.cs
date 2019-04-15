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
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Action = c3IDE.Models.Action;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for EffectParameter.xaml
    /// </summary>
    public partial class EffectParameterWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Parameters";
        private Dictionary<string, EffectParameter> _params;
        private EffectParameter _selectedParam;
        //private CompletionWindow completionWindow;

        public EffectParameterWindow()
        {
            InitializeComponent();

            AddonTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            AddonTextEditor.TextArea.TextEntered += AddonTextEditor_TextEntered;
            LangTextEditor.TextArea.TextEntering += TextEditor_TextEntering;
            LangTextEditor.TextArea.TextEntered += LangTextEditor_TextEntered;

            AddonTextEditor.Options.EnableEmailHyperlinks = false;
            AddonTextEditor.Options.EnableHyperlinks = false;
            LangTextEditor.Options.EnableEmailHyperlinks = false;
            LangTextEditor.Options.EnableHyperlinks = false;
        }

        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(AddonTextEditor, Syntax.Json);
            ThemeManager.SetupTextEditor(LangTextEditor, Syntax.Json);

            if (AddonManager.CurrentAddon != null)
            {
                _params = AddonManager.CurrentAddon.Effect.Parameters;
                ParameterListBox.ItemsSource = _params;

                if (_params.Any())
                {
                    ParameterListBox.SelectedIndex = 0;
                    _selectedParam = _params.Values.First();
                    AddonTextEditor.Text = _selectedParam.Json;
                    LangTextEditor.Text = _selectedParam.Lang;
                }
            }
            else
            {
                ParameterListBox.ItemsSource = null;
                AddonTextEditor.Text = string.Empty;
                LangTextEditor.Text = string.Empty;
            }
        }

        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                if (_selectedParam != null)
                {
                    _selectedParam.Json = AddonTextEditor.Text;
                    _selectedParam.Lang = LangTextEditor.Text;
                   
                    _params[_selectedParam.Key] = _selectedParam;
                }

                AddonManager.CurrentAddon.Effect.Parameters = _params;
                AddonManager.SaveCurrentAddon();
            }
        }

        public void Clear()
        {
            _params = new Dictionary<string, EffectParameter>();
            _selectedParam = null;
            ParameterListBox.ItemsSource = null;
            AddonTextEditor.Text = string.Empty;
            LangTextEditor.Text = string.Empty;
        }

        public void ChangeTab(string tab, int lineNum)
        {

        }

        private async void AddFloatParameter_OnClick(object sender, RoutedEventArgs e)
        {
            var id = await WindowManager.ShowInputDialog("New Float Effect Parameter", "float parameter id", "float-id");
            if (string.IsNullOrWhiteSpace(id)) return;

            if (_params.ContainsKey(id))
            {
                NotificationManager.PublishErrorNotification("failed to add parameter, parameter id already exists");
                return;
            }

            var param = new EffectParameter {Key = id.Replace(" ", "-")};
            param.Json = $@"{{
    ""id"":""{id}"",
    ""type"": ""float"",
    ""initial-value"":0,
    ""uniform"": ""{param.Uniform}""
}}";
            param.Lang = $@"""{id}"": {{
    ""name"": ""{param.Uniform}"",
    ""desc"": ""{param.Uniform}""
}}";
            param.VariableDeclaration = $"uniform lowp float {param.Uniform};";

            _params.Add(id, param);
            ParameterListBox.Items.Refresh();
            ParameterListBox.SelectedIndex = _params.Count - 1;

            AddonManager.CurrentAddon.Effect.Parameters = _params;
        }

        private async void AddColorParameter_OnClick(object sender, RoutedEventArgs e)
        {
            var id = await WindowManager.ShowInputDialog("New COlor Effect Parameter", "color parameter id", "color-id");
            if (string.IsNullOrWhiteSpace(id)) return;


            if (_params.ContainsKey(id))
            {
                NotificationManager.PublishErrorNotification("failed to add parameter, parameter id already exists");
                return;
            }

            var param = new EffectParameter { Key = id.Replace(" ", "-") };
            param.Json =$@"{{
    ""id"":""{id}"",
    ""type"": ""color"",
    ""initial-value"":[0,0,0],
    ""uniform"": ""{param.Uniform}""
}}";
            param.Lang = $@"""{id}"": {{
    ""name"": ""{param.Uniform}"",
    ""desc"": ""{param.Uniform}""
}}";
            param.VariableDeclaration = $"uniform lowp vec3 {param.Uniform};";

            _params.Add(id, param);
            ParameterListBox.Items.Refresh();
            ParameterListBox.SelectedIndex = _params.Count - 1;

            AddonManager.CurrentAddon.Effect.Parameters = _params;
        }

        private async void AddPercentParameter_OnClick(object sender, RoutedEventArgs e)
        {
            var id = await WindowManager.ShowInputDialog("New Percent Effect Parameter", "percent parameter id", "percent-id");
            if (string.IsNullOrWhiteSpace(id)) return;


            if (_params.ContainsKey(id))
            {
                NotificationManager.PublishErrorNotification("failed to add parameter, parameter id already exists");
                return;
            }

            var param = new EffectParameter { Key = id.Replace(" ", "-") };
            param.Json = $@"{{
    ""id"":""{id}"",
    ""type"": ""percent"",
    ""initial-value"":0,
    ""uniform"": ""{param.Uniform}""
}}";
            param.Lang = $@"""{id}"": {{
    ""name"": ""{param.Uniform}"",
    ""desc"": ""{param.Uniform}""
}}";
            param.VariableDeclaration = $"uniform lowp float {param.Uniform};";

            _params.Add(id, param);
            ParameterListBox.Items.Refresh();
            ParameterListBox.SelectedIndex = _params.Count - 1;

            AddonManager.CurrentAddon.Effect.Parameters = _params;
        }

        private void RemoveParameter_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedParam != null)
            {
                _params.Remove(_selectedParam.Key);
                ParameterListBox.ItemsSource = _params;
                ParameterListBox.Items.Refresh();

                AddonTextEditor.Text = string.Empty;
                LangTextEditor.Text = string.Empty;
                _selectedParam = null;
            }
            else
            {
                NotificationManager.PublishErrorNotification("failed to remove parameter, no parameter selected");
            }
        }

        private void ParameterListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParameterListBox.SelectedIndex == -1)
            {
                return;
            }

            if (_selectedParam != null)
            {
                _selectedParam.Json = AddonTextEditor.Text;
                _selectedParam.Lang = LangTextEditor.Text;
                _params[_selectedParam.Key] = _selectedParam;
                AddonManager.CurrentAddon.Effect.Parameters = _params;
                AddonManager.SaveCurrentAddon();
            }

            var selectedKey = ((KeyValuePair<string, EffectParameter>)ParameterListBox.SelectedItem).Key;
            _selectedParam = _params[selectedKey];

            AddonTextEditor.Text = _selectedParam.Json;
            LangTextEditor.Text = _selectedParam.Lang;
        }

        //todo - add code completion
        private void TextEditor_TextEntering(object sender, TextCompositionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //todo - add code completion
        private void LangTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //todo - add code completion
        private void AddonTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //todo - add find and replace
        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void FormatJsonEffect_OnClick(object sender, RoutedEventArgs e)
        {
            AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text);
        }

        private void FormatLangEffect_OnClick(object sender, RoutedEventArgs e)
        {
            LangTextEditor.Text = FormatHelper.Insatnce.Json(LangTextEditor.Text, true);
        }
    }
}
