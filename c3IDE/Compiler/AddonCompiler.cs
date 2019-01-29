using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Server;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Logging;
using Action = System.Action;

namespace c3IDE.Compiler
{
    public class AddonCompiler : Singleton<AddonCompiler>
    {
        private Dictionary<string, string> _addonFiles;
        public Action<string> UpdateLogText;
        private CompilerLog _log;
        public WebServerClient WebServer { get; set; }
        public bool IsCompilationValid { get; set; }

        public async Task<bool> CompileAddon(C3Addon addon, bool startWebServer = true)
        {
            _log = new CompilerLog(UpdateLogText);

            try
            {
                var vaild = ValidateFiles(addon);
                if (!vaild)
                {
                    IsCompilationValid = false;
                    return false;
                }
                IsCompilationValid = true;

                _log.Insert($"compliation starting...");

                //generate unique folder for specific addon class
                var folderName = addon.Class.ToLower();
                addon.AddonFolder = Path.Combine(AppData.Insatnce.Options.CompilePath, folderName);

                //clear out compile path
                if (Directory.Exists(addon.AddonFolder))
                {
                    _log.Insert($"compile directory exists => { addon.AddonFolder}");
                    System.IO.Directory.Delete(addon.AddonFolder, true);
                    _log.Insert($"removed compile directory...");
                }

                //create main compile directory
                _log.Insert($"recreating compile directory => { addon.AddonFolder}");
                if (!Directory.Exists(AppData.Insatnce.Options.CompilePath))
                {
                    System.IO.Directory.CreateDirectory(AppData.Insatnce.Options.CompilePath);
                }

                //create addon compile directory and addon specific paths
                System.IO.Directory.CreateDirectory(addon.AddonFolder);
                System.IO.Directory.CreateDirectory(Path.Combine(addon.AddonFolder, "lang"));
                System.IO.Directory.CreateDirectory(Path.Combine(addon.AddonFolder, "c3runtime"));
                _log.Insert($"compile directory created successfully => { addon.AddonFolder}");

                //generate file strings
                _addonFiles = new Dictionary<string, string>();

                //generate simple files
                _log.Insert($"generating addon.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "addon.json"), _log.WrapLogger(() => FormatHelper.Insatnce.Json(addon.AddonJson)));
                _log.Insert($"generating addon.json => complete");

                if (addon.Type == PluginType.Behavior)
                {
                    _log.Insert($"generating behavior.js (edittime)");
                    _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "behavior.js"), FormatHelper.Insatnce.Javascript(addon.PluginEditTime));
                    _log.Insert($"generating behavior.js (edittime) => complete");

                    _log.Insert($"generating behavior.js (runtime)");
                    _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "c3runtime", "behavior.js"), FormatHelper.Insatnce.Javascript(addon.PluginRunTime));
                    _log.Insert($"generating behavior.js (runtime) => complete");
                }
                else
                {
                    _log.Insert($"generating plugin.js (edittime)");
                    _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "plugin.js"), FormatHelper.Insatnce.Javascript(addon.PluginEditTime));
                    _log.Insert($"generating plugin.js (edittime) => complete");

                    _log.Insert($"generating plugin.js (runtime)");
                    _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "c3runtime", "plugin.js"), FormatHelper.Insatnce.Javascript(addon.PluginRunTime));
                    _log.Insert($"generating plugin.js (runtime) => complete");
                }

                _log.Insert($"generating type.js (edittime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "type.js"), FormatHelper.Insatnce.Javascript(addon.TypeEditTime));
                _log.Insert($"generating type.js (edittime) => complete");

                _log.Insert($"generating instance.js (edittime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "instance.js"), FormatHelper.Insatnce.Javascript(addon.InstanceEditTime));
                _log.Insert($"generating instance.js (edittime) => complete");

                _log.Insert($"generating ace.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "aces.json"), _log.WrapLogger(() => FormatHelper.Insatnce.Json(CompileAce(addon))));
                _log.Insert($"generating ace.json => complete");

                _log.Insert($"generating en-US.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "lang", "en-US.json"), _log.WrapLogger(() => FormatHelper.Insatnce.Json(CompileLang(addon))));
                _log.Insert($"generating en-US.json => complete");

                _log.Insert($"generating type.js (runtime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "c3runtime", "type.js"), FormatHelper.Insatnce.Javascript(addon.TypeRunTime));
                _log.Insert($"generating type.js (runtime) => complete");

                _log.Insert($"generating instance.js (runtime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "c3runtime", "instance.js"), FormatHelper.Insatnce.Javascript(addon.InstanceRunTime));
                _log.Insert($"generating instance.js (runtime) => complete");

                _log.Insert($"generating action.js.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "c3runtime", "actions.js"), FormatHelper.Insatnce.Javascript(CompileActions(addon)));
                _log.Insert($"generating action.js => complete");

                _log.Insert($"generating conditions.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "c3runtime", "conditions.js"), FormatHelper.Insatnce.Javascript(CompileConditions(addon)));
                _log.Insert($"generating conditions.json => complete");

                _log.Insert($"generating expressions.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "c3runtime", "expressions.js"), FormatHelper.Insatnce.Javascript(CompileExpressions(addon)));
                _log.Insert($"generating expressions.json => complete");

                _log.Insert("generating 3rd party files");
                foreach (var files in addon.ThirdPartyFiles.Values)
                {
                    _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "c3runtime", files.FileName), files.Content);
                    _log.Insert($"generating {files.FileName}");
                }
                _log.Insert("generating 3rd party files => complete");

                //write files to path
                foreach (var file in _addonFiles)
                {
                    System.IO.File.WriteAllText(file.Key, file.Value);
                    _log.Insert($"writing file => {file.Key}");
                }

                File.WriteAllText(Path.Combine(AppData.Insatnce.Options.CompilePath, folderName, "icon.svg"), addon.IconXml);
                _log.Insert($"writing file => icon.svg");
                _log.Insert($"compilation complete...");

            }
            catch (Exception ex)
            {
                IsCompilationValid = false;
                LogManager.Insatnce.Exceptions.Add(ex);
                _log.Insert($"compilation terminated due to error...");
                _log.Insert($"error => {ex.Message}");
                AppData.Insatnce.ErrorMessage("There was an error generating the addon, please check the log.");
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
                        WebServer.Start(_log);
                    });
                }           
            }
            catch (Exception ex)
            {
                IsCompilationValid = false;
                LogManager.Insatnce.Exceptions.Add(ex);
                _log.Insert($"web server failed to start...");
                AppData.Insatnce.ErrorMessage("The web server failed to start... check that the port 8080, is not being used by another application.");
                return false;
            }

            return true;
        }

        private bool ValidateFiles(C3Addon addon)
        {
            //TODO: add other validation here to help ensure a proper add has been created
            //validate all the files & properties 
            if (string.IsNullOrWhiteSpace(addon.LanguageProperties))
            {
                _log.Insert("generate properties json has not been ran, generate the json in the langauge view");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addon.LanguageCategories))
            {
                _log.Insert("generate category json has not been ran, generate the json in the langauge view");
                return false;
            }

            var placeholder = new Regex("{(\\d+)}");
            //validate actions
            foreach (var action in addon.Actions)
            {
                var paramCount = action.Value.Ace.Count(x => x == '{') - 1;
                var displayCount = placeholder.Matches(action.Value.Language).Count;

                if (paramCount != displayCount)
                {
                    _log.Insert($"invalid parameter placeholder {{0}} in display text for {action.Value.Id}");
                    return false;
                }
            }

            foreach (var condition in addon.Conditions)
            {
                var paramCount = condition.Value.Ace.Count(x => x == '{') - 1;
                var displayCount = placeholder.Matches(condition.Value.Language).Count;

                if (paramCount != displayCount)
                {
                    _log.Insert($"invalid parameter placeholder {{0}} in display text for {condition.Value.Id}");
                    return false;
                }
            }

            return true;
        }

        private string CompileActions(C3Addon addon)
        {
            var actions = addon.Actions.Select(x => x.Value.Code);
            var actionString = string.Join(",\n\n", actions);
            var pluginType = addon.Type == PluginType.Behavior ? "Behaviors" : "Plugins";

            return $@"""use strict"";
{{
    C3.{pluginType}.{addon.Company}_{addon.Class}.Acts = {{
        {actionString}
    }};
}}";
        }

        private string CompileConditions(C3Addon addon)
        {
            var conditions = addon.Conditions.Select(x => x.Value.Code);
            var conditionString = string.Join(",\n\n", conditions);
            var pluginType = addon.Type == PluginType.Behavior ? "Behaviors" : "Plugins";

            return $@"""use strict"";
{{
    C3.{pluginType}.{addon.Company}_{addon.Class}.Cnds = {{
        {conditionString}
    }};
}}";
        }

        private string CompileExpressions(C3Addon addon)
        {
            var expressions = addon.Expressions.Select(x => x.Value.Code);
            var expressionString = string.Join(",\n\n", expressions);
            var pluginType = addon.Type == PluginType.Behavior ? "Behaviors" : "Plugins";

            return $@"""use strict"";
{{
    C3.{pluginType}.{addon.Company}_{addon.Class}.Exps = {{
        {expressionString}
    }};
}}";
        }

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
            ""{addon.Company.ToLower()}_{addon.Class.ToLower()}"": {{
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
    }
}
