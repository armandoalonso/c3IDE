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
            var syntaxKey = syntax == Syntax.Javascript ? "JS" : "JSON";
            key = key + " " + syntaxKey;
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }

            //TODO: add ayu theme https://github.com/ayu-theme/ayu-colors

            IHighlightingDefinition def;
            switch (key)
            {
                case "Default Theme JS":
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JavascriptDefault.xshd");
                    break;
                case "Default Theme JSON":
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JsonDefault.xshd");
                    break;
                case "Monokai Theme JS":
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JavascriptMonokai.xshd");
                    break;
                case "Monokai Theme JSON":
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JsonMonokai.xshd");
                    break;
                case "Ayu Light Theme JS":
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JavascriptAyuLight.xshd");
                    break;
                case "Ayu Light Theme JSON":
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JsonAyuLight.xshd");
                    break;
                default:
                    throw new InvalidOperationException("Invaild Syntax Highlighting Key");
            }

            Cache.Add(key, def);
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

