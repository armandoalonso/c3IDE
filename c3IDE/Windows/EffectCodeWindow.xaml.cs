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
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Utilities.ThemeEngine;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for EffectCodeWindow.xaml
    /// </summary>
    public partial class EffectCodeWindow : UserControl, IWindow
    {
        public EffectCodeWindow()
        {
            InitializeComponent();
            EffectPluginTextEditor.Options.EnableEmailHyperlinks = false;
            EffectPluginTextEditor.Options.EnableHyperlinks = false;
        }

        public string DisplayName { get; set; } = "Effect";
        public void OnEnter()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                EffectPluginTextEditor.Text = AppData.Insatnce.CurrentAddon.EffectCode;
            }
            else
            {
                EffectPluginTextEditor.Text = string.Empty;
            }
        }

        public void OnExit()
        {
            AppData.Insatnce.SetupTextEditor(EffectPluginTextEditor, Syntax.Javascript);

            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.EffectCode = EffectPluginTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
        }

        public void Clear()
        {

        }

        public void SetupTheme(Theme t)
        {
           
        }

        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AppData.Insatnce.GlobalSave();
            }
        }
    }
}
