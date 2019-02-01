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
                        SyntaxBackgroundColor = new SolidColorBrush(Colors.White),
                        SyntaxForegroundColor = new SolidColorBrush(Colors.Black),
                        ApplicationTheme = ThemeManager.GetTheme("Light.Blue"),
                        TextBoxForground = new SolidColorBrush(Colors.Black),
                        AutoCompleteBackground = new SolidColorBrush(Color.FromRgb(200,200,200)),
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
                case ThemeTypes.AyuLight:
                    return new Theme
                    {
                        Name = "Ayu Light Theme",
                        JavascriptSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Ayu Light Theme", Syntax.Javascript),
                        JsonSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Ayu Light Theme", Syntax.Json),
                        SyntaxBackgroundColor = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
                        SyntaxForegroundColor = new SolidColorBrush(Color.FromRgb(108,118,128)),
                        ApplicationTheme = ThemeManager.GetTheme("Light.Blue"),
                        TextBoxForground = new SolidColorBrush(Colors.Black),
                        AutoCompleteBackground = new SolidColorBrush(Color.FromRgb(210, 210, 210)),
                        ListBoxBorderColor = new SolidColorBrush(Color.FromRgb(108, 118, 128))
                    };
                case ThemeTypes.AyuMirage:
                    return new Theme
                    {
                        Name = "Ayu Mirage Theme",
                        JavascriptSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Ayu Mirage Theme", Syntax.Javascript),
                        JsonSyntaxTheme = SyntaxHighlighting.SyntaxHighligtResolver.Insatnce.Resolve("Ayu Mirage Theme", Syntax.Json),
                        SyntaxBackgroundColor = new SolidColorBrush(Color.FromRgb(31, 36, 48)),
                        SyntaxForegroundColor = new SolidColorBrush(Color.FromRgb(203, 204, 198)),
                        ApplicationTheme = ThemeManager.GetTheme("Light.Blue"),
                        TextBoxForground = new SolidColorBrush(Color.FromRgb(50, 50, 50)),
                        AutoCompleteBackground = new SolidColorBrush(Color.FromRgb(190, 190, 190)),
                        ListBoxBorderColor = new SolidColorBrush(Color.FromRgb(108, 118, 128))
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public enum ThemeTypes
    {
        DefaultTheme = 0,
        Monokai = 1,
        AyuLight = 2,
        AyuMirage = 3
    }
}
