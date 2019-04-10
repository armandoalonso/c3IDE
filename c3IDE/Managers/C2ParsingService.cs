using System;
using System.Collections.Generic;
using System.Text;
using c3IDE.Models;
using c3IDE.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;



namespace c3IDE.Managers
{
    public class C2ParsingService : Singleton<C2ParsingService>
    {
        public C2Addon Execute(string edittime)
        {
            try
            {
                var client = new RestClient("https://addon-parser-armaldio.webcreationclub.now.sh/parse/c2");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "multipart/form-data");
                request.AddFile("file", Encoding.ASCII.GetBytes(edittime), "edittime.js", "application/javascript");
                IRestResponse response = client.Execute(request);
                LogManager.AddImportLogMessage($"RESPONSE => {response.Content}");
                return Parse(response.Content);
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"c2 parsing service failed => {ex.Message}");
                return null;
            }
        }

        public C2Addon Parse(string json)
        {
            var c2addon = new C2Addon();
            dynamic data = JObject.Parse(json);

            //settings
            var settings = data["settings"];
            foreach (JProperty prop in settings)
            {
                //handle array
                if (prop.Name == "flags")
                {
                    var sb = new StringBuilder();
                    foreach (var val in prop.Value)
                    {
                        sb.Append($" {val} ");
                    }
                    c2addon.Properties.Add(prop.Name,sb.ToString());
                    continue;
                }

                c2addon.Properties.Add(prop.Name, prop.Value.ToString());
            }

            //properties
            var props = data["properties"];
            foreach (var p in props)
            {
                var property = new C2Property();

                property.Type = p["flags"].ToString();
                property.Name = p["key"].ToString();
                property.Value = p["initial_str"]?.ToString() ?? string.Empty;
                property.Description = p["description"].ToString();
                var flags = p["params"];
                var fList = new List<string>();
                foreach (var flag in flags)
                {
                    fList.Add(flag);
                }
                property.Params = string.Join("|", fList);
                property.Readonly = p["read_only"] != null ? p["read_only"].ToString().ToLower() : "false";
            }

            //actions
            var actions = data["actions"];
            c2addon.Actions = new List<C2Ace>();
            foreach (JObject act in actions)
            {
                var ace = new C2Ace
                {
                    Id = act["id"].ToString(),
                    ListName = act["list_name"].ToString(),
                    Category = act["category"].ToString(),
                    DisplayString = act["display_string"].ToString(),
                    Description = act["description"].ToString(),
                    ScriptName = act["script_name"].ToString()
                };

                //get flags
                var sb = new StringBuilder();
                foreach (var val in act["flags"])
                {
                    sb.Append($" {val} ");
                }
                ace.Flags = sb.ToString();

                //params
                foreach (var param in act["params"])
                {
                    var aceParam = new C2AceParam
                    {
                        Text = param["name"]?.ToString(),
                        Description = param["description"]?.ToString(),
                        DefaultValue = param["initial"]?.ToString(),
                        Script = param["caller"]?.ToString()
                    };

                    if (param["caller"]?.ToString() == "AddComboParam")
                    {
                        aceParam.ComboItems = new List<string>();
                        foreach (var val in param["options"])
                        {
                            aceParam.ComboItems.Add(val["text"].ToString());
                        }
                    }

                    ace.Params.Add(aceParam);
                    c2addon.Actions.Add(ace);
                }

            }

            //conditions
            var conditions = data["conditions"];
            c2addon.Conditions = new List<C2Ace>();
            foreach (JObject cnd in conditions)
            {
                var ace = new C2Ace
                {
                    Id = cnd["id"].ToString(),
                    ListName = cnd["list_name"].ToString(),
                    Category = cnd["category"].ToString(),
                    DisplayString = cnd["display_string"].ToString(),
                    Description = cnd["description"].ToString(),
                    ScriptName = cnd["script_name"].ToString()
                };

                //get flags
                var sb = new StringBuilder();
                foreach (var val in cnd["flags"])
                {
                    sb.Append($" {val} ");
                }
                ace.Flags = sb.ToString();

                //params
                foreach (var param in cnd["params"])
                {
                    var aceParam = new C2AceParam
                    {
                        Text = param["name"]?.ToString(),
                        Description = param["description"]?.ToString(),
                        DefaultValue = param["initial"]?.ToString(),
                        Script = param["caller"]?.ToString()
                    };

                    if (param["caller"]?.ToString() == "AddComboParam")
                    {
                        aceParam.ComboItems = new List<string>();
                        foreach (var val in param["options"])
                        {
                            aceParam.ComboItems.Add(val["text"].ToString());
                        }
                    }

                    ace.Params.Add(aceParam);
                }
                c2addon.Conditions.Add(ace);
            }

            //expressions
            var expressions = data["expressions"];
            c2addon.Expressions = new List<C2Ace>();
            foreach (JObject exp in expressions)
            {
                var ace = new C2Ace
                {
                    Id = exp["id"].ToString(),
                    ListName = exp["list_name"].ToString(),
                    Category = exp["category"].ToString(),
                    Description = exp["description"].ToString(),
                    ScriptName = exp["expression_name"].ToString(),                 
                };

                //get flags
                var sb = new StringBuilder();
                foreach (var val in exp["flags"])
                {
                    sb.Append($" {val} ");
                }
                ace.Flags = sb.ToString();

                //params
                foreach (var param in exp["params"])
                {
                    var aceParam = new C2AceParam
                    {
                        Text = param["name"]?.ToString(),
                        Description = param["description"]?.ToString(),
                        DefaultValue = param["initial"]?.ToString(),
                        Script = param["caller"]?.ToString()
                    };

                    if (param["caller"]?.ToString() == "AddComboParam")
                    {
                        aceParam.ComboItems = new List<string>();
                        foreach (var val in param["options"])
                        {
                            aceParam.ComboItems.Add(val["text"].ToString());
                        }
                    }

                    ace.Params.Add(aceParam);
                }

                c2addon.Expressions.Add(ace);
            }

            return c2addon;
        }
    }
}
