using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Windows;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Managers
{
    public static class ApplicationWindows
    {
        public static DashboardWindow DashboardWindow;
        public static AddonMetadataWindow MetadataWindow;
        public static AddonWindow AddonWindow;
        public static PluginWindow PluginWindow;
        public static TypeWindow TypeWindow;
        public static InstanceWindow InstanceWindow;
        public static ActionWindow ActionWindow;
        public static ConditionWindow ConditionWindow;
        public static ExpressionWindow ExpressionWindow;
        public static LanguageWindow LanguageWindow;
        public static TestWindow TestWidnow;
        public static OptionsWindow OptionsWindow;
        public static PopoutCompileWindow PopoutCompileWindow;
        public static SearchAndReplaceWindow FindAndReplaceWindow;
        public static EffectPropertiesWindow EffectPropertiesWindow;
        public static EffectParameterWindow EffectParameterWindow;
        public static EffectCodeWindow EffectCodeWindow;
        public static C2RuntimeWindow C2Runtime;

        static ApplicationWindows()
        {
            try
            {
                DashboardWindow = new DashboardWindow();
                MetadataWindow = new AddonMetadataWindow();
                AddonWindow = new AddonWindow();
                PluginWindow = new PluginWindow();
                TypeWindow = new TypeWindow();
                InstanceWindow = new InstanceWindow();
                ActionWindow = new ActionWindow();
                ConditionWindow = new ConditionWindow();
                ExpressionWindow = new ExpressionWindow();
                LanguageWindow = new LanguageWindow();
                TestWidnow = new TestWindow();
                OptionsWindow = new OptionsWindow();
                FindAndReplaceWindow = new SearchAndReplaceWindow();
                EffectPropertiesWindow = new EffectPropertiesWindow();
                EffectParameterWindow = new EffectParameterWindow();
                EffectCodeWindow = new EffectCodeWindow();
                C2Runtime = new C2RuntimeWindow();
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }

        }


    }
}
