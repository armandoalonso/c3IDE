using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Server;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using Yahoo.Yui.Compressor;

namespace c3IDE.Compiler
{
    public class AddonCompiler : Singleton<AddonCompiler>
    {
        private Dictionary<string, string> _addonFiles;
        public WebServerClient WebServer { get; set; }
        public bool IsCompilationValid { get; set; }

        /// <summary>
        /// compiles an addon and starts the web server
        /// </summary>
        /// <param name="addon"></param>
        /// <param name="startWebServer"></param>
        /// <returns></returns>
        public async Task<bool> CompileAddon(C3Addon addon, bool startWebServer = true)
        {
            
            if (!ValidateFiles(addon))
            {
                IsCompilationValid = false;
                return false;
            }
            IsCompilationValid = true;

            try
            {
                LogManager.CompilerLog.Insert($"compilation starting...");

                //generate unique folder for specific addon class
                var folderName = addon.Class.ToLower();
                addon.AddonFolder = Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName);

                //check for addon id
                if (string.IsNullOrWhiteSpace(addon.AddonId))
                {
                    addon.AddonId = $"{addon.Author}_{addon.Class}";
                }

                //clear out compile path
                if (Directory.Exists(addon.AddonFolder))
                {
                    LogManager.CompilerLog.Insert($"compile directory exists => { addon.AddonFolder}");
                    System.IO.Directory.Delete(addon.AddonFolder, true);
                    LogManager.CompilerLog.Insert($"removed compile directory...");
                }

                //create main compile directory
                LogManager.CompilerLog.Insert($"recreating compile directory => { addon.AddonFolder}");
                if (!Directory.Exists(OptionsManager.CurrentOptions.CompilePath))
                {
                    System.IO.Directory.CreateDirectory(OptionsManager.CurrentOptions.CompilePath);
                }

                //create addon compile directory and addon specific paths
                System.IO.Directory.CreateDirectory(addon.AddonFolder);
                System.IO.Directory.CreateDirectory(Path.Combine(addon.AddonFolder, "lang"));
                if (addon.Type != PluginType.Effect && addon.Type != PluginType.Theme)
                {
                    System.IO.Directory.CreateDirectory(Path.Combine(addon.AddonFolder, "c3runtime"));
                }
                if (!string.IsNullOrWhiteSpace(addon.C2RunTime) ||(addon.ThirdPartyFiles != null && addon.ThirdPartyFiles.Any(x => x.Value.C2Folder)))
                {
                    System.IO.Directory.CreateDirectory(Path.Combine(addon.AddonFolder, "c2runtime"));
                }
                LogManager.CompilerLog.Insert($"compile directory created successfully => { addon.AddonFolder}");


                if (addon.Type == PluginType.Effect)
                {
                    //todo: effect validator http://shdr.bkcore.com/ 
                    CreateEffectFiles(addon, folderName);
                }
                else if (addon.Type == PluginType.Theme)
                {
                    CreateThemeFiles(addon, folderName);
                }
                else
                {
                    CreateAddonFiles(addon, folderName);
                }
            }
            catch (Exception ex)
            {
                IsCompilationValid = false;
                LogManager.AddErrorLog(ex);
                LogManager.CompilerLog.Insert($"compilation terminated due to error...");
                LogManager.CompilerLog.Insert($"error => {ex.Message}");
                NotificationManager.PublishErrorNotification("There was an error generating the addon, please check the log.");
                return false;
            }

            //try and start the web server
            try
            {
                if (startWebServer && IsCompilationValid)
                {
                    //start web server installation
                    await Task.Run(() =>
                    {
                        WebServer = new WebServerClient();
                        WebServer.Start();
                    });
                }           
            }
            catch (Exception ex)
            {
                IsCompilationValid = false;
                LogManager.AddErrorLog(ex);
                LogManager.CompilerLog.Insert($"web server failed to start...");
                NotificationManager.PublishErrorNotification("The web server failed to start... check that the port 8080, is not being used by another application.");
                WebServerManager.WebServerStarted = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// generates all the addon files
        /// </summary>
        /// <param name="addon"></param>
        /// <param name="folderName"></param>
        private void CreateAddonFiles(C3Addon addon, string folderName)
        {
            //generate file strings
            _addonFiles = new Dictionary<string, string>();


            LogManager.CompilerLog.Insert($"generating addon.json");
            //generate addon json files
            _addonFiles.Add(Path.Combine(
                OptionsManager.CurrentOptions.CompilePath, folderName, "addon.json"), 
                LogManager.CompilerLog.WrapLogger(() => FormatHelper.Insatnce.Json(addon.AddonJson)));
            LogManager.CompilerLog.Insert($"generating addon.json => complete");

            if (addon.Type == PluginType.Behavior)
            {
                LogManager.CompilerLog.Insert($"generating behavior.js (edittime)");
                _addonFiles.Add(
                    Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "behavior.js"), 
                    ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.PluginEditTime)));
                LogManager.CompilerLog.Insert($"generating behavior.js (edittime) => complete");

                LogManager.CompilerLog.Insert($"generating behavior.js (runtime)");
                _addonFiles.Add(
                    Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", "behavior.js"),
                    ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.PluginRunTime)));
                LogManager.CompilerLog.Insert($"generating behavior.js (runtime) => complete");
            }
            else
            {
                LogManager.CompilerLog.Insert($"generating plugin.js (edittime)");
                _addonFiles.Add(
                    Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "plugin.js"),
                    ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.PluginEditTime)));
                LogManager.CompilerLog.Insert($"generating plugin.js (edittime) => complete");

                LogManager.CompilerLog.Insert($"generating plugin.js (runtime)");
                _addonFiles.Add(
                    Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", "plugin.js"),
                    ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.PluginRunTime)));
                LogManager.CompilerLog.Insert($"generating plugin.js (runtime) => complete");
            }

            LogManager.CompilerLog.Insert($"generating type.js (edittime)");
            _addonFiles.Add(
                Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "type.js"),
                ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.TypeEditTime)));
            LogManager.CompilerLog.Insert($"generating type.js (edittime) => complete");

            LogManager.CompilerLog.Insert($"generating instance.js (edittime)");
            _addonFiles.Add(
                Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "instance.js"),
                ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.InstanceEditTime)));
            LogManager.CompilerLog.Insert($"generating instance.js (edittime) => complete");

            LogManager.CompilerLog.Insert($"generating ace.json");
            _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "aces.json"), LogManager.CompilerLog.WrapLogger(() => FormatHelper.Insatnce.Json(CompileAce(addon))));
            LogManager.CompilerLog.Insert($"generating ace.json => complete");

            LogManager.CompilerLog.Insert($"generating en-US.json");
            _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "lang", "en-US.json"), LogManager.CompilerLog.WrapLogger(() => FormatHelper.Insatnce.Json(CompileLang(addon))));
            LogManager.CompilerLog.Insert($"generating en-US.json => complete");

            LogManager.CompilerLog.Insert($"generating type.js (runtime)");
            _addonFiles.Add(Path.Combine(
                OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", "type.js"),
                ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.TypeRunTime)));
            LogManager.CompilerLog.Insert($"generating type.js (runtime) => complete");

            LogManager.CompilerLog.Insert($"generating instance.js (runtime)");
            _addonFiles.Add(
                Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", "instance.js"),
                ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.InstanceRunTime)));
            LogManager.CompilerLog.Insert($"generating instance.js (runtime) => complete");

            LogManager.CompilerLog.Insert($"generating action.js");
            _addonFiles.Add(
                Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", "actions.js"),
                ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(CompileActions(addon))));
            LogManager.CompilerLog.Insert($"generating action.js => complete");

            LogManager.CompilerLog.Insert($"generating conditions.js");
            _addonFiles.Add(
                Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", "conditions.js"),
                ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(CompileConditions(addon))));
            LogManager.CompilerLog.Insert($"generating conditions.js => complete");

            LogManager.CompilerLog.Insert($"generating expressions.js");
            _addonFiles.Add(Path.Combine(
                OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", "expressions.js"),
                ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(CompileExpressions(addon))));
            LogManager.CompilerLog.Insert($"generating expressions.js => complete");

            LogManager.CompilerLog.Insert("generating 3rd party files");
            foreach (var files in addon.ThirdPartyFiles.Values)
            {
                switch (files.Extention)
                {
                    case ".js":
                        //todo: add an option to compress js strings for third party files
                        string content = string.Empty;
                        if (files.Compress)
                        {
                            content = FormatHelper.Insatnce.CompressMinifiedFiles(files.Content);
                        }
                        else
                        {
                            content = files.Content;    
                        }
                       
                        if (files.Rootfolder) _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, files.FileName), content);
                        if (files.C3Folder) _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", files.FileName), content);
                        if (files.C2Folder) _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c2runtime", files.FileName), content);
                        break;
                    case ".css":
                    case ".html":
                    case ".json":
                    case ".xml":
                    case ".txt":
                    case null:
                        if(files.Rootfolder) _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, files.FileName), files.Content);
                        if (files.C3Folder) _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", files.FileName), files.Content);
                        if (files.C2Folder) _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c2runtime", files.FileName), files.Content);
                        break;
                    default:
                        if (files.Rootfolder)  File.WriteAllBytes(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, files.FileName), files.Bytes);
                        if (files.C3Folder) File.WriteAllBytes(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c3runtime", files.FileName), files.Bytes);
                        if (files.C2Folder) File.WriteAllBytes(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c2runtime", files.FileName), files.Bytes);
                        break;
                }

                LogManager.CompilerLog.Insert($"generating {files.FileName}");
            }
            LogManager.CompilerLog.Insert("generating 3rd party files => complete");

            if (!string.IsNullOrWhiteSpace(addon.C2RunTime))
            {
                LogManager.CompilerLog.Insert("generating c2runtime file");
                _addonFiles.Add(
                    Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "c2runtime", "runtime.js"),
                    ConsoleLogRemover.Insatnce.CommentOut(FormatHelper.Insatnce.Javascript(addon.C2RunTime)));
                LogManager.CompilerLog.Insert("generating c2runtime file => complete");
            }

            //write files to path
            foreach (var file in _addonFiles)
            {
                var fi = new FileInfo(file.Key);
                if(fi.Directory != null && !fi.Directory.Exists) fi.Directory.Create(); 
                System.IO.File.WriteAllText(file.Key, file.Value);
                LogManager.CompilerLog.Insert($"writing file => {file.Key}");
            }

            File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "icon.svg"), addon.IconXml);
            LogManager.CompilerLog.Insert($"writing file => icon.svg");
            LogManager.CompilerLog.Insert($"compilation complete...");
        }

        /// <summary>
        /// generate effect files
        /// </summary>
        /// <param name="addon"></param>
        /// <param name="folderName"></param>
        private void CreateEffectFiles(C3Addon addon, string folderName)
        {
            //generate file strings
            _addonFiles = new Dictionary<string, string>();

            //generate simple files
            LogManager.CompilerLog.Insert($"generating addon.json");
            _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "addon.json"), CompileEffectAddon(addon));
            LogManager.CompilerLog.Insert($"generating addon.json => complete");

            LogManager.CompilerLog.Insert($"generating effect.fx ");
            _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "effect.fx"), addon.Effect.Code);
            LogManager.CompilerLog.Insert($"generating effect.fx  => complete");

            LogManager.CompilerLog.Insert($"generating en-US.json");
            _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "lang", "en-US.json"), CompileEffectLang(addon));
            LogManager.CompilerLog.Insert($"generating en-US.json => complete");

            //write files to path
            foreach (var file in _addonFiles)
            {
                System.IO.File.WriteAllText(file.Key, file.Value);
                LogManager.CompilerLog.Insert($"writing file => {file.Key}");
            }

            LogManager.CompilerLog.Insert($"compilation complete...");
        }


        private void CreateThemeFiles(C3Addon addon, string folderName)
        {
            //generate file strings
            _addonFiles = new Dictionary<string, string>();

            //generate simple files
            LogManager.CompilerLog.Insert($"generating addon.json");
            _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "addon.json"), addon.AddonJson);
            LogManager.CompilerLog.Insert($"generating addon.json => complete");

            LogManager.CompilerLog.Insert($"generating css ");
            _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "theme.css"), addon.ThemeCss);
            LogManager.CompilerLog.Insert($"generating css  => complete");

            LogManager.CompilerLog.Insert($"generating en-US.json");
            _addonFiles.Add(Path.Combine(OptionsManager.CurrentOptions.CompilePath, folderName, "lang", "en-US.json"), addon.ThemeLangauge);
            LogManager.CompilerLog.Insert($"generating en-US.json => complete");

            //write files to path
            foreach (var file in _addonFiles)
            {
                System.IO.File.WriteAllText(file.Key, file.Value);
                LogManager.CompilerLog.Insert($"writing file => {file.Key}");
            }

            LogManager.CompilerLog.Insert($"compilation complete...");
        }


        //todo: add effect validation here
        //todo: move this into the validate addons helper
        /// <summary>
        /// run some simple validations for addon
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        private bool ValidateFiles(C3Addon addon)
        {
            if (addon.Type == PluginType.Effect)
            {
                if (!AddonValidator.Insatnce.Validate(addon))
                {
                    return false;
                }
            }
            else if (addon.Type == PluginType.Theme)
            {
                //todo: validate css
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addon.LanguageProperties))
                {
                    LogManager.CompilerLog.Insert("generate properties json has not been ran, generate the json in the langauge view");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(addon.LanguageCategories))
                {
                    LogManager.CompilerLog.Insert("generate category json has not been ran, generate the json in the langauge view");
                    return false;
                }

                var placeholder = new Regex("{(\\d+)}|{\\.\\.\\.}");

                foreach (var action in addon.Actions)
                {
                    var paramCount = action.Value.Ace.Count(x => x == '{') - 1;
                    var displayCount = placeholder.Matches(action.Value.Language).Count;

                    if (paramCount != displayCount)
                    {
                        LogManager.CompilerLog.Insert($"invalid amount of parameter placeholder in display text for {action.Value.Id}, {paramCount} parameters expected {displayCount} placeholders {{#}} in display text");
                        return false;
                    }
                }

                foreach (var condition in addon.Conditions)
                {
                    var paramCount = condition.Value.Ace.Count(x => x == '{') - 1;
                    var displayCount = placeholder.Matches(condition.Value.Language).Count;

                    if (paramCount != displayCount)
                    {
                        LogManager.CompilerLog.Insert($"invalid amount parameter placeholder in display text for {condition.Value.Id}, {paramCount} parameters expected {displayCount} placeholders {{#}} in display text");
                        return false;
                    }
                }
            }
           
            return true;
        }

        /// <summary>
        /// generates the strings for actions
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        private string CompileActions(C3Addon addon)
        {
            var actions = addon.Actions.Select(x => x.Value.Code);
            var actionString = string.Join(",\n\n", actions);
            var pluginType = addon.Type == PluginType.Behavior ? "Behaviors" : "Plugins";

            return $@"""use strict"";
{{
    C3.{pluginType}.{addon.AddonId}.Acts = {{
        {actionString}
    }};
}}";
        }

        /// <summary>
        /// generates the strings for conditions
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        private string CompileConditions(C3Addon addon)
        {
            var conditions = addon.Conditions.Select(x => x.Value.Code);
            var conditionString = string.Join(",\n\n", conditions);
            var pluginType = addon.Type == PluginType.Behavior ? "Behaviors" : "Plugins";

            return $@"""use strict"";
{{
    C3.{pluginType}.{addon.AddonId}.Cnds = {{
        {conditionString}
    }};
}}";
        }

        /// <summary>
        /// generates the string for expressions
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        private string CompileExpressions(C3Addon addon)
        {
            var expressions = addon.Expressions.Select(x => x.Value.Code);
            var expressionString = string.Join(",\n\n", expressions);
            var pluginType = addon.Type == PluginType.Behavior ? "Behaviors" : "Plugins";

            return $@"""use strict"";
{{
    C3.{pluginType}.{addon.AddonId}.Exps = {{
        {expressionString}
    }};
}}";
        }

        /// <summary>
        /// generates the string for the language file
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        private string CompileLang(C3Addon addon)
        {
            var actionList = addon.Actions.Select(x => x.Value.Language);
            var conditionList = addon.Conditions.Select(x => x.Value.Language);
            var expressionList = addon.Expressions.Select(x => x.Value.Language);

            var actionString = string.Join(",\n", actionList);
            var conditionString = string.Join(",\n", conditionList);
            var expressionString = string.Join(",\n", expressionList);

            var pluginType = addon.Type == PluginType.Behavior ? "behaviors" : "plugins";

            return $@"{{
    ""languageTag"": ""en-US"",
    ""fileDescription"": ""Strings for {addon.Name} Plugin"",
    ""text"": {{
        ""{pluginType}"": {{
            ""{addon.AddonId.ToLower()}"": {{
                ""name"": ""{addon.Name}"",
                ""description"": ""{addon.Description}"",
                ""help-url"": ""https://github.com/armandoalonso/c3IDE"",
                {addon.LanguageProperties},
                {addon.LanguageCategories},
                ""conditions"": {{
                    {conditionString}
                }},
                ""actions"": {{
                    {actionString}
                }},
                ""expressions"": {{
                    {expressionString}
                }}
            }}
        }}
    }}
}}";
        }

        /// <summary>
        /// generates the string for the ace json
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        private string CompileAce(C3Addon addon)
        {
            var categoryAce = new List<string>();
            var categoryList = addon.Categories;

            foreach (var category in categoryList)
            {
                var actions = addon.Actions.Values.Where(x => x.Category == category);
                var conditions = addon.Conditions.Values.Where(x => x.Category == category);
                var expressions = addon.Expressions.Values.Where(x => x.Category == category);

                var actionString = string.Join(",\n", actions.Select(x => x.Ace));
                var conditionString = string.Join(",\n", conditions.Select(x => x.Ace));
                var expressionString = string.Join(",\n", expressions.Select(x => x.Ace));

                var template = $@"""{category}"": {{
    ""conditions"":[
        {conditionString}
    ],
    ""actions"":[
        {actionString}
    ],
    ""expressions"":[
        {expressionString}
    ]
}}";
                categoryAce.Add(template);
            }

            return $@"{{
    {string.Join(",\n", categoryAce)}
}}";
        }

        private string CompileEffectAddon(C3Addon addon)
        {
            var parameters = string.Join(",\n", addon.Effect.Parameters.Select(x => x.Value.Json));

            return $@"{{
	""is-c3-addon"": true,
	""type"": ""effect"",
	""name"": ""{addon.Name}"",
	""id"": ""{addon.AddonId}"",
	""version"": ""{addon.MajorVersion}.{addon.MinorVersion}.{addon.RevisionVersion}.{addon.RevisionVersion}"",
	""author"": ""{addon.Author}"",
	""website"": ""https://www.construct.net"",
	""documentation"": ""https://www.construct.net"",
	""description"": ""{addon.Description}"",
	""file-list"": [
		""lang/en-US.json"",
		""addon.json"",
		""effect.fx""
	],
	
	""category"": ""{addon.AddonCategory}"",
	""blends-background"": {addon.Effect.BlendsBackground.ToString().ToLower()},
	""cross-sampling"": {addon.Effect.CrossSampling.ToString().ToLower()},
	""preserves-opaqueness"": {addon.Effect.PreservesOpaqueness.ToString().ToLower()},
	""animated"": {addon.Effect.Animated.ToString().ToLower()},
	""must-predraw"" : {addon.Effect.MustPredraw.ToString().ToLower()},

	""extend-box"": {{
		""horizontal"": {addon.Effect.ExtendBoxHorizontal},
		""vertical"": {addon.Effect.ExtendBoxVertical}
	}},
	
	""parameters"": [
        {parameters}
	]
}}";
        }

        private string CompileEffectLang(C3Addon addon)
        {
            var parameters = string.Join(",\n", addon.Effect.Parameters.Select(x => x.Value.Lang));

            return $@"{{
	""languageTag"": ""en-US"",
	""fileDescription"": ""Strings for the '{addon.Class}' effect."",
	""text"": {{
		""effects"": {{
			""{addon.AddonId.ToLower()}"": {{
				""name"": ""{addon.Name}"",
				""description"": ""{addon.Description}"",
				""parameters"": {{
					{parameters}
				}}
			}}
		}}
	}}
}}";
        }
    }
}
