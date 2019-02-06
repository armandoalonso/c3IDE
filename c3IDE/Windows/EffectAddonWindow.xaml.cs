using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Utilities.ThemeEngine;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for EffectAddonWindow.xaml
    /// </summary>
    public partial class EffectAddonWindow : UserControl, IWindow
    {
        public EffectAddonWindow()
        {
            InitializeComponent();
            AddonTextEditor.Options.EnableEmailHyperlinks = false;
            AddonTextEditor.Options.EnableHyperlinks = false;
        }

        public string DisplayName { get; set; } = "Addon";
        public void OnEnter()
        {
            AppData.Insatnce.SetupTextEditor(AddonTextEditor, Syntax.Json);

            if (AppData.Insatnce.CurrentAddon != null)
            {
                AddonTextEditor.Text = AppData.Insatnce.CurrentAddon.AddonJson;
            }
            else
            {
                AddonTextEditor.Text = string.Empty;
            }
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.AddonJson = AddonTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
        }

        public void Clear()
        {
            AddonTextEditor.Text = string.Empty;
        }


        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void FormatJsonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            AddonTextEditor.Text = AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text);
        }
        private void AddParameterMenu_Click(object sender, RoutedEventArgs e)
        {
            NewPropertyWindow.IsOpen = true;
            ParameterIdText.Text = "test-parameter";
            ParameterTypeDropdown.Text = "color";
            ParameterUniformText.Text = "setColor";
            ParameterNameText.Text = "Name";
            ParameterDescText.Text = "Description";
        }

        private void AddParameterButton_Click(object sender, RoutedEventArgs e)
        {
            var id = ParameterIdText.Text;
            var type = ParameterTypeDropdown.Text;
            var uniform = ParameterUniformText.Text;
            var name = ParameterNameText.Text;
            var desc = ParameterDescText.Text;
            string template;

            //check for duplicate property id
            var propertyRegex = new Regex(@"{""id"":""(?<id>.*)"",""type"":""(?<type>.*)"",""initial-value"":(?<val>.*),""uniform"":""(?<uniform>.*)""}", RegexOptions.IgnorePatternWhitespace );

            var compress = FormatHelper.Insatnce.JsonCompress(AddonTextEditor.Text);
            var propertyMatches = propertyRegex.Matches(compress);
            var firstProperty = propertyMatches.Count == 0;

            foreach (Match propertyMatch in propertyMatches)
            {
                if (propertyMatch.Groups["id"].ToString() == id)
                {
                    AppData.Insatnce.ErrorMessage("cannot have duplicate parameter id.");
                    return;
                }
            }

            var comma = firstProperty ? string.Empty : ",";
            switch (type)
            {
                case "float":
                    template = $@"{{""id"":""{id}"",""type"": ""{type}"",""initial-value"":1,""uniform"": ""{uniform}""}}{comma}";
                    break;

                case "color":
                    template = $@"{{""id"":""{id}"",""type"": ""{type}"",""initial-value"":@COLOR@,""uniform"": ""{uniform}""}}{comma}";
                    break;

                case "percent":
                    template = $@"{{""id"":""{id}"",""type"": ""{type}"",""initial-value"":100,""uniform"": ""{uniform}""}}{comma}";
                    break;

                default:
                    throw new ArgumentException();
            }

            AddonTextEditor.Text = FormatHelper.Insatnce.Json(compress.Replace("parameters\":[", $"parameters\":[{template}")).Replace("@COLOR@", "[1,0,0]");
            NewPropertyWindow.IsOpen = false;
        }


    }
}
