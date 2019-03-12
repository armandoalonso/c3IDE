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
using c3IDE.Managers;
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
        public string DisplayName { get; set; } = "Addon";

        /// <summary>
        /// effect addon constructor
        /// </summary>
        public EffectAddonWindow()
        {
            InitializeComponent();

            AddonTextEditor.Options.EnableEmailHyperlinks = false;
            AddonTextEditor.Options.EnableHyperlinks = false;
        }

        /// <summary>
        /// handles the effect addon getting focus
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(AddonTextEditor, Syntax.Json);

            if (AddonManager.CurrentAddon != null)
            {
                AddonTextEditor.Text = AddonManager.CurrentAddon.AddonJson;
            }
            else
            {
                AddonTextEditor.Text = string.Empty;
            }
        }

        /// <summary>
        /// handles the effect addon window losing focus
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.AddonJson = AddonTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AddonManager.CurrentAddon);
            }
        }

        /// <summary>
        /// clear all inputs from the effect addon window
        /// </summary>
        public void Clear()
        {
            AddonTextEditor.Text = string.Empty;
        }

        /// <summary>
        /// handle all keyboard shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// formats effect addon as json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatJsonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            AddonTextEditor.Text = AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text);
        }

        /// <summary>
        /// opens the add effect parameter window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddParameterMenu_Click(object sender, RoutedEventArgs e)
        {
            NewPropertyWindow.IsOpen = true;
            ParameterIdText.Text = "test-parameter";
            ParameterTypeDropdown.Text = "color";
            ParameterUniformText.Text = "setColor";
            ParameterNameText.Text = "Name";
            ParameterDescText.Text = "Description";
        }

        /// <summary>
        /// handles the save button to save effect parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    NotificationManager.PublishErrorNotification("cannot have duplicate parameter id.");
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
                    template = $@"{{""id"":""{id}"",""type"": ""{type}"",""initial-value"":[1,0,0],""uniform"": ""{uniform}""}}{comma}";
                    break;

                case "percent":
                    template = $@"{{""id"":""{id}"",""type"": ""{type}"",""initial-value"":100,""uniform"": ""{uniform}""}}{comma}";
                    break;

                default:
                    throw new ArgumentException();
            }

            AddonTextEditor.Text = FormatHelper.Insatnce.Json(compress.Replace("parameters\":[", $"parameters\":[{template}"));
            AddonManager.CurrentAddon.EffectAddon = AddonTextEditor.Text;
            
            //todo: need to update effect language maybe redesign effect layouts

            NewPropertyWindow.IsOpen = false;
        }


    }
}
