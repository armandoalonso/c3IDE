using System.Windows.Controls;
using c3IDE.DataAccess;
using c3IDE.Managers;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for EffectLanguageWindow.xaml
    /// </summary>
    public partial class EffectLanguageWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Language";

        public EffectLanguageWindow()
        {
            InitializeComponent();
            LanguageTextEditor.Options.EnableEmailHyperlinks = false;
            LanguageTextEditor.Options.EnableHyperlinks = false;
        }

        public void OnEnter()
        {
           ThemeManager.SetupTextEditor(LanguageTextEditor, Syntax.Json);

            if (AddonManager.CurrentAddon != null)
            {
                LanguageTextEditor.Text = AddonManager.CurrentAddon.EffectLanguage;
            }
            else
            {
                LanguageTextEditor.Text = string.Empty;
            }
        }

        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.EffectLanguage = LanguageTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AddonManager.CurrentAddon);
            }
        }

        public void Clear()
        {
            LanguageTextEditor.Text = string.Empty;
        }

    }
}
