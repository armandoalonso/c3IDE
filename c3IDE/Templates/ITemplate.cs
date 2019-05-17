using System;

namespace c3IDE.Templates
{
    public interface ITemplate
    {
        string AddonJson { get; }
        string PluginEditTime { get; }
        string PluginRunTime { get; }
        string TypeEditTime { get; }
        string TypeRunTime { get; }
        string InstanceEditTime { get; }
        string InstanceRunTime { get; }
        string ActionAces { get; }
        string ActionLanguage { get; }
        string ActionCode { get; }
        string LanguageProperty { get; }
        string LanguageCategory { get; }
        string ExpressionLanguage { get; }

        string EffectCode { get; }
        string EffectLangauge { get; }

        string ThemeCode { get; }
        string ThemeLanguage { get; }
    }
}
