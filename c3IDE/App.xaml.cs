using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using c3IDE.DataAccess;
using c3IDE.Models;

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

            DataFolder = dataFolder;

            //setup default options
            DefaultOptions = new Options
            {
                DataPath = dataFolder,
                CompilePath = defaultCompilePath,
                ExportPath = defaultExportPath
            };

            //create default options
            AppData.Insatnce.Options = DataAccessFacade.Insatnce.OptionData.GetAll().FirstOrDefault() ?? DefaultOptions;

            //create exports folder if it does not exists
            if (!System.IO.Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);
            if (!System.IO.Directory.Exists(defaultExportPath)) Directory.CreateDirectory(defaultExportPath);
            if (!System.IO.Directory.Exists(defaultCompilePath)) Directory.CreateDirectory(defaultCompilePath);
        }
    }
}
