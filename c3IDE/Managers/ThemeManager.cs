using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Search;

namespace c3IDE.Managers
{
    public static class ThemeManager
    {
        /// <summary>
        /// sets up the syntax highlight & fonts for text editors
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="syntax"></param>
        public static void SetupTextEditor(TextEditor editor, Syntax syntax)
        {
            editor.FontSize = OptionsManager.CurrentOptions.FontSize;
            editor.FontFamily = new FontFamily(OptionsManager.CurrentOptions.FontFamily);
            var syntaxDefinition = syntax == Syntax.Javascript
                ? OptionsManager.CurrentOptions.ApplicationTheme.JavascriptSyntaxTheme
                : OptionsManager.CurrentOptions.ApplicationTheme.JsonSyntaxTheme;
            editor.SyntaxHighlighting = syntaxDefinition;
            editor.Background = OptionsManager.CurrentOptions.ApplicationTheme.SyntaxBackgroundColor;
            editor.Foreground = OptionsManager.CurrentOptions.ApplicationTheme.SyntaxForegroundColor;
        }

        /// <summary>
        /// sets up the color of the highlighted word in each search panel
        /// </summary>
        /// <param name="panels"></param>
        public static void SetupSearchPanel(params SearchPanel[] panels)
        {
            foreach (var searchPanel in panels)
            {
                searchPanel.MarkerBrush = OptionsManager.CurrentOptions.ApplicationTheme.HighlightColor;
            }
        }

        /// <summary>
        /// setups up the application theme for the metro windwo & controls
        /// </summary>
        public static void SetupTheme()
        {
            //change window style
           MahApps.Metro.ThemeManager.ChangeTheme(Application.Current, OptionsManager.CurrentOptions.ApplicationTheme.ApplicationTheme);

            //change textbox style
            var txtboxStyle = new Style { TargetType = typeof(TextBlock) };
            txtboxStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, OptionsManager.CurrentOptions.ApplicationTheme.TextBoxForground));
            Application.Current.Resources["TextBlockStyle"] = txtboxStyle;

            //change auto completion window style
            var autoCompleteStyle = new Style { TargetType = typeof(CompletionListBox) };
            autoCompleteStyle.Setters.Add(new Setter(CompletionListBox.BackgroundProperty, OptionsManager.CurrentOptions.ApplicationTheme.AutoCompleteBackground));
            Application.Current.Resources[typeof(CompletionListBox)] = autoCompleteStyle;

            //list box item bg
            var listboxItemStyle = new Style { TargetType = typeof(ListBoxItem) };
            listboxItemStyle.Setters.Add(new Setter(ListBoxItem.BackgroundProperty, OptionsManager.CurrentOptions.ApplicationTheme.AutoCompleteBackground));
            Application.Current.Resources[typeof(ListBoxItem)] = listboxItemStyle;

            //change listbox border & background style
            Application.Current.Resources["ListBoxBorder"] = OptionsManager.CurrentOptions.ApplicationTheme.ListBoxBorderColor;
            Application.Current.Resources["ListBoxBackColor"] = OptionsManager.CurrentOptions.ApplicationTheme.AutoCompleteBackground;
        }
    }
}
