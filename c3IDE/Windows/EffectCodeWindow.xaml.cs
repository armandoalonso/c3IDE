using System.Windows.Controls;
using System.Windows.Input;
using c3IDE.DataAccess;
using c3IDE.Managers;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for EffectCodeWindow.xaml
    /// </summary>
    public partial class EffectCodeWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Effect";

        public EffectCodeWindow()
        {
            InitializeComponent();
            EffectPluginTextEditor.Options.EnableEmailHyperlinks = false;
            EffectPluginTextEditor.Options.EnableHyperlinks = false;
        }

        /// <summary>
        /// handles when the effect code window gets focus
        /// </summary>
        public void OnEnter()
        {
            if (AddonManager.CurrentAddon != null)
            {
                EffectPluginTextEditor.Text = AddonManager.CurrentAddon.EffectCode;
            }
            else
            {
                EffectPluginTextEditor.Text = string.Empty;
            }
        }

        /// <summary>
        /// handles when the effect code window loses focus
        /// </summary>
        public void OnExit()
        {
            ThemeManager.SetupTextEditor(EffectPluginTextEditor, Syntax.Javascript);

            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.EffectCode = EffectPluginTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AddonManager.CurrentAddon);
            }
        }

        /// <summary>
        /// clears all inputs on effect code window
        /// </summary>
        public void Clear()
        {

        }

        /// <summary>
        /// handles keyboard shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
