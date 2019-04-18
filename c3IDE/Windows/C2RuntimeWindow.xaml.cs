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
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for C2RuntimeWindow.xaml
    /// </summary>
    public partial class C2RuntimeWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "C2Runtime";
        private SearchPanel runtimePanel;

        public C2RuntimeWindow()
        {
            InitializeComponent();

            C2RuntimeTextEditor.TextArea.TextEntered += C2RuntimeTextEditor_TextEntered;
            C2RuntimeTextEditor.Options.EnableHyperlinks = false;
            C2RuntimeTextEditor.Options.EnableEmailHyperlinks = false;

            //setip ctrl-f to single page code find
            runtimePanel = SearchPanel.Install(C2RuntimeTextEditor);
        }

        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(C2RuntimeTextEditor, Syntax.Javascript);
            ThemeManager.SetupSearchPanel(runtimePanel);

            if (AddonManager.CurrentAddon != null)
            {
                C2RuntimeTextEditor.Text = AddonManager.CurrentAddon.C2RunTime;
            }
        }

        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.C2RunTime = C2RuntimeTextEditor.Text;
                AddonManager.SaveCurrentAddon();
            }
        }

        public void Clear()
        {
            C2RuntimeTextEditor.Text = string.Empty;
        }

        public void ChangeTab(string tab, int lineNum)
        {
          
        }

        private void C2RuntimeTextEditor_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Text)) return;

            //limited code completion
            TextEditorHelper.Insatnce.MatchSymbol(C2RuntimeTextEditor, e.Text);
        }

        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                //AppData.Insatnce.GlobalSave(false);
                Searcher.Insatnce.UpdateFileIndex("c2runtime.js", C2RuntimeTextEditor.Text, ApplicationWindows.C2Runtime);
                var editor = ((TextEditor)sender);
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }

        private void FormatJavascriptRuntime_OnClick(object sender, RoutedEventArgs e)
        {
            C2RuntimeTextEditor.Text = FormatHelper.Insatnce.Javascript(C2RuntimeTextEditor.Text);
        }

        private void FindGlobal_Click(object sender, RoutedEventArgs e)
        {
            //AppData.Insatnce.GlobalSave(false);
            Searcher.Insatnce.UpdateFileIndex("c2runtime.js", C2RuntimeTextEditor.Text, ApplicationWindows.C2Runtime);

            MenuItem mnu = sender as MenuItem;
            TextEditor editor = null;

            if (mnu != null)
            {
                editor = ((ContextMenu)mnu.Parent).PlacementTarget as TextEditor;
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }
    }
}
