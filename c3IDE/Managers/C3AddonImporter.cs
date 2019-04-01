using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace c3IDE.Managers
{
    public static class C3AddonImporter
    {
        public static void Import(string path)
        {
            var fi = new FileInfo(path);
            var tmpPath = OptionsManager.CurrentOptions.DataPath + "\\tmp";
            if(Directory.Exists(tmpPath)) Directory.Delete(tmpPath, true);


            //unzip c3addon to temp location
            ZipFile.ExtractToDirectory(path, tmpPath);

            var addon = JObject.Parse(File.ReadAllText(Path.Combine(tmpPath, "addon.json")));
            string type = addon["type"].ToString();
            string id = addon["id"].ToString();

            if (type != "effect")
            {
                string pluginEdit, pluginRun;
                pluginEdit = File.ReadAllText(Path.Combine(tmpPath, $"{type}.js"));
                pluginRun = File.ReadAllText(Path.Combine(tmpPath, "c3runtime", $"{type}.js"));
                string typeEdit = File.ReadAllText(Path.Combine(tmpPath, $"type.js"));
                string typeRun = File.ReadAllText(Path.Combine(tmpPath, "c3runtime", $"type.js"));
                string instanceEdit = File.ReadAllText(Path.Combine(tmpPath, $"instance.js"));
                string instanceRun = File.ReadAllText(Path.Combine(tmpPath, "c3runtime", $"instance.js"));

                //get ace
                //var ace = JsonConvert.DeserializeObject<IDictionary<string, object>>(File.ReadAllText(Path.Combine(tmpPath, "aces.json")));
                //var lang = JsonConvert.DeserializeObject<IDictionary<string, object>>(File.ReadAllText(Path.Combine(tmpPath, "lang", "en-US.json")));

                var ace = JObject.Parse(File.ReadAllText(Path.Combine(tmpPath, "aces.json")));
                var lang = JObject.Parse(File.ReadAllText(Path.Combine(tmpPath, "lang", "en-US.json")))["text"][type+"s"][id.ToLower()];

                foreach (JProperty category in ace.Properties())
                {
                    var actions = JArray.Parse(ace[category.Name]["actions"].ToString());
                    foreach (var action in actions.Children<JObject>())
                    {
                        var actionId = action["id"].ToString();
                        var actionAce = action.ToString();
                        var actionLang = $"\"{actionId}\":" + lang["actions"][actionId];
                        var actionParams = action["params"].Children<JObject>().Select(x => x["id"].ToString());
                    }

                    var conditions = JArray.Parse(ace[category.Name]["conditions"].ToString());
                   
                    var expressions = JArray.Parse(ace[category.Name]["expressions"].ToString());
                }
            }
            else
            {

            }



        }
    }
}
