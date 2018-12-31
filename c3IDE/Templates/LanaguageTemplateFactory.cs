using c3IDE.Models;
using c3IDE.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Templates
{
    public class LanaguageTemplateFactory : Singleton<LanaguageTemplateFactory>
    {
        public string Create(C3Plugin data)
        {
            //base lanagauge file
            var template = $@"{{
	""languageTag"": ""en-US"",
	""fileDescription"": ""Strings for {data.Plugin.Name} Plugin"",
	""text"": {{
		""plugins"": {{
			""{data.Plugin.Company.ToLower()}_{data.Plugin.ClassName.ToLower()}"": {{
				""name"": ""{data.Plugin.Name}"",
				""description"": ""{data.Plugin.Description}"",
				""help-url"": ""{data.Plugin.Documentation}"",
				""properties"": {{
                    {GeneratePropertLang(data)}
				}},
				""aceCategories"": {{
					{GenerateCategoryLang(data)}
				}},
				""conditions"": {{
                    {GenerateConditionsLang(data)}
				}},
				""actions"": {{
					{GenerateActionsLang(data)}				
				}},
				""expressions"": {{
					{GenerateExpressionsLang(data)}	
				}}		
			}}
		}}
	}}
}}";

            //properties

            //categories

            //actions

            //conditions
                
            //expression


            return string.Empty;
        }


        private string GeneratePropertLang(C3Plugin data)
        {
            var propertyList = new List<string>();

            foreach (var property in data.Plugin.Properties)
            {
                string template = string.Empty;
                switch (property.Type)
                {
                    case "combo":
                        template = $@"""{property.Id}"": {{
						""name"": ""{property.Name}"",
						""desc"": ""{property.Description}"",
                        ""items"": {{
                            {GeneratePropertyItems(property)}
                        }}
					}}";
                        break;
                    case "link":
                        template = $@"""{property.Id}"": {{
						""name"": ""{property.Name}"",
						""desc"": ""{property.Description}"",
                        ""link-text"": ""{property.LinkText}""
					}}";
                        break;
                    default:
                        template = $@"""{property.Id}"": {{
						""name"": ""{property.Name}"",
						""desc"": ""{property.Description}""
					}}";
                        break;
                }
                propertyList.Add(template);
            }

            return string.Join(",\n", propertyList);
        }

        private string GeneratePropertyItems(Property property)
        {
            var itemList = property.Items.Select(propertyItem => $@"""{propertyItem.Key}"": ""{propertyItem.Value}""").ToList();
            return string.Join(",\n", itemList);
        }


        private string GenerateCategoryLang(C3Plugin data)
        {
            throw new NotImplementedException();
        }

        private string GenerateConditionsLang(C3Plugin data)
        {
            throw new NotImplementedException();
        }

        private string GenerateActionsLang(C3Plugin data)
        {
            throw new NotImplementedException();
        }

        private string GenerateExpressionsLang(C3Plugin data)
        {
            throw new NotImplementedException();
        }
    }
}
