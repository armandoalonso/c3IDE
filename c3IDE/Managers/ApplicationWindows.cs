using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Windows;

namespace c3IDE.Managers
{
    public static class ApplicationWindows
    {
        public static DashboardWindow DashboardWindow = new DashboardWindow();
        public static AddonMetadataWindow MetadataWindow = new AddonMetadataWindow();
        public static AddonWindow AddonWindow = new AddonWindow();
        public static PluginWindow PluginWindow = new PluginWindow();
        public static TypeWindow TypeWindow = new TypeWindow();
        public static InstanceWindow InstanceWindow = new InstanceWindow();
        public static ActionWindow ActionWindow = new ActionWindow();
        public static ConditionWindow ConditionWindow = new ConditionWindow();
        public static ExpressionWindow ExpressionWindow = new ExpressionWindow();
        public static LanguageWindow LanguageWindow = new LanguageWindow();
        public static TestWindow TestWidnow = new TestWindow();
        public static OptionsWindow OptionsWindow = new OptionsWindow();
        public static PopoutCompileWindow PopoutCompileWindow;
        public static SearchAndReplaceWindow FindAndReplaceWindow = new SearchAndReplaceWindow();
        public static EffectPropertiesWindow EffectPropertiesWindow = new EffectPropertiesWindow();
        public static EffectParameterWindow EffectParameterWindow = new EffectParameterWindow();
        public static EffectCodeWindow EffectCodeWindow = new EffectCodeWindow();
    }
}
