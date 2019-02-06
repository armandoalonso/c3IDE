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
        private HttpServer _httpServer;

        public void Start()
        {
            var _log = AppData.Insatnce.CompilerLog;

            _httpServer = new HttpServer(new HttpRequestProvider());

            C3FileHandler.Logger = _log;
            C3FileHandler.HttpRootDirectory = AppData.Insatnce.Options.CompilePath;

            _httpServer.Use(new TcpListenerAdapter(AppData.Insatnce.TcpListener));

            _httpServer.Use((context, next) =>
            {
                _log.Insert($"got request => {context.Request.Uri}", "C3");

                //TODO: add option to log out headers
                //foreach (var requestHeader in context.Request.Headers)
                //{
                //    _log.Insert($"got request headers => {requestHeader.Key} : {requestHeader.Value}");
                //}

                return next();
            });

            //handle static files (only suport js, json, png and svg)
            _httpServer.Use(new C3FileHandler());

            AppData.Insatnce.WebServerUrl = $"http://localhost:8080/{AppData.Insatnce.CurrentAddon.Class}/addon.json";
            AppData.Insatnce.WebServiceUrlChanged(AppData.Insatnce.WebServerUrl);
            _log.Insert($"starting server => {AppData.Insatnce.WebServerUrl}");
            _httpServer.Start();

        }

        public void Stop()
        {
            _httpServer.Dispose();
            _httpServer = null;
            AppData.Insatnce.TcpListener.Stop();
            AppData.Insatnce.CompilerLog.Insert("server stopped...");
            AppData.Insatnce.WebServerUrl = string.Empty;
            AppData.Insatnce.WebServiceUrlChanged(AppData.Insatnce.WebServerUrl);
        }
    }
}
