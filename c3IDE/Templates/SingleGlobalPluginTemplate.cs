using c3IDE.Utilities;

namespace c3IDE.Templates
{
      class SingleGlobalPluginTemplate : ITemplate
    {
        public SingleGlobalPluginTemplate()
        {
            ResourceReader.Insatnce.LogResourceFiles();
            AddonJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.AddonTemplate.txt");
            EditTimePluginJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.EditTimePluginTemplate.txt");
            EditTimeTypeJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.EditTimeTypeTemplate.txt");
            EditTimeInstanceJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.EditTimeInstanceTemplate.txt");
            AcesJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.AcesTemplate.txt");
            LanguageJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.LanguageTemplate.txt");
            RunTimePluginJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.RunTimePluginTemplate.txt");
            RunTimeTypeJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.RunTimeTypeTemplate.txt");
            RunTimeInstanceJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.RunTimeInstanceTemplate.txt");
            ActionsJs = RunTimeTypeJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.ActionsTempalte.txt");
            ConditionsJs = RunTimeTypeJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.ConditionsTempalte.txt");
            ExpressionsJs = RunTimeTypeJs = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.TemplateFiles.SingleGlobal.ExpressionsTempalte.txt");
            Base64Icon = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Templates.TemplateFiles.icon.png");
        }

        public string Base64Icon { get; }
        public string AddonJson { get; }
        public string EditTimePluginJs { get; }
        public string EditTimeTypeJs { get; }
        public string EditTimeInstanceJs { get; }
        public string AcesJson { get; }
        public string LanguageJson { get; }
        public string RunTimeTypeJs { get; set; }
        public string RunTimeInstanceJs { get; set; }
        public string RunTimePluginJs { get; }
        public string ActionsJs { get; }
        public string ConditionsJs { get; }
        public string ExpressionsJs { get; }
    }
}
