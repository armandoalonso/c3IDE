using System.IO;
using c3IDE.Utilities.Helpers;

namespace c3IDE.Managers
{
    public static class LintingManager
    {
        public static void Lint(string source)
        {
            //start web server if not started
            if (!WebServerManager.WebServerStarted)
            {
                WebServerManager.StartWebServer();
            }

            //generate jslint html files
            var browserjs = ResourceReader.Insatnce.GetResourceText("c3IDE.Server.Test.jslint.browser.js");
            var reportjs = ResourceReader.Insatnce.GetResourceText("c3IDE.Server.Test.jslint.report.js");
            var jslintjs = ResourceReader.Insatnce.GetResourceText("c3IDE.Server.Test.jslint.jslint.js");
            var jslintcss = ResourceReader.Insatnce.GetResourceText("c3IDE.Server.Test.jslint.jslint.css");
            var reporthtml = ResourceReader.Insatnce.GetResourceText("c3IDE.Server.Test.jslint.report.html");
            var functionhtml = ResourceReader.Insatnce.GetResourceText("c3IDE.Server.Test.jslint.function.html");

            var script = source.Replace("\"", "\\\"");
            script = script.Replace("\'", "\\\'");
            browserjs = browserjs.Replace("[@SOURCE@]", script);

            //copy to web server 
            System.IO.File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.CompilePath, "lint", "browser.js"), browserjs);
            System.IO.File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.CompilePath, "lint", "report.js"), reportjs);
            System.IO.File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.CompilePath, "lint", "jslint.js"), jslintjs);
            System.IO.File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.CompilePath, "lint", "jslint.css"), jslintcss);
            System.IO.File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.CompilePath, "lint", "report.html"), reporthtml);
            System.IO.File.WriteAllText(Path.Combine(OptionsManager.CurrentOptions.CompilePath, "lint", "function.html"), functionhtml);

            //open chrome to url
            ProcessHelper.Insatnce.StartProcess("chrome.exe", "http://localhost:8080/lint/report.html");
        }
    }
}
