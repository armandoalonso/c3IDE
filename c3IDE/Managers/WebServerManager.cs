using System;
using System.Net;
using System.Net.Sockets;

namespace c3IDE.Managers
{
    public static class WebServerManager
    {
        public static bool WebServerStarted { get; set; } = false;
        public static Action<string> WebServiceUrlChanged { get; set; }
        public static Action<bool> WebServerStateChanged { get; set; }
        public static string WebServerUrl { get; set; }

        public static TcpListener TcpListener = new TcpListener(IPAddress.Any, 8080);

    }
}
