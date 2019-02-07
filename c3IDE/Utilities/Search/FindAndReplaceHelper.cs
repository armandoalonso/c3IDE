using System;
using System.Text.RegularExpressions;
using c3IDE.Models;
using c3IDE.Utilities.Logging;

namespace c3IDE.Utilities.Search
{
    public class FindAndReplaceHelper : Singleton<FindAndReplaceHelper>
    {
        public void ReplaceMetadata(string addonClass, string name, string author, string version, string desc, C3Addon addon, RegexOptions option = RegexOptions.None)
        {
            try
            {
                var id = $"{addon.Author}_{addon.Class}";
                var newId = $"{author}_{addonClass}";
                addon.AddonJson = ReplaceAll(id, newId, addon.AddonJson, option);
                var oldClass = addon.Class;
                addon.AddonJson = ReplaceAll(addon.Class, addonClass, addon.AddonJson, option);
                addon.AddonJson = ReplaceAll(addon.Name, name, addon.AddonJson, option);
                addon.AddonJson = ReplaceAll(addon.Author, author, addon.AddonJson, option);
                addon.AddonJson = ReplaceAll(addon.Version, version, addon.AddonJson, option);
                addon.AddonJson = ReplaceAll(addon.Description, desc, addon.AddonJson, option);

                //update id & classes
                if (addon.Type == PluginType.Effect)
                {
                    //todo: replace effect addons tokens
                }
                else
                {
                    addon.PluginEditTime = ReplaceAll(id, newId, addon.PluginEditTime, option);
                    addon.PluginRunTime = ReplaceAll(id, newId, addon.PluginRunTime, option);
                    addon.TypeEditTime = ReplaceAll(id, newId, addon.TypeEditTime, option);
                    addon.TypeRunTime = ReplaceAll(id, newId, addon.TypeRunTime, option);
                    addon.InstanceRunTime = ReplaceAll(id, newId, addon.InstanceRunTime, option);
                    addon.InstanceEditTime = ReplaceAll(id, newId, addon.InstanceEditTime, option);
                    addon.PluginEditTime = ReplaceAll($"{oldClass}Plugin", $"{addonClass}Plugin", addon.PluginEditTime, option);
                    addon.PluginRunTime = ReplaceAll($"{oldClass}Plugin", $"{addonClass}Plugin", addon.PluginRunTime, option);
                    addon.PluginEditTime = ReplaceAll($"{oldClass}Behavior", $"{addonClass}Behavior", addon.PluginEditTime, option);
                    addon.PluginRunTime = ReplaceAll($"{oldClass}Behavior", $"{addonClass}Behavior", addon.PluginRunTime, option);
                    addon.TypeEditTime = ReplaceAll($"{oldClass}Type", $"{addonClass}Type", addon.TypeEditTime, option);
                    addon.TypeRunTime = ReplaceAll($"{oldClass}Type", $"{addonClass}Type", addon.TypeRunTime, option);
                    addon.InstanceRunTime = ReplaceAll($"{oldClass}Instance", $"{addonClass}Instance", addon.InstanceRunTime, option);
                    addon.InstanceEditTime = ReplaceAll($"{oldClass}Instance", $"{addonClass}Instance", addon.InstanceEditTime, option);
                }
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
                AppData.Insatnce.ErrorMessage($"update addon failed => {ex.Message}");
            }
           
        }

        public string ReplaceAll(string oldValue, string newValue, string input, RegexOptions option = RegexOptions.None)
        {
            return Regex.Replace(input, oldValue, newValue, option);
        }
    }
}
