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
    /// Interaction logic for EffectLanguageWindow.xaml
    /// </summary>
    public partial class EffectLanguageWindow : UserControl, IWindow
    {
        public EffectLanguageWindow()
        {
            InitializeComponent();
            LanguageTextEditor.Options.EnableEmailHyperlinks = false;
            LanguageTextEditor.Options.EnableHyperlinks = false;
        }

        public string DisplayName { get; set; } = "Language";
        public void OnEnter()
        {
            AppData.Insatnce.SetupTextEditor(LanguageTextEditor, Syntax.Json);

            if (AppData.Insatnce.CurrentAddon != null)
            {
                LanguageTextEditor.Text = AppData.Insatnce.CurrentAddon.EffectLanguage;
            }
            else
            {
                LanguageTextEditor.Text = string.Empty;
            }
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.EffectLanguage = LanguageTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
        }

        public void Clear()
        {
            LanguageTextEditor.Text = string.Empty;
        }

    }
}
