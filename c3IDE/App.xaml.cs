using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using c3IDE.DataAccess;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Utilities.Helpers;
using Newtonsoft.Json;


namespace c3IDE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DataFolder { get; set; }
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
            var lintingPath = Path.Combine(defaultCompilePath, "lint");
            var defaultC3AddonPath = Path.Combine(dataFolder, "C3Addons");

            DataFolder = dataFolder;
            //create exports folder if it does not exists
            if (!System.IO.Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);
            if (!System.IO.Directory.Exists(defaultExportPath)) Directory.CreateDirectory(defaultExportPath);
            if (!System.IO.Directory.Exists(defaultCompilePath)) Directory.CreateDirectory(defaultCompilePath);
            if (!System.IO.Directory.Exists(defaultC3AddonPath)) Directory.CreateDirectory(defaultC3AddonPath);
            if (!System.IO.Directory.Exists(lintingPath)) Directory.CreateDirectory(lintingPath);

            //create default options
            OptionsManager.CurrentOptions = DataAccessFacade.Insatnce.OptionData.GetAll().FirstOrDefault() ?? OptionsManager.DefaultOptions;

            try
            {
                //check each property
                if (string.IsNullOrWhiteSpace(OptionsManager.CurrentOptions.DataPath)) OptionsManager.CurrentOptions.DataPath = OptionsManager.DefaultOptions.DataPath;
                if (string.IsNullOrWhiteSpace(OptionsManager.CurrentOptions.DataPath)) OptionsManager.CurrentOptions.DataPath = OptionsManager.DefaultOptions.DataPath;
                if (string.IsNullOrWhiteSpace(OptionsManager.CurrentOptions.CompilePath)) OptionsManager.CurrentOptions.CompilePath = OptionsManager.DefaultOptions.CompilePath;
                if (string.IsNullOrWhiteSpace(OptionsManager.CurrentOptions.ExportPath)) OptionsManager.CurrentOptions.ExportPath = OptionsManager.DefaultOptions.ExportPath;
                if (string.IsNullOrWhiteSpace(OptionsManager.CurrentOptions.C3AddonPath)) OptionsManager.CurrentOptions.C3AddonPath = OptionsManager.DefaultOptions.C3AddonPath;
                if (string.IsNullOrWhiteSpace(OptionsManager.CurrentOptions.DefaultAuthor)) OptionsManager.CurrentOptions.DefaultAuthor = OptionsManager.DefaultOptions.DefaultAuthor;
                if (string.IsNullOrWhiteSpace(OptionsManager.CurrentOptions.FontFamily)) OptionsManager.CurrentOptions.FontFamily = OptionsManager.DefaultOptions.FontFamily;
                if (OptionsManager.CurrentOptions.FontSize < 5) OptionsManager.CurrentOptions.FontSize = OptionsManager.DefaultOptions.FontSize;
                if (string.IsNullOrWhiteSpace(OptionsManager.CurrentOptions.ThemeKey)) OptionsManager.CurrentOptions.ThemeKey = OptionsManager.DefaultOptions.ThemeKey;
            }
            catch
            {
                OptionsManager.CurrentOptions = OptionsManager.DefaultOptions;
            }
        }

        private void OnWindowKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            {
                WindowManager.MainWindow.Save(true);
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
                    var info = new FileInfo(path);
                    C3Addon c3addon;

                    //check if file is json or project
                    var addonInfo = File.ReadAllLines(info.FullName)[0];
                    if (addonInfo == "@@METADATA")
                    {
                        c3addon = ProjectManager.ReadProject(info.FullName);
                    }
                    else
                    {
                        var data = File.ReadAllText(info.FullName);
                        c3addon = JsonConvert.DeserializeObject<C3Addon>(data);
                    }

                    var currAddon = DataAccessFacade.Insatnce.AddonData.Get(x => x.Id.Equals(c3addon.Id));
                    if (currAddon != null)
                    {
                        var results = MessageBox.Show(
                            "Addon currently exists do you want to overwrite addon? \n(YES) will overwrite, \n(NO) will assign new addon id.",
                            "Overwrite?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                        if (results == MessageBoxResult.Yes)
                        {
                            c3addon.LastModified = DateTime.Now;
                            DataAccessFacade.Insatnce.AddonData.Upsert(c3addon);
                        }
                        else if (results == MessageBoxResult.No)
                        {
                            c3addon.Id = Guid.NewGuid();
                            c3addon.LastModified = DateTime.Now;
                            DataAccessFacade.Insatnce.AddonData.Upsert(c3addon);
                        }
                        else
                        {
                            //do not open new addon
                            return;
                        }
                    }
                    else
                    {
                        c3addon.LastModified = DateTime.Now;
                        DataAccessFacade.Insatnce.AddonData.Upsert(c3addon);
                    }

                    //get the plugin template
                    c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
                    AddonManager.CurrentAddon = c3addon;
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
                           AddonManager.CurrentAddon = addon;
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
                LogManager.AddErrorLog(ex);
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

                foreach (var exception in LogManager.Exceptions)
                {
                    sb.AppendLine("\n===============================================================\n");
                    sb.AppendLine($"ERROR MESSAGE => {exception.Message}");
                    sb.AppendLine($"ERROR TRACE => {exception.StackTrace}");
                    sb.AppendLine($"ERROR INNER EX => {exception.InnerException}");
                    sb.AppendLine($"ERROR SOURCE => {exception.Source}");
                    sb.AppendLine("\n===============================================================\n");
                }

                var logFile = Path.Combine( OptionsManager.CurrentOptions.DataPath, $"app_log.txt");
                ProcessHelper.Insatnce.WriteFile(logFile, sb.ToString());
                ProcessHelper.Insatnce.StartProcess(logFile);
            }

            Environment.Exit(0);

        }
    }
}
