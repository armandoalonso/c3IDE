using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit.Highlighting;

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

        //application theme
        public SolidColorBrush ApplicationBackgroundColor { get; set; } = new SolidColorBrush(Colors.White);
        public SolidColorBrush ApplicationForegroundColor { get; set; } = new SolidColorBrush(Colors.Black);
    }
}
    