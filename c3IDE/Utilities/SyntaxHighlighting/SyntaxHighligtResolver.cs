using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;

namespace c3IDE.Utilities.SyntaxHighlighting
{
    public class SyntaxHighligtResolver : Singleton<SyntaxHighligtResolver>
    {
        public Dictionary<string, IHighlightingDefinition> Cache = new Dictionary<string, IHighlightingDefinition>();

        public IHighlightingDefinition Resolve(string key, Syntax syntax)
        {
            var syntaxKey = "Javascript";
            switch (syntax)
            {
                case Syntax.Javascript: syntaxKey = "Javascript"; break;
                case Syntax.Json: syntaxKey = "Json"; break;
                case Syntax.Css: syntaxKey = "CSS"; break;
            }


            var themeKey = key + " " + syntaxKey;

            if (Cache.ContainsKey(themeKey))
            {
                return Cache[themeKey];
            }

            //TODO: add ayu theme https://github.com/ayu-theme/ayu-colors

            IHighlightingDefinition def;
            switch (key)
            {
                case "Default Theme":
                    def = ResolveDefinition($"c3IDE.Utilities.SyntaxHighlighting.Themes.{syntaxKey}Default.xshd");
                    break;
                case "Monokai Theme":
                    def = ResolveDefinition($"c3IDE.Utilities.SyntaxHighlighting.Themes.{syntaxKey}Monokai.xshd");
                    break;
                case "Ayu Light Theme":
                    def = ResolveDefinition($"c3IDE.Utilities.SyntaxHighlighting.Themes.{syntaxKey}AyuLight.xshd");
                    break;
                case "Ayu Mirage Theme":
                    def = ResolveDefinition($"c3IDE.Utilities.SyntaxHighlighting.Themes.{syntaxKey}AyuMirage.xshd");
                    break;
                default:
                    throw new InvalidOperationException("Invaild Syntax Highlighting Key");
            }

            Cache.Add(themeKey, def);
            return def;
        }

        private IHighlightingDefinition ResolveDefinition(string resource)
        {
            IHighlightingDefinition definition;
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resource))
            using (XmlTextReader xshd = new XmlTextReader(stream))
            {
                definition = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd, HighlightingManager.Instance);
            }

            return definition;
        }
    }
}

