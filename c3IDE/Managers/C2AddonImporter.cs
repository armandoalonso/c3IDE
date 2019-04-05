using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;
using Newtonsoft.Json;

namespace c3IDE.Managers
{
    public class C2AddonImporter : Singleton<C2AddonImporter>
    {
        public C3Addon Import2Addon(string path)
        {
            try
            {
                var tmpPath = OptionsManager.CurrentOptions.DataPath + "\\tmp_c2";
                if (Directory.Exists(tmpPath)) Directory.Delete(tmpPath, true);

                //unzip c3addon to temp location
                ZipFile.ExtractToDirectory(path, tmpPath);

                var edittimefile = Directory.GetFiles(tmpPath, "edittime.js", SearchOption.AllDirectories).FirstOrDefault();
                var runtimefile = Directory.GetFiles(tmpPath, "runtime.js", SearchOption.AllDirectories).FirstOrDefault();
                LogManager.AddImportLogMessage($"edittime.js => {edittimefile}");


                if (edittimefile != null)
                {
                    var source = File.ReadAllText(edittimefile);
                    var c2addon = C2AddonParser.Insatnce.ReadEdittimeJs(source);
                    LogManager.AddImportLogMessage($"C2 Parsed Model => \n\n\n {JsonConvert.SerializeObject(c2addon, Formatting.Indented)}");
                    var c3addon = C2AddonConverter.Insatnce.ConvertToC3(c2addon);

                    var runtime = !string.IsNullOrWhiteSpace(runtimefile) ? File.ReadAllText(runtimefile) : string.Empty;
                    c3addon.C2RunTime = runtime;
                    return c3addon;
                }

                LogManager.AddImportLogMessage($"ERROR => edittime.js not found => null");
                throw new Exception("edittime.js not found");
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                LogManager.AddImportLogMessage($"ERROR => \n{ex.Message}");
                LogManager.AddImportLogMessage($"STACKTRACE => \n{ex.StackTrace}");
                return null;
            }
            finally
            {
                var logData = string.Join(Environment.NewLine, LogManager.ImportLog);
                File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.DataPath, "import.log"), logData);
                LogManager.ImportLog.Clear();
            }
        }
    }
}
