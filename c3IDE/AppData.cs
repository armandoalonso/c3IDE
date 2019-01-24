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
using c3IDE.Models;
using c3IDE.Utilities;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Utilities.ThemeEngine;
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

        public Func<string, string, Task<bool>> ShowDialog { get; internal set; }
        public Func<string, string, string, Task<string>> ShowInputDialog { get; set; }

        public Action<string> InfoMessage { get; set; }
        public Action<string> ErrorMessage { get; set; }

        public Action<string> LoadAddon { get; set; }
        public Action GlobalSave { get; set; }
        public Action<Theme> ThemeChangedEvent { get; set; }

        public void SetupTextEditor(TextEditor editor, Syntax syntax)
        { 
            editor.FontSize = Options.FontSize;
            editor.FontFamily = new FontFamily(Options.FontFamily);
            var syntaxDefinition = syntax == Syntax.Javascript
                ? Options.ApplicationTheme.JavascriptSyntaxTheme
                : Options.ApplicationTheme.JsonSyntaxTheme;
            editor.SyntaxHighlighting = syntaxDefinition;
            editor.Background = Options.ApplicationTheme.SyntaxBackgroundColor;
            editor.Foreground = Options.ApplicationTheme.SyntaxForegroundColor;
        }

        public void SetupTheme(Grid grid)
        {
           
        }
    }
}
