using System;
using System.Diagnostics;
using System.IO;
using c3IDE.Managers;

namespace c3IDE.Utilities.Helpers
{
    public class ProcessHelper : Singleton<ProcessHelper>
    {
        public void WriteFile(string path, string content)
        {
            try
            {
                System.IO.File.WriteAllText(path, content);
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification(ex.Message);
            }
        }

        public void StartProcess(string process)
        {
            try
            {
                Process.Start(process);
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification(ex.Message);
            }
        }

        public void StartProcess(string process, string args)
        {
            try
            {
                Process.Start(process, args);
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification(ex.Message);
            }
        }

        public string ExecuteProcess(string batchCommand)
        {
            File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.DataPath, "execute.bat"), batchCommand);

            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = Path.Combine(OptionsManager.CurrentOptions.DataPath, "execute.bat")
                }
            };
            p.Start();

            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }
}
