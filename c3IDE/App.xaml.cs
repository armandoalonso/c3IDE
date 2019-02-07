using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Logging;
using c3IDE.Utilities.SyntaxHighlighting;
using Microsoft.Win32;
using Newtonsoft.Json;
using Action = System.Action;

namespace c3IDE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DataFolder { get; set; }
        public static Options DefaultOptions { get; set; }
        private bool ApplicationError = false;

        public App()
        {
            //register global key down
            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyUpEvent, new KeyEventHandler(OnWindowKeyUp));

            //global unhandled exception catch
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;

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
            //if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.DefaultCompany)) AppData.Insatnce.Options.DefaultCompany = DefaultOptions.DefaultCompany;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.DefaultAuthor)) AppData.Insatnce.Options.DefaultAuthor = DefaultOptions.DefaultAuthor;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.FontFamily)) AppData.Insatnce.Options.FontFamily = DefaultOptions.FontFamily;
            if (AppData.Insatnce.Options.FontSize < 5) AppData.Insatnce.Options.FontSize = DefaultOptions.FontSize;
            if (string.IsNullOrWhiteSpace(AppData.Insatnce.Options.ThemeKey)) AppData.Insatnce.Options.ThemeKey = DefaultOptions.ThemeKey;
        }

        private void OnWindowKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            {
                AppData.Insatnce.GlobalSave();

                if (AppData.Insatnce.Options.CompileOnSave)
                {
                    //compile on save
                }
            }
        }

        public void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                //check for oneclick args
                if (AppDomain.CurrentDomain?.SetupInformation?.ActivationArguments?.ActivationData != null)
                {
                    
                    var path = new Uri(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0]).LocalPath;
                    if(string.IsNullOrWhiteSpace(path)) return;
                    var data = File.ReadAllText(path);
                    var c3addon = JsonConvert.DeserializeObject<C3Addon>(data);

                    //todo: this will overwrite you addon
                    DataAccessFacade.Insatnce.AddonData.Upsert(c3addon);

                    //get the plugin template
                    c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);

                    AppData.Insatnce.CurrentAddon = c3addon;
                }

                //process command line args
                var args = e.Args;
                if (args.Any())
                {
                    //checked if string is guid
                    if (Guid.TryParse(args[0], out var guid))
                    {
                        var addon = DataAccessFacade.Insatnce.AddonData.Get(x => x.Id == guid).FirstOrDefault();
                        if (addon != null)
                        {
                            AppData.Insatnce.CurrentAddon = addon;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"invalid arg => {args[0]}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
            }

            //always start main window
            //MainWindow main = new MainWindow();
            //main.Show();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (!ApplicationError)
            {
                //create log
                var sb = new StringBuilder();

                sb.AppendLine("There was an unhandled error in the application. see logs below => ");
                sb.AppendLine($"ERROR MESSAGE => {e.Exception.Message}");
                sb.AppendLine($"ERROR TRACE => {e.Exception.StackTrace}");
                sb.AppendLine($"ERROR INNER EX => {e.Exception.InnerException}");
                sb.AppendLine($"ERROR SOURCE => {e.Exception.Source}");

                sb.AppendLine();
                sb.AppendLine("EXCEPTION LIST =>");

                foreach (var exception in LogManager.Insatnce.Exceptions)
                {
                    sb.AppendLine("\n===============================================================\n");
                    sb.AppendLine($"ERROR MESSAGE => {exception.Message}");
                    sb.AppendLine($"ERROR TRACE => {exception.StackTrace}");
                    sb.AppendLine($"ERROR INNER EX => {exception.InnerException}");
                    sb.AppendLine($"ERROR SOURCE => {exception.Source}");
                    sb.AppendLine("\n===============================================================\n");
                }

                var logFile = Path.Combine(AppData.Insatnce.Options.DataPath, $"app_log.txt");
                ProcessHelper.Insatnce.WriteFile(logFile, sb.ToString());
                ProcessHelper.Insatnce.StartProcess(logFile);
            }

            Environment.Exit(0);

        }
    }
}
