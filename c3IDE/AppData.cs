using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using c3IDE.Models;
using c3IDE.Utilities;
using c3IDE.Utilities.SyntaxHighlighting;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using uhttpsharp.Listeners;
using Action = System.Action;


namespace c3IDE
{
    public class AppData : Singleton<AppData>
    {
        public List<C3Addon> AddonList = new List<C3Addon>();
        public C3Addon CurrentAddon = new C3Addon();
        public Options Options = new Options();
        public TcpListener TcpListener = new TcpListener(IPAddress.Any, 8080);
        public MainWindow MainWidnow { get; set; }

        public Func<string, string, Task<bool>> ShowDialog { get; internal set; }
        public Func<string, string, string, Task<string>> ShowInputDialog { get; set; }

        public Action<string> InfoMessage { get; set; }
        public Action<string> ErrorMessage { get; set; }

        public Action<string> LoadAddon { get; set; }
        public Action GlobalSave { get; set; }

        public void SetupTextEditor(TextEditor editor)
        { 
            editor.FontSize = Options.FontSize;
            editor.FontFamily = new FontFamily(Options.FontFamily);
            editor.SyntaxHighlighting = Options.HighlightingDefinition;
            editor.Background = ThemeResolver.Insatnce.ResolveBackground(Options.HighlightKey);

            if (MainWidnow != null)
            {
                MainWidnow.MainGrid.Background = ThemeResolver.Insatnce.ResolveFormBackground(Options.HighlightKey);
            }
        }
    }
}
