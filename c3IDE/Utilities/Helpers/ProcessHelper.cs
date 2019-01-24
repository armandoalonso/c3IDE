using System.Diagnostics;

namespace c3IDE.Utilities.Helpers
{
    public class ProcessHelper : Singleton<ProcessHelper>
    {
        public void WriteFile(string path, string content)
        {
            System.IO.File.WriteAllText(path, content);
        }

        public void StartProcess(string process)
        {
            Process.Start(process);
        }

        public void StartProcess(string process, string args)
        {
            Process.Start(process, args);
        }
    }
}
