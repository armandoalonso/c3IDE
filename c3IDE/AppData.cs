using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;
using uhttpsharp.Listeners;

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
    }
}
