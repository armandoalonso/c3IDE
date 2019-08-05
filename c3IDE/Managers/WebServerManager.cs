using System;
using System.Net;
using System.Net.Sockets;
using c3IDE.Compiler;
using c3IDE.Server;

namespace c3IDE.Managers
{
    public static class WebServerManager
    {
        public static bool WebServerStarted { get; set; } = false;
        public static Action<string> WebServiceUrlChanged { get; set; }
        public static Action<bool> WebServerStateChanged { get; set; }
        public static string WebServerUrl { get; set; }
        public static int WebServerPort { get; set; } 

        public static TcpListener TcpListener = new TcpListener(IPAddress.Any, 8080);

        public static void StartWebServer(int port = 8080)
        {
            try
            {
                WebServerPort = port;
                TcpListener = new TcpListener(IPAddress.Any, WebServerPort);
                AddonCompiler.Insatnce.WebServer = new WebServerClient();
                AddonCompiler.Insatnce.WebServer.Start(WebServerPort);

                LogManager.CompilerLog.Insert($"starting server => {WebServerUrl}");
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"failed to start web server => {ex.Message}");
            }
        }

        public static void StopWebServer()
        {
            AddonCompiler.Insatnce.WebServer.Stop();
        }

    }
}
