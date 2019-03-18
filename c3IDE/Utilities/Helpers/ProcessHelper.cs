using System;
using System.Diagnostics;
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
    }
}
