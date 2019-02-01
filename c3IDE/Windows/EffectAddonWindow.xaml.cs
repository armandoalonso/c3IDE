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
           if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AppData.Insatnce.GlobalSave();
            }
        }

        private void FormatJsonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            AddonTextEditor.Text = AddonTextEditor.Text = FormatHelper.Insatnce.Json(AddonTextEditor.Text);
        }
    }
}
