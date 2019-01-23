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

        public IHighlightingDefinition Resolve(string key)
        {
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }

            IHighlightingDefinition def;
            switch (key)
            {
                case "Default Theme":
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JavaScriptDefault.xshd");
                    break;
                case "Test Theme":
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JavaScriptTest.xshd");
                    break;
                default:
                    def = ResolveDefinition("c3IDE.Utilities.SyntaxHighlighting.Themes.JavaScriptDefault.xshd");
                    break;
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

