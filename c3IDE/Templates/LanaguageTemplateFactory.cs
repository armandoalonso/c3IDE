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
            return JsonHelper.Insatnce.FormatJson(template);
        }

        private string GeneratePropertLang(C3Plugin data)
        {
            var propertyList = new List<string>();

            foreach (var property in data.Plugin.Properties)
            {
                string template;
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
            var categoryList = data.Aces.Categories.Select(category => $@"""{category.Key}"": ""{category.Value}""").ToList();
            return string.Join(",\n", categoryList);
        }

        private string GenerateConditionsLang(C3Plugin data)
        {
            var conditionList = new List<string>();

            foreach (var condition in data.Aces.Conditions)
            {
                var parameters = $@"                ""params"": {{
                    {condition.ParamLangList}
                }}";

                if (condition.Params.Any())
                {
                    var template = $@"""{condition.Id}"": {{
						""list-name"": ""{condition.ListName}"",
						""display-text"": ""{condition.DisplayText}"",
						""description"": ""{condition.Description}"",
                        {parameters}
                   }}";
                    conditionList.Add(template);
                }
                else
                {
                    var template = $@"""{condition.Id}"": {{
						""list-name"": ""{condition.ListName}"",
						""display-text"": ""{condition.DisplayText}"",
						""description"": ""{condition.Description}""
                   }}";
                    conditionList.Add(template);
                }
            }

            return string.Join(",\n", conditionList);
        }

        private string GenerateActionsLang(C3Plugin data)
        {
            var actionList = new List<string>();

            foreach (var action in data.Aces.Actions)
            {
                var parameters = $@"                ""params"": {{
                    {action.ParamLangList}
                }}";

                if (action.Params.Any())
                {
                    var template = $@"""{action.Id}"": {{
						""list-name"": ""{action.ListName}"",
						""display-text"": ""{action.DisplayText}"",
						""description"": ""{action.Description}"",
                        {parameters}
                   }}";
                    actionList.Add(template);
                }
                else
                {
                    var template = $@"""{action.Id}"": {{
						""list-name"": ""{action.ListName}"",
						""display-text"": ""{action.DisplayText}"",
						""description"": ""{action.Description}""
                   }}";
                    actionList.Add(template);
                }
            }

            return string.Join(",\n", actionList);
        }

        private string GenerateExpressionsLang(C3Plugin data)
        {
            var expressionList = new List<string>();

            foreach (var expression in data.Aces.Expressions)
            {
                var parameters = $@"                ""params"": {{
                    {expression.ParamLangList}
                }}";

                if (expression.Params.Any())
                {
                    var template = $@"""{expression.Id}"": {{
						""description"": ""{expression.Description}"",
					    ""translated-name"": ""{expression.TranslatedName}"",
                        {parameters}
                   }}";
                    expressionList.Add(template);
                }
                else
                {
                    var template = $@"""{expression.Id}"": {{
						""description"": ""{expression.Description}"",
					    ""translated-name"": ""{expression.TranslatedName}""
                   }}";
                    expressionList.Add(template);
                }
            }

            return string.Join(",\n", expressionList);
        }
    }
}
