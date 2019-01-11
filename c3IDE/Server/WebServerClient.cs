using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using c3IDE.Compiler;
using uhttpsharp;
using uhttpsharp.Headers;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;


namespace c3IDE.Server
{
    public class WebServerClient
    {
        private CompilerLog _log;
        private HttpServer _httpServer;

        public void Start(CompilerLog log)
        {
            this._log = log;

            _httpServer = new HttpServer(new HttpRequestProvider());

            C3FileHandler.Logger = _log;
            C3FileHandler.HttpRootDirectory = AppData.Insatnce.Options.CompilePath;

            _httpServer.Use(new TcpListenerAdapter(AppData.Insatnce.TcpListener));

            _httpServer.Use((context, next) =>
            {
                _log.Insert($"got request => {context.Request.Uri}");

                foreach (var requestHeader in context.Request.Headers)
                {
                    _log.Insert($"got request headers => {requestHeader.Key} : {requestHeader.Value}");
                }

                return next();
            });

            //handle static files (only suport js, json and png)
            _httpServer.Use(new C3FileHandler());

            _log.Insert("starting server => https://localhost:8080/addon.json");
            _httpServer.Start();

        }

        public void Stop()
        {
            _log.Insert("server stopped...");
            _httpServer.Dispose();
            _httpServer = null;
            AppData.Insatnce.TcpListener.Stop();
        }
    }
}
