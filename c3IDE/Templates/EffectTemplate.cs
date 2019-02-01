﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities.Helpers;

namespace c3IDE.Templates
{
    public class EffectTemplate : ITemplate
    {
        public EffectTemplate()
        {
            ResourceReader.Insatnce.LogResourceFiles();
            AddonJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.Effect.addon.txt");
            EffectCode = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.Effect.effect.fx");
            EffectLangauge = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.Effect.lang.txt");
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
        public string ExpressionLanguage => string.Empty;
        public string EffectCode { get; }
        public string EffectLangauge { get; }
    }
}
