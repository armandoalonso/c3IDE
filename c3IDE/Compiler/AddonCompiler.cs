using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;
using Action = System.Action;

namespace c3IDE.Compiler
{
    public class AddonCompiler : Singleton<AddonCompiler>
    {
        private Dictionary<string, string> _addonFiles;
        public Action<string> UpdateLogText;
        private CompilerLog _log;

        public void CompileAddon(C3Addon addon)
        {
            _log = new CompilerLog(UpdateLogText);

            try
            {
                var vaild = ValidateFiles(addon);
                if (!vaild) return;

                _log.Insert($"compliation starting...");

                //clear out compile path
                if (Directory.Exists(AppData.Insatnce.Options.CompilePath))
                {
                    _log.Insert($"compile directory exists => {AppData.Insatnce.Options.CompilePath}");
                    System.IO.Directory.Delete(AppData.Insatnce.Options.CompilePath, true);
                    _log.Insert($"removed compile directory...");
                }

                //create new addon tmp folder
                _log.Insert($"recreating compile directory => {AppData.Insatnce.Options.CompilePath}");
                System.IO.Directory.CreateDirectory(AppData.Insatnce.Options.CompilePath);
                System.IO.Directory.CreateDirectory(Path.Combine(AppData.Insatnce.Options.CompilePath, "lang"));
                System.IO.Directory.CreateDirectory(Path.Combine(AppData.Insatnce.Options.CompilePath, "c3runtime"));
                _log.Insert($"compile directory created successfully => {AppData.Insatnce.Options.CompilePath}");

                //generate file strings
                _addonFiles = new Dictionary<string, string>();

                //generate simple files
                _log.Insert($"generating addon.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "addon.json"), FormatHelper.Insatnce.Json(addon.AddonJson, "addon.json", _log));
                _log.Insert($"generating addon.json => complete");

                _log.Insert($"generating plugin.js (edittime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "plugin.js"), FormatHelper.Insatnce.Javascript(addon.PluginEditTime));
                _log.Insert($"generating plugin.js (edittime) => complete");

                _log.Insert($"generating type.js (edittime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "type.js"), FormatHelper.Insatnce.Javascript(addon.TypeEditTime));
                _log.Insert($"generating type.js (edittime) => complete");

                _log.Insert($"generating instance.js (edittime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "instance.js"), FormatHelper.Insatnce.Javascript(addon.InstanceEditTime));
                _log.Insert($"generating instance.js (edittime) => complete");

                _log.Insert($"generating ace.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "aces.json"), FormatHelper.Insatnce.Json(CompileAce(addon), "aces.json", _log));
                _log.Insert($"generating ace.json => complete");

                _log.Insert($"generating en-US.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "lang", "en-US.json"), FormatHelper.Insatnce.Json(CompileLang(addon), "lang/en-US.json", _log));
                _log.Insert($"generating en-US.json => complete");

                _log.Insert($"generating plugin.js (runtime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "c3runtime", "plugin.js"), FormatHelper.Insatnce.Javascript(addon.PluginRunTime));
                _log.Insert($"generating plugin.js (runtime) => complete");

                _log.Insert($"generating type.js (runtime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "c3runtime", "type.js"), FormatHelper.Insatnce.Javascript(addon.TypeRunTime));
                _log.Insert($"generating type.js (runtime) => complete");

                _log.Insert($"generating instance.js (runtime)");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "c3runtime", "instance.js"), FormatHelper.Insatnce.Javascript(addon.InstanceRunTime));
                _log.Insert($"generating instance.js (runtime) => complete");

                _log.Insert($"generating action.js.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "c3runtime", "actions.js"), FormatHelper.Insatnce.Javascript(CompileActions(addon)));
                _log.Insert($"generating action.js => complete");

                _log.Insert($"generating conditions.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "c3runtime", "conditions.js"), FormatHelper.Insatnce.Javascript(CompileConditions(addon)));
                _log.Insert($"generating conditions.json => complete");

                _log.Insert($"generating expressions.json");
                _addonFiles.Add(Path.Combine(AppData.Insatnce.Options.CompilePath, "c3runtime", "expressions.js"), FormatHelper.Insatnce.Javascript(CompileExpressions(addon)));
                _log.Insert($"generating expressions.json => complete");

                var icon = ImageHelper.Insatnce.Base64ToImage(addon.IconBase64);

                //todo: add support for 3rd party files

                //write files to path
                foreach (var file in _addonFiles)
                {
                    System.IO.File.WriteAllText(file.Key, file.Value);
                    _log.Insert($"writing file => {file.Key}");
                }
                icon.Save(Path.Combine(AppData.Insatnce.Options.CompilePath, "icon.png"));
                _log.Insert($"writing file => {Path.Combine(AppData.Insatnce.Options.CompilePath, "icon.png")}");
                _log.Insert($"compliation complete...");

                //start web server installation
                //var ws = new WebServer();
                //ws.Start(_log);
                //ws.Shutdown(_log);
            }
            catch (Exception ex)
            {
                _log.Insert($"compliation terminated due to error...");
                _log.Insert($"error => {ex.Message}");
            }
        }

        private bool ValidateFiles(C3Addon addon)
        {
            //todo: add other validation here to help ensure a proper add has been created
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

            return true;
        }

        private string CompileActions(C3Addon addon)
        {
            var actions = addon.Actions.Select(x => x.Value.Code);
            var actionString = string.Join(",\n\n", actions);

            return $@"""use strict"";
{{
    C3.Plugins.{addon.Company}_{addon.Class}.Acts = {{
        {actionString}
    }};
}}";
        }

        private string CompileConditions(C3Addon addon)
        {
            var conditions = addon.Conditions.Select(x => x.Value.Code);
            var conditionString = string.Join(",\n\n", conditions);

            return $@"""use strict"";
{{
    C3.Plugins.{addon.Company}_{addon.Class}.Cnds = {{
        {conditionString}
    }};
}}";
        }

        private string CompileExpressions(C3Addon addon)
        {
            var expressions = addon.Conditions.Select(x => x.Value.Code);
            var expressionString = string.Join(",\n\n", expressions);

            return $@"""use strict"";
{{
    C3.Plugins.{addon.Company}_{addon.Class}.Exps = {{
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

            return $@"{{
    ""languageTag"": ""en-US"",
    ""fileDescription"": ""Strings for {addon.Name} Plugin"",
    ""text"": {{
        ""plugins"": {{
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
