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
        public async Task<C3Addon> Import2Addon(string path)
        {
            WindowManager.ShowLoadingOverlay(true);

            try
            {
                C2Addon c2addon = null;
                C3Addon c3addon = null;

                await Task.Run(() =>
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

                        //try to parse file using https://github.com/WebCreationClub/construct-addon-parser
                        try
                        {
                            if (!OptionsManager.CurrentOptions.UseC2ParserService) throw new Exception("C2 parsing service is no enabled");
                            c2addon = C2ParsingService.Insatnce.Execute(source);
                        }
                        //if online parse fails fallback to walkign ast tree
                        catch (Exception ex)
                        {
                            LogManager.AddErrorLog(ex);
                            LogManager.AddImportLogMessage($"C2 Pasring Service Failed => {ex.Message} \nFalling back to Walking Edittime.js AST");

                            c2addon = C2AddonParser.Insatnce.ReadEdittimeJs(source);
                        }

                        LogManager.AddImportLogMessage($"C2 Parsed Model => \n\n\n {JsonConvert.SerializeObject(c2addon, Formatting.Indented)}");
                        c3addon = C2AddonConverter.Insatnce.ConvertToC3(c2addon);
                        var runtime = !string.IsNullOrWhiteSpace(runtimefile) ? File.ReadAllText(runtimefile) : string.Empty;
                        c3addon.C2RunTime = runtime;
                    }
                });

                if (c3addon != null) return c3addon;

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
                WindowManager.ShowLoadingOverlay(false);
            }
        }
    }
}
