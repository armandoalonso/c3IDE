using System;
using c3IDE.Models;
using c3IDE.Utilities;

namespace c3IDE.Templates
{
    public class TemplateFactory : Singleton<TemplateFactory>
    {
        public ITemplate CreateTemplate(PluginType type)
        {
            switch (type)
            {
                case PluginType.SingleGlobalPlugin:
                    return new SingleGlobalPluginTemplate();
                //case PluginType.MultiInstance:
                //    return new MultiInstancePluginTemplate();
                case PluginType.DrawingPlugin:
                    return new DrawingPluginTemplate();
                case PluginType.Behavior:
                    return new BehaviorTemplate();
                case PluginType.Effect:
                    return new EffectTemplate();
                case PluginType.Theme:
                    return new ThemeTemplate();
            }

            throw new InvalidOperationException("Not Valid Plugin Type");
        }

    }
}
