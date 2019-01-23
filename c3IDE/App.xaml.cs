using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.SyntaxHighlighting;

namespace c3IDE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DataFolder { get; set; }
        public static Options DefaultOptions { get; set; }

        public App()
        {
            //load saved options if none are saved
            var executingPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName;
            var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "C3IDE_DATA");
            var defaultExportPath = Path.Combine(dataFolder, "Exports");
            var defaultCompilePath = Path.Combine(dataFolder, "Server", "Test");
            var defaultC3AddonPath = Path.Combine(dataFolder, "C3Addons");

            DataFolder = dataFolder;

            //setup default options
            DefaultOptions = new Options
            {
                DataPath = dataFolder,
                CompilePath = defaultCompilePath,
                ExportPath = defaultExportPath,
                C3AddonPath = defaultC3AddonPath,
                DefaultCompany = "c3IDE",
                DefaultAuthor = "c3IDE",
                FontSize = 12,
                FontFamily = "Consolas",
                HighlightKey = "Default Theme"
            };

            //create exports folder if it does not exists
            if (!System.IO.Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);
            if (!System.IO.Directory.Exists(defaultExportPath)) Directory.CreateDirectory(defaultExportPath);
            if (!System.IO.Directory.Exists(defaultCompilePath)) Directory.CreateDirectory(defaultCompilePath);
            if (!System.IO.Directory.Exists(defaultC3AddonPath)) Directory.CreateDirectory(defaultC3AddonPath);

            //create default options
            AppData.Insatnce.Options = DataAccessFacade.Insatnce.OptionData.GetAll().FirstOrDefault() ?? DefaultOptions;

            //check each property
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.DataPath)) AppData.Insatnce.Options.DataPath = DefaultOptions.DataPath;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.CompilePath)) AppData.Insatnce.Options.CompilePath = DefaultOptions.CompilePath;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.ExportPath)) AppData.Insatnce.Options.ExportPath = DefaultOptions.ExportPath;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.C3AddonPath)) AppData.Insatnce.Options.C3AddonPath = DefaultOptions.C3AddonPath;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.DefaultCompany)) AppData.Insatnce.Options.DefaultCompany = DefaultOptions.DefaultCompany;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.DefaultAuthor)) AppData.Insatnce.Options.DefaultAuthor = DefaultOptions.DefaultAuthor;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.FontFamily)) AppData.Insatnce.Options.FontFamily = DefaultOptions.FontFamily;
            if (AppData.Insatnce.Options.FontSize < 5) AppData.Insatnce.Options.FontSize = DefaultOptions.FontSize;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.HighlightKey)) AppData.Insatnce.Options.HighlightKey = DefaultOptions.HighlightKey;

            //create exmaple projects if they don't exists
            var examples = new string[] {"Example_Log.c3ide", "Example_FSM.c3ide" };
            foreach (var example in examples)
            {
                var path = Path.Combine(AppData.Insatnce.Options.ExportPath, example);
                if (!System.IO.File.Exists(path))
                {
                    var data = ResourceReader.Insatnce.GetResourceText($"c3IDE.Examples.{example}");
                    Utils.Insatnce.WriteFile(path, data);
                }
            }

        }
    }
}
