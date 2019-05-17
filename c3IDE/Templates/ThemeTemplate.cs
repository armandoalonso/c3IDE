using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities.Helpers;

namespace c3IDE.Templates
{
    public class ThemeTemplate : ITemplate
    {
        public ThemeTemplate()
        {
            ResourceReader.Insatnce.LogResourceFiles();
            AddonJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.Theme.addon.txt");
            ThemeCode = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.Theme.theme_css.txt");
            ThemeLanguage = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.Theme.lang.txt");
        }

        public string AddonJson { get; }
        public string PluginEditTime => string.Empty;
        public string PluginRunTime => string.Empty;
        public string TypeEditTime => string.Empty;
        public string TypeRunTime => string.Empty;
        public string InstanceEditTime => string.Empty;
        public string InstanceRunTime => string.Empty;
        public string ActionAces => string.Empty;
        public string ActionLanguage => string.Empty;
        public string ActionCode => string.Empty;
        public string LanguageProperty => string.Empty;
        public string LanguageCategory => string.Empty;
        public string ExpressionLanguage => string.Empty;
        public string EffectCode => string.Empty;
        public string EffectLangauge => string.Empty;
        public string ThemeCode { get; }
        public string ThemeLanguage { get; }
    }
}
