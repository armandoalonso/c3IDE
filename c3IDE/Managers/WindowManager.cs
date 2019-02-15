using System;
using System.Threading.Tasks;
using c3IDE.Windows;
using c3IDE.Windows.Interfaces;


namespace c3IDE.Managers
{
    public static class WindowManager
    {
        public static IWindow CurrentWindow { get; set; }
        public static IWindow PreviousWindow { get; set; }
        public static Func<string, string, Task<bool>> ShowDialog { get; set; }
        public static Func<string, string, string, Task<string>> ShowInputDialog { get; set; }
        private static Action<IWindow> windowChangeCallback { get; set; }

        public static void SetWindowChangeCallback(Action<IWindow> window)
        {
            windowChangeCallback = window;
        }

        public static void ChangeWindow(IWindow window)
        {
            PreviousWindow = CurrentWindow;
            CurrentWindow = window;

            PreviousWindow.OnExit();
            CurrentWindow.OnEnter();

            windowChangeCallback?.Invoke(CurrentWindow);
        }
    }

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
        public static EffectAddonWindow FxAddonWindow = new EffectAddonWindow();
        public static EffectCodeWindow FxCodeWindow = new EffectCodeWindow();
        public static EffectLanguageWindow FxLanguageWindow = new EffectLanguageWindow();
        public static PopoutCompileWindow PopoutCompileWindow;
        public static SearchAndReplaceWindow FindAndReplaceWindow = new SearchAndReplaceWindow();

    }
}
