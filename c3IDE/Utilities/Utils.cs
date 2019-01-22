using System.Diagnostics;

namespace c3IDE.Utilities
{
    public class Utils : Singleton<Utils>
    {
        public string Base64Encode(string text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

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
