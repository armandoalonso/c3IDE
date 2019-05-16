using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities.Helpers;
using Esprima.Ast;
using Newtonsoft.Json;
using Expression = c3IDE.Models.Expression;

namespace c3IDE.Managers
{
    public static class AddonManager
    {
        public static List<C3Addon> AllAddons { get; set; }
        public static C3Addon CurrentAddon { get; set; }
        private static readonly List<Action<C3Addon>> _addonLoadedCallback = new List<Action<C3Addon>>();

        /// <summary>
        /// subscribe a callback to the loaded addon 
        /// </summary>
        /// <param name="loadedCallback"></param>
        /// <returns></returns>
        public static int AddLoadedCallback(Action<C3Addon> loadedCallback)
        {
            var index = _addonLoadedCallback.Count;
            _addonLoadedCallback.Add(loadedCallback);
            return index;
        }

        /// <summary>
        /// sets the cuurent addon
        /// </summary>
        /// <param name="addon"></param>
        public static void LoadAddon(C3Addon addon)
        {
            //check for empty id
            if (string.IsNullOrWhiteSpace(addon.AddonId))
            {
                addon.AddonId = $"{addon.Author}_{addon.Class}";
            }
            //load addon
            CurrentAddon = addon;

            NotificationManager.PublishNotification($"{addon.Name} loaded successfully");
            foreach (var callback in _addonLoadedCallback)
            {
                callback?.Invoke(addon);
            }
        }

        /// <summary>
        /// loads all addons from storage
        /// </summary>
        public static void LoadAllAddons()
        {
            AllAddons = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
        }

        /// <summary>
        /// saves the cuurently loaded addon
        /// </summary>
        public static void SaveCurrentAddon()
        {
            DataAccessFacade.Insatnce.AddonData.Upsert(CurrentAddon);
        }

        /// <summary>
        /// removes the passed in addon, based on guid id
        /// </summary>
        /// <param name="addon"></param>
        public static void DeleteAddon(C3Addon addon)
        {
            CurrentAddon = null;
            DataAccessFacade.Insatnce.AddonData.Delete(addon);
            LoadAllAddons();
            WindowManager.ChangeWindow(ApplicationWindows.DashboardWindow);
        }

        /// <summary>
        /// changes the id of the passe din addon, and upsert it back to duplicate it
        /// </summary>
        /// <param name="addon"></param>
        public static void DuplicateAddon(C3Addon addon, string name = null)
        {
            addon.Id = Guid.NewGuid();
            addon.CreateDate = DateTime.Now;

            DataAccessFacade.Insatnce.AddonData.Upsert(addon);
            LoadAllAddons();
        }

        /// <summary>
        /// validates all the fields for the current addon are populated correctly
        /// </summary>
        /// <returns></returns>
        public static bool ValidateCurrentAddon()
        {
            if (string.IsNullOrWhiteSpace(CurrentAddon.Class) ||
                string.IsNullOrWhiteSpace(CurrentAddon.Company) ||
                string.IsNullOrWhiteSpace(CurrentAddon.Name) ||
                string.IsNullOrWhiteSpace(CurrentAddon.Author) ||
                string.IsNullOrWhiteSpace(CurrentAddon.Description))
            {
                return false;
            }
                return true;
        }

        /// <summary>
        /// creates a c3ide project file in the export folder based on the loaded addon
        /// </summary>
        public static string ExportAddonProject()
        {
            if (CurrentAddon == null) return null;
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var name = OptionsManager.CurrentOptions.IncludeTimeStampOnExport
                ? $"{CurrentAddon.Author}_{CurrentAddon.Class}_{timestamp}"
                : $"{CurrentAddon.Author}_{CurrentAddon.Class}";
            var path = Path.Combine(OptionsManager.CurrentOptions.ExportPath, name);

            if (OptionsManager.CurrentOptions.ExportSingleFileProject)
            {
                var addonJson = JsonConvert.SerializeObject(CurrentAddon);
                ProcessHelper.Insatnce.WriteFile($"{path}.c3ide", addonJson);
                return OptionsManager.CurrentOptions.ExportPath;
            }
            else
            {
                ProjectManager.WriteProject(CurrentAddon, path);
                return path;
            }
        }

        /// <summary>
        /// compiles the templates into the addon properties for each section
        /// </summary>
        public static void CompileTemplates()
        {
          CompileTemplates(CurrentAddon);
        }

        public static void CompileTemplates(C3Addon addon)
        {
            //compile standard templates
            addon.AddonJson = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.AddonJson, addon);
            addon.PluginEditTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.PluginEditTime, addon);
            addon.PluginRunTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.PluginRunTime, addon);
            addon.TypeEditTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.TypeEditTime, addon);
            addon.TypeRunTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.TypeRunTime, addon);
            addon.InstanceEditTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.InstanceEditTime, addon);
            addon.InstanceRunTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.InstanceRunTime, addon);
            addon.Actions = new Dictionary<string, Models.Action>();
            addon.Conditions = new Dictionary<string, Condition>();
            addon.Expressions = new Dictionary<string, Expression>();
            addon.ThirdPartyFiles = new Dictionary<string, ThirdPartyFile>();
            addon.LanguageProperties = addon.Template.LanguageProperty;
            addon.LanguageCategories = addon.Template.LanguageCategory;

            //compile effect template
            addon.Effect.Code = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.EffectCode, addon);

            //compile theme
            addon.ThemeCss = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ThemeCode, addon);
            addon.ThemeLangauge = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ThemeLanguage, addon);
        }

        public static void IncrementVersion()
        {
            if (OptionsManager.CurrentOptions.AutoIncrementVersionOnPublish)
            {
                CurrentAddon.BuildVersion++;
                UpdateAddonJsonVersion();
                SaveCurrentAddon();
            }
        }

        public static void UpdateAddonJsonVersion()
        {
            var versionRegex = @"\d+\.\d+\.\d+\.\d+";
            CurrentAddon.AddonJson = Regex.Replace(CurrentAddon.AddonJson, versionRegex, CurrentAddon.Version);
            CurrentAddon.PluginEditTime = Regex.Replace(CurrentAddon.PluginEditTime, versionRegex, CurrentAddon.Version);
        }
    }
}
