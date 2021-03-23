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
        private static int attempt { get; set; } = 0;

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

                //reset attempts if server starts normally
                attempt = 0;
            }
            catch(SocketException ex)
            {
                //add threshold, to not run into infinite loop if for some reason can't cannot open to any port. 
                if(attempt > 10)
                {
                    throw new InvalidOperationException("tried to many times to connect to port, and failed");
                }
                //increment attempt when failed to connect
                attempt++;

                //port is already being used, increment port by attempt and try agian
                WebServerPort = port + attempt;
                TcpListener = new TcpListener(IPAddress.Any, WebServerPort);
                AddonCompiler.Insatnce.WebServer = new WebServerClient();
                AddonCompiler.Insatnce.WebServer.Start(WebServerPort);

                //reset attempts if server starts normally
                attempt = 0;
            }
            catch (Exception ex)
            {           
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"failed to start web server => {ex.Message}");
                throw;
            }
        }

        public static void StopWebServer()
        {
            AddonCompiler.Insatnce.WebServer.Stop();
        }

    }
}
