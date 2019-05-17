using c3IDE.Utilities.Helpers;
using System;

namespace c3IDE.Templates
{
    [Serializable]
    public class DrawingPluginTemplate : ITemplate
    {
        public DrawingPluginTemplate()
        {
            ResourceReader.Insatnce.LogResourceFiles();
            AddonJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.addon.txt");
            PluginEditTime = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.DrawingPlugin.plugin_edittime.txt");
            PluginRunTime = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.plugin_runtime.txt");
            TypeEditTime = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.type_edittime.txt");
            TypeRunTime = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.DrawingPlugin.type_runtime.txt");
            InstanceEditTime = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.DrawingPlugin.instance_edittime.txt");
            InstanceRunTime = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.DrawingPlugin.instance_runtime.txt");
            LanguageProperty = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.language_property.txt");
            LanguageCategory = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.language_category.txt");
            ActionAces = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.action_ace.txt");
            ActionLanguage = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.action_language.txt");
            ActionCode = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.action_code.txt");
            ExpressionLanguage = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.expression_language.txt");
        }

        public string AddonJson { get; }
        public string PluginEditTime { get; }
        public string PluginRunTime { get; }
        public string TypeEditTime { get; }
        public string TypeRunTime { get; }
        public string InstanceEditTime { get; }
        public string InstanceRunTime { get; }
        public string ActionAces { get; }
        public string ActionLanguage { get; }
        public string ActionCode { get; }
        public string LanguageProperty { get; }
        public string LanguageCategory { get; }
        public string ExpressionLanguage { get; }
        public string EffectCode => string.Empty;
        public string EffectLangauge => string.Empty;
        public string ThemeCode => string.Empty;
        public string ThemeLanguage => string.Empty;
    }
}
