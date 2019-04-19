using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit.Highlighting;
using MahApps.Metro;

namespace c3IDE.Utilities.ThemeEngine
{
    public class Theme
    {
        public string Name { get; set; } = "Default Theme";

        //syntax editor themes
        public IHighlightingDefinition JavascriptSyntaxTheme { get; set; } = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Default Theme", Syntax.Javascript);
        public IHighlightingDefinition JsonSyntaxTheme { get; set; } = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Default Theme", Syntax.Json);
        public SolidColorBrush SyntaxForegroundColor { get; set; } = new SolidColorBrush(Colors.Black);
        public SolidColorBrush SyntaxBackgroundColor { get; set; } = new SolidColorBrush(Colors.White);
        public MahApps.Metro.Theme ApplicationTheme { get; set; } = ThemeManager.GetTheme("Light.Blue");
        public SolidColorBrush TextBoxForground { get; set; } = new SolidColorBrush(Colors.Black);
        public SolidColorBrush AutoCompleteBackground { get; set; } = new SolidColorBrush(Color.FromRgb(240,240,240));
        public SolidColorBrush ListBoxBorderColor { get; set; } = new SolidColorBrush(Colors.Black);
        public SolidColorBrush HighlightColor { get; set; } = new SolidColorBrush(Color.FromRgb(73,72,62));
    }
}
    