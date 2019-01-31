using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using c3IDE.Utilities.SyntaxHighlighting;
using MahApps.Metro;

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
                        SyntaxBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA")),
                        SyntaxForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#959DA6")),
                        ApplicationTheme = ThemeManager.GetTheme("Light.Blue"),
                        TextBoxForground = new SolidColorBrush(Colors.Black),
                        AutoCompleteBackground = new SolidColorBrush(Color.FromRgb(240,240,240)),
                        ListBoxBorderColor = new SolidColorBrush(Colors.Black)
                    };
                case ThemeTypes.Monokai:
                    return new Theme
                    {
                        Name = "Monokai Theme",
                        JavascriptSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Monokai Theme", Syntax.Javascript),
                        JsonSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Monokai Theme", Syntax.Json),
                        SyntaxBackgroundColor = new SolidColorBrush(Color.FromRgb(39,40,34)),
                        SyntaxForegroundColor = new SolidColorBrush(Color.FromRgb(248,248,240)),
                        ApplicationTheme = ThemeManager.GetTheme("Dark.Blue"),
                        TextBoxForground = new SolidColorBrush(Colors.White),
                        AutoCompleteBackground = new SolidColorBrush(Color.FromRgb(50, 50, 50)),
                        ListBoxBorderColor = new SolidColorBrush(Colors.White)
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
