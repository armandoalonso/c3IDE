using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using c3IDE.Utilities.SyntaxHighlighting;

namespace c3IDE.Utilities.ThemeEngine
{
    public class ThemeFactory : Singleton<ThemeFactory>
    {
        public Theme GetTheme(ThemeTypes type)
        {
            switch (type)
            {
                case ThemeTypes.DefaultTheme:
                    return new Theme
                    {
                        Name = "Default Theme",
                        JavascriptSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Default Theme", Syntax.Javascript),
                        JsonSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Default Theme", Syntax.Json),
                        SyntaxBackgroundColor = new SolidColorBrush(Colors.White),
                        SyntaxForegroundColor = new SolidColorBrush(Colors.Black),

                        ApplicationForegroundColor = new SolidColorBrush(Colors.Black),
                        ApplicationBackgroundColor = new SolidColorBrush(Colors.White)
                    };
                case ThemeTypes.Monokai:
                    return new Theme
                    {
                        Name = "Monokai Theme",
                        JavascriptSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Monokai Theme", Syntax.Javascript),
                        JsonSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Monokai Theme", Syntax.Json),
                        SyntaxBackgroundColor = new SolidColorBrush(Color.FromRgb(39,40,34)),
                        SyntaxForegroundColor = new SolidColorBrush(Color.FromRgb(248,248,240)),

                        ApplicationForegroundColor = new SolidColorBrush(Colors.White),
                        ApplicationBackgroundColor = new SolidColorBrush(Color.FromRgb(50, 50, 54)),
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public enum ThemeTypes
    {
        DefaultTheme = 0,
        Monokai = 1

    }
}
