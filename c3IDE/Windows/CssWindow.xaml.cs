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
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for CssWindow.xaml
    /// </summary>
    public partial class CssWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Css Style Sheet";
        public SearchPanel runtimePanel;

        public CssWindow()
        {
            InitializeComponent();

            CssTextEditor.Options.EnableHyperlinks = false;
            CssTextEditor.Options.EnableEmailHyperlinks = false;

            //setip ctrl-f to single page code find
            runtimePanel = SearchPanel.Install(CssTextEditor);
        }

        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(CssTextEditor, Syntax.Css);
            ThemeManager.SetupSearchPanel(runtimePanel);

            if (AddonManager.CurrentAddon != null)
            {
                CssTextEditor.Text = AddonManager.CurrentAddon.ThemeCss;
            }
        }

        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.ThemeCss = CssTextEditor.Text;
                AddonManager.SaveCurrentAddon();
            }
        }

        public void Clear()
        {
            CssTextEditor.Text = string.Empty;
        }

        public void ChangeTab(string tab, int lineNum)
        {
        }

        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {    
        }
    }
}
