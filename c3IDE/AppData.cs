using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using c3IDE.Compiler;
using c3IDE.Models;
using c3IDE.Utilities;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using MahApps.Metro;
using MahApps.Metro.Converters;
using uhttpsharp.Listeners;
using Action = System.Action;
using Theme = c3IDE.Utilities.ThemeEngine.Theme;


namespace c3IDE
{
    //public class AppData : Singleton<AppData>
    //{
    //    //public List<C3Addon> AddonList = new List<C3Addon>();
    //    //public C3Addon CurrentAddon = new C3Addon();

    //    //public CompilerLog CompilerLog { get; set; } = new CompilerLog();
    //    //public MainWindow MainWindow { get; set; }

    //    //public Func<string, string, Task<bool>> ShowDialog { get; internal set; }
    //    //public Func<string, string, string, Task<string>> ShowInputDialog { get; set; }

    //    //public Action<string> LoadAddon { get; set; }
    //    //public Action<Options> OptionChanged { get; set; }
    //    //public Action<bool> GlobalSave { get; set; }
    //    //public string WebServerUrl { get; set; }
    //    //public Action<string> WebServiceUrlChanged { get; set; }
    //    //public bool WebServerStarted { get; set; }

    //    //public Action<bool> WebServerStateChanged { get; set; }
    //    //public Action UpdateTestWindow { get; set; }
    //    //public Action<IEnumerable<SearchResult>, IWindow> OpenFindAndReplace { get; set; }
    //    //public Action<IWindow> NavigateToWindow { get; set; }
    //}
}
