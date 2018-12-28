namespace c3IDE.Templates
{
    public interface ITemplate
    {
        string Base64Icon { get; }
        string AddonJson { get; }
        string EditTimePluginJs { get; }
        string EditTimeTypeJs { get; }
        string EditTimeInstanceJs { get; }
        string AcesJson { get; }
        string LanguageJson { get; }
        string RunTimeTypeJs { get; set; }
        string RunTimeInstanceJs { get; set; }
        string RunTimePluginJs { get; }
        string ActionsJs { get; }
        string ConditionsJs { get; }
        string ExpressionsJs { get; }
    }
}
    