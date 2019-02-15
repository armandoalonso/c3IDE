using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using c3IDE.DataAccess;
using c3IDE.Models;

namespace c3IDE.Managers
{
    public static class OptionsManager
    {
        public static Options CurrentOptions { get; set; }
        private static readonly List<Action<Options>> _optionsChangedCallback = new List<Action<Options>>();

        /// <summary>
        /// subscribe to callback when options change
        /// </summary>
        /// <param name="loadedCallback"></param>
        /// <returns></returns>
        public static int AddOptionsChangeddCallback(Action<Options> loadedCallback)
        {
            var index = _optionsChangedCallback.Count;
            _optionsChangedCallback.Add(loadedCallback);
            return index;
        }

        /// <summary>
        /// saves the current options and calls the options changed callback
        /// </summary>
        public static void SaveOptions()
        {
            DataAccessFacade.Insatnce.OptionData.Upsert(CurrentOptions);
            foreach (var callback in _optionsChangedCallback)
            {
                callback?.Invoke(CurrentOptions);
            }
        }

        /// <summary>
        /// tries to load options from storage, if there are no options a default object is loaded
        /// </summary>
        public static void LoadOptions()
        {
            //todo: this should come from the application 
            var executingPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName;
            var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "C3IDE_DATA");
            var defaultExportPath = Path.Combine(dataFolder, "Exports");
            var defaultCompilePath = Path.Combine(dataFolder, "Server", "Test");
            var defaultC3AddonPath = Path.Combine(dataFolder, "C3Addons");

            var defaultOptions = new Options
            {
                DataPath = dataFolder,
                CompilePath = defaultCompilePath,
                ExportPath = defaultExportPath,
                C3AddonPath = defaultC3AddonPath,
                //DefaultCompany = "c3IDE",
                DefaultAuthor = "c3IDE",
                IncludeTimeStampOnExport = true,
                FontSize = 12,
                FontFamily = "Consolas",
                ThemeKey = "Default Theme",
                OpenC3InWeb = true,
                C3DesktopPath = string.Empty,
                PinMenu = false,
                CompileOnSave = false
            };

            AppData.Insatnce.Options = DataAccessFacade.Insatnce.OptionData.GetAll().FirstOrDefault() ?? defaultOptions;
        }
    }
}
