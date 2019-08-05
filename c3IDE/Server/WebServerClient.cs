using System;
using c3IDE.Managers;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;


namespace c3IDE.Server
{
    public class WebServerClient
    {
        private HttpServer _httpServer;

        //todo: add option to log out headers
        /// <summary>
        /// starts the web server
        /// </summary>
        public void Start(int port)
        {
            _httpServer = new HttpServer(new HttpRequestProvider());

            C3FileHandler.HttpRootDirectory = OptionsManager.CurrentOptions.CompilePath;

            _httpServer.Use(new TcpListenerAdapter(WebServerManager.TcpListener));

            _httpServer.Use((context, next) =>
            {
                LogManager.CompilerLog.Insert($"got request => {context.Request.Uri}", "C3");

                //foreach (var requestHeader in context.Request.Headers)
                //{
                //    LogManager.CompilerLog.Insert($"got request headers => {requestHeader.Key} : {requestHeader.Value}");
                //}

                return next();
            });

            //handle static files (only suport js, json, png and svg)
            _httpServer.Use(new C3FileHandler());

            WebServerManager.WebServerUrl = $"http://localhost:{port}/{AddonManager.CurrentAddon.Class}/addon.json";
            WebServerManager.WebServiceUrlChanged?.Invoke(WebServerManager.WebServerUrl);
            LogManager.CompilerLog.Insert($"starting server => {WebServerManager.WebServerUrl}");
            _httpServer.Start();
            WebServerManager.WebServerStarted = true;
            WebServerManager.WebServerStateChanged?.Invoke(true);
        }

        /// <summary>
        /// stops the web server
        /// </summary>
        public void Stop()
        {
            _httpServer.Dispose();
            _httpServer = null;
            WebServerManager.TcpListener.Stop();
            WebServerManager.WebServerUrl = string.Empty;
            WebServerManager.WebServiceUrlChanged?.Invoke(WebServerManager.WebServerUrl);
            WebServerManager.WebServerStarted = false;
            WebServerManager.WebServerStateChanged?.Invoke(false);

            try
            {
                LogManager.CompilerLog.Insert("server stopped...");
            }
            catch(Exception ex)
            {
                LogManager.AddErrorLog(ex);
            }
        }
    }
}
