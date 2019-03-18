using System.IO;
using System.IO.Compression;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Utilities;

namespace c3IDE.Compiler
{
    public class AddonExporter : Singleton<AddonExporter>
    {
        /// <summary>
        /// compiles the addon and creates a c3addon export file
        /// </summary>
        /// <param name="addon"></param>
        public async void ExportAddon(C3Addon addon)
        {
            //compile addon
            await AddonCompiler.Insatnce.CompileAddon(addon, false);
            if (AddonCompiler.Insatnce.IsCompilationValid)
            {
                //export c3addon file
                CreateC3AddonFile(addon, addon.AddonFolder);
            }
            else
            {
                NotificationManager.PublishErrorNotification( "compilation failed, no .c3addon file created, please test your addon to get error information");
            }
        }

        /// <summary>
        /// generate the c3addon zip file
        /// </summary>
        /// <param name="addon"></param>
        /// <param name="addonAddonFolder"></param>
        private void CreateC3AddonFile(C3Addon addon, string addonAddonFolder)
        {
            var outputPath = OptionsManager.CurrentOptions.C3AddonPath;
            var c3addonFile = System.IO.Path.Combine(outputPath,$"{addon.Class.ToLower()}_{addon.Version.Replace(".", "_")}.c3addon");
            if(System.IO.File.Exists(c3addonFile)) File.Delete(c3addonFile);
            ZipFile.CreateFromDirectory(addonAddonFolder, c3addonFile);
        }
    }
}
