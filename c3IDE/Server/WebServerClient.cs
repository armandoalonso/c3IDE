using c3IDE.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Swan;

namespace c3IDE.Server
{
    public class WebServerClient
    {
        public string Url { get; set; } = "http://localhost:8080";
        private WebServer _webServer;
        private CompilerLog _log;

        public void Start(CompilerLog log)
        {
            //setup logging
            this._log = log;

            //setup webserver logging
            Terminal.OnLogMessageReceived += Terminal_OnLogMessageReceived;

            //create instance of web server @ url
            _webServer = new WebServer(Url, RoutingStrategy.Regex);
            _log.Insert($"starting web server on => {Url}", "WEB");

           
            //var corsModule = new CorsModule("*", "Origin, X-Requested-With, Content-Type, Accept", "*");
            //_webServer.RegisterModule(corsModule);
            _webServer.EnableCors("*", "Origin, X-Requested-With, Content-Type, Accept", "*");

            _log.Insert($"static file path => {AppData.Insatnce.Options.CompilePath}", "WEB");

            //assign path to hist static files
            var staticFileModule = new StaticFilesModule(AppData.Insatnce.Options.CompilePath);
            staticFileModule.DefaultExtension = ".json";
            //add cors header to every file request header
            staticFileModule.DefaultHeaders.Add(Headers.AccessControlAllowOrigin, "*");
            staticFileModule.DefaultHeaders.Add(Headers.AccessControlAllowHeaders, "Origin, X-Requested-With, Content-Type, Accept");
            _webServer.RegisterModule(staticFileModule);
            //the static files module will cache small files in ram until it detects they have been modified
            _webServer.Module<StaticFilesModule>().UseRamCache = true;

            _log.Insert($"c3addon dev url => {Url}/addon.json", "WEB");
            //start web server in background
            var task = _webServer.RunAsync();

        }

        private void Terminal_OnLogMessageReceived(object sender, LogMessageReceivedEventArgs e)
        {
            _log.Insert(e.Message, "WEB");
        }

        public void Stop()
        {
            _log.Insert("terminating web server...", "WEB");
            _webServer.Dispose();
        }
    }
}
