using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;

namespace c3IDE.Compiler
{
    public class AddonExporter : Singleton<AddonExporter>
    {
        public void ExportAddon(C3Addon addon)
        {
            //compile addon
            AddonCompiler.Insatnce.CompileAddon(addon, false);
            if (AddonCompiler.Insatnce.IsCompilationValid)
            {
                //export c3addon file
                CreateC3AddonFile(addon, addon.AddonFolder);
            }
            else
            {
                AppData.Insatnce.ErrorMessage(
                    "compilation failed, no .c3addon file created, please test your addon to get error information");
            }
        }

        private void CreateC3AddonFile(C3Addon addon, string addonAddonFolder)
        {
            var outputPath = AppData.Insatnce.Options.C3AddonPath;
            var c3addonFile = System.IO.Path.Combine(outputPath,$"{addon.Class.ToLower()}_{addon.Version.Replace(".", "_")}.c3addon");
            if(System.IO.File.Exists(c3addonFile)) File.Delete(c3addonFile);
            ZipFile.CreateFromDirectory(addonAddonFolder, c3addonFile);
        }
    }
}
