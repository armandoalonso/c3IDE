using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;

namespace c3IDE.Managers
{
    public class C2EffectConverter : Singleton<C2EffectConverter>
    {
        private C3Addon c3addon ;

        public C3Addon ConvertEffect(string effextXml, string effectCode)
        {
            c3addon = new C3Addon();
            ParseXml(effextXml);

            c3addon.Effect.Code = effectCode;
            c3addon.CreateDate = DateTime.Now;
            c3addon.LastModified = DateTime.Now;
            c3addon.MajorVersion = 1;
            c3addon.MinorVersion = 0;
            c3addon.RevisionVersion = 0;
            c3addon.BuildVersion = 0;

            c3addon.Type = PluginType.Effect;
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            c3addon.IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg");

            return c3addon;
        }

        private void ParseXml(string effextXml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(effextXml);
            var root = xmlDoc.GetElementsByTagName("c2effect")[0];

            var id = xmlDoc.GetElementsByTagName("id")[0].InnerText;
            c3addon.AddonId = id;

            var name = xmlDoc.GetElementsByTagName("name")[0].InnerText;
            c3addon.Name = name;
            c3addon.Class = name.Replace(" ", string.Empty);

            var category = xmlDoc.GetElementsByTagName("category")[0].InnerText.ToLower();
            c3addon.AddonCategory = category;

            var desc = xmlDoc.GetElementsByTagName("description")[0].InnerText;
            c3addon.Description = desc;

            var author = xmlDoc.GetElementsByTagName("author")[0].InnerText;
            c3addon.Author = author;
            c3addon.Company = author;

            c3addon.Effect = new Effect();

            try
            {
                var extendH = xmlDoc.GetElementsByTagName("extend-box-horizontal")[0].InnerText;
                c3addon.Effect.ExtendBoxHorizontal = int.Parse(extendH);
            }
            catch
            {
                c3addon.Effect.ExtendBoxHorizontal = 0;
            }

            try
            {
                var extendV = xmlDoc.GetElementsByTagName("extend-box-vertical")[0].InnerText;
                c3addon.Effect.ExtendBoxHorizontal = int.Parse(extendV);
            }
            catch
            {
                c3addon.Effect.ExtendBoxVertical = 0;
            }

            var blendbg = xmlDoc.GetElementsByTagName("blends-background").Count > 0 ? xmlDoc.GetElementsByTagName("animated")[0].InnerText : "false";
            c3addon.Effect.BlendsBackground = blendbg == "true";

            var crosssample = xmlDoc.GetElementsByTagName("cross-sampling").Count > 0 ? xmlDoc.GetElementsByTagName("cross-sampling")[0].InnerText : "false";
            c3addon.Effect.CrossSampling = crosssample == "true";

            var animated = xmlDoc.GetElementsByTagName("animated").Count > 0 ? xmlDoc.GetElementsByTagName("animated")[0].InnerText : "false";
            c3addon.Effect.Animated = animated == "true";

            c3addon.Effect.Parameters = new Dictionary<string, EffectParameter>();
            var parameters = xmlDoc.GetElementsByTagName("param");
            if (parameters.Count > 0)
            {
                foreach (XmlNode param in parameters)
                {
                    var parameter = new EffectParameter();

                    var pname = param.SelectSingleNode("name")?.InnerText;
                    var pdesc = param.SelectSingleNode("description")?.InnerText;
                    var ptype = param.SelectSingleNode("type")?.InnerText;
                    var pinit = param.SelectSingleNode("initial")?.InnerText;
                    var puniform = param.SelectSingleNode("uniform")?.InnerText;
                    var pid = Regex.Replace(pname ?? "", @"[^a-zA-Z0-9]", "-");

                    string initValue = "";
                    string varDec = "";

                    switch (ptype)
                    {
                        case "float":
                        case "percent":
                            initValue = $"{pinit}";
                            varDec = $"uniform lowp float {puniform};";
                            break;
                        case "color":
                            varDec = $"uniform lowp vec3 {puniform};";
                            break;
                    }

                    parameter.Json = $@"{{
    ""id"":""{pid}"",
    ""type"": ""{ptype}"",
    ""initial-value"":{initValue},
    ""uniform"": ""{puniform}""
}}";

                    parameter.Lang = $@"""{pid}"": {{
    ""name"": ""{pname}"",
    ""desc"": ""{pdesc}""
}}";

                    parameter.Key = id;
                    parameter.VariableDeclaration = varDec;
                    c3addon.Effect.Parameters.Add(pid, parameter);
                }     
            }
          
        }
    }
}
