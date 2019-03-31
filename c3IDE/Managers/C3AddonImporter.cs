using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace c3IDE.Managers
{
    public static class C3AddonImporter
    {
        public static void Import(string path)
        {
            var fi = new FileInfo(path);
            var tmpPath = OptionsManager.CurrentOptions.DataPath + "\\tmp";
            if(Directory.Exists(tmpPath)) Directory.Delete(tmpPath);


            //unzip c3addon to temp location
            ZipFile.ExtractToDirectory(path, tmpPath);

            dynamic addon = JsonConvert.DeserializeObject(File.ReadAllText(Path.Combine(tmpPath, "addon.json")));
        }
    }
}
