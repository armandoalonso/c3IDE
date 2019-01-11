

# research for web server implementaion

https://github.com/unosquare/embedio
https://github.com/bonesoul/uhttpsharp
https://archive.codeplex.com/?p=webserver
https://github.com/pvginkel/NHttp
https://github.com/kayakhttp/kayak
https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener?redirectedfrom=MSDN&view=netframework-4.7.2


#embed io web server client implementation

[code]
using c3IDE.Compiler;
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

           
            var corsModule = new CorsModule("*", "Origin, X-Requested-With, Content-Type, Accept", "*");
            _webServer.RegisterModule(corsModule);



            //assign path to hist static files
            //var staticFileModule = new StaticFilesModule(AppData.Insatnce.Options.CompilePath);
            //staticFileModule.DefaultDocument = "addon.json";
            //staticFileModule.DefaultExtension = ".json";

            //add cors header to every file request header
            //staticFileModule.DefaultHeaders.Add(Headers.AccessControlAllowOrigin, "*");
            //staticFileModule.DefaultHeaders.Add(Headers.AccessControlAllowHeaders, "Origin, X-Requested-With, Content-Type, Accept");
            //_webServer.RegisterModule(staticFileModule);

            //the static files module will cache small files in ram until it detects they have been modified
            _webServer.Module<StaticFilesModule>().UseRamCache = true;
            _log.Insert($"static file path => {AppData.Insatnce.Options.CompilePath}", "WEB");


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
[/code]


# attemp to persist saved properties on generation

[code]  
 //language property, trying to preserve original text
 private void GeneratePropertyText(object sender, RoutedEventArgs e)
{
    var currentRegex =new Regex(@"\""(?<id>\w+[-]?\w+)\""\s*:\s*\{\s*\""name\""\s*:\s*\""(?<name>.+)\"",\s*\""desc\""\s*:\s*\""(?<desc>.+)\""");
    var currentMatches = currentRegex.Matches(PropertyLanguageTextEditor.Text);
    var propertyDictionary = new Dictionary<string, string>();

    //gather all current properties
    foreach (Match m in currentMatches)
    {
        var id = m.Groups["id"].ToString();
        var name = m.Groups["name"].ToString();
        var desc = m.Groups["desc"].ToString();

        var template = $@"    ""{id}"" : {{
			""name"": ""{name}"",
			""desc"": ""{desc}"",
		}}";
        propertyDictionary.Add(id, template);
    }

    //generate new property json
    var propertyRegex = new Regex(@"new SDK[.]PluginProperty\(\""(?<type>\w+)\""\W+(?<id>.*)\""");
    var propertyMatches = propertyRegex.Matches(AppData.Insatnce.CurrentAddon.PluginEditTime);

    var propList = new List<string>();
    foreach (Match m in propertyMatches)
    {
        var type = m.Groups["type"].ToString(); //todo: update property genberation based on type (combo, link etc...)
        var id = m.Groups["id"].ToString();

        if (propertyDictionary.ContainsKey(id))
        {
            //add existing property
            propList.Add(propertyDictionary[id]);
        }
        else
        {
            //create new property
            var template = $@"    ""{id}"" : {{
				""name"": ""property name"",
				""desc"": ""property desc"",
			}}";
            propList.Add(template);
        }
    }

    //set the editor to the new property json
    PropertyLanguageTextEditor.Text = $@"""properties"":{{
	{string.Join(",\n", propList)}
	}}";
}
[/code]