using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using c3IDE.Utilities.Search;
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
        public static MainWindow MainWindow { get; set; }
        public static Action<IEnumerable<SearchResult>, IWindow, string> OpenFindAndReplace { get; set; }
        public static Action<bool> ShowLoadingOverlay { get; set; }

        public static List<IWindow> WindowList = new List<IWindow>
        {
            ApplicationWindows.AddonWindow,
            ApplicationWindows.ActionWindow,
            ApplicationWindows.ConditionWindow,
            ApplicationWindows.ExpressionWindow,
            ApplicationWindows.PluginWindow,
            ApplicationWindows.TypeWindow,
            ApplicationWindows.InstanceWindow,
            ApplicationWindows.EffectPropertiesWindow,
            ApplicationWindows.EffectParameterWindow,
            ApplicationWindows.EffectCodeWindow,
            ApplicationWindows.CssWindow,
            ApplicationWindows.LanguageWindow
        };

        public static void SetWindowChangeCallback(Action<IWindow> window)
        {
            windowChangeCallback = window;
        }

        public static IWindow ChangeWindow(IWindow window)
        {
            PreviousWindow = CurrentWindow;
            CurrentWindow = window;

            PreviousWindow.OnExit();
            CurrentWindow.OnEnter();

            windowChangeCallback?.Invoke(CurrentWindow);
            return window;
        }

        public static void ClearAllWindows()
        {
            foreach (var window in WindowList)
            {
                window.Clear();
            }
        }
    }
}
