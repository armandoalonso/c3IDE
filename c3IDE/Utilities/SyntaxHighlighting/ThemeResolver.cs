using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace c3IDE.Utilities.SyntaxHighlighting
{
    public class ThemeResolver : Singleton<ThemeResolver>
    {
        public Dictionary<string, SolidColorBrush> colorCache = new Dictionary<string, SolidColorBrush>();

        public SolidColorBrush ResolveBackground(string key)
        {
            if (colorCache.ContainsKey(key))
            {
                return colorCache[key];
            }

            SolidColorBrush c;
            switch (key)
            {
                case "Default Theme":
                    c = new SolidColorBrush(Colors.White);
                    break;
                case "Test Theme":
                    c =  new SolidColorBrush(Color.FromRgb(30,30,30));
                    break;
                default:
                    c = new SolidColorBrush(Colors.White);
                    break;
            }

            colorCache.Add(key, c);
            return c;
        }

        public Brush ResolveFormBackground(string key)
        {
            SolidColorBrush c;
            switch (key)
            {
                case "Default Theme":
                    c = new SolidColorBrush(Colors.White);
                    break;
                case "Test Theme":
                    c = new SolidColorBrush(Color.FromRgb(70, 70, 70));
                    break;
                default:
                    c = new SolidColorBrush(Colors.White);
                    break;
            }

            return c;
        }
    }
}
