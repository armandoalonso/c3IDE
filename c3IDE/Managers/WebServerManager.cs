using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Managers
{
    public static class WebServerManager
    {
        public static bool WebServerStarted { get; set; } = false;
        public static Action<string> WebServiceUrlChanged { get; set; }
        public static Action<bool> WebServerStateChanged { get; set; }

    }
}
