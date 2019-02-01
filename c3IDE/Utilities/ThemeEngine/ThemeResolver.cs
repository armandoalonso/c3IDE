using System;
using System.Collections.Generic;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace c3IDE.Utilities.ThemeEngine
{
    public class ThemeResolver : Singleton<ThemeResolver>
    {
        public Dictionary<string, Theme> themeCache = new Dictionary<string, Theme>();

        public Theme Resolve(string key)
        {
            if (themeCache.ContainsKey(key))
            {
                return themeCache[key];
            }

            Theme theme = ThemeFactory.Insatnce.GetTheme(ThemeTypes.DefaultTheme);
            switch (key)
            {
                case "Default Theme":
                    themeCache.Add(key, theme);
                    break;
                case "Monokai Theme":
                    theme = ThemeFactory.Insatnce.GetTheme(ThemeTypes.Monokai);
                    themeCache.Add(key, theme);
                    break;
                case "Ayu Light Theme":
                    theme = ThemeFactory.Insatnce.GetTheme(ThemeTypes.AyuLight);
                    themeCache.Add(key, theme);
                    break;
                case "Ayu Mirage Theme":
                    theme = ThemeFactory.Insatnce.GetTheme(ThemeTypes.AyuMirage);
                    themeCache.Add(key, theme);
                    break;
            }

            return theme;
        }

       
    }
}
