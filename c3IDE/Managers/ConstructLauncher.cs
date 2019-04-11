using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace c3IDE.Managers
{
    public class ConstructLauncher : Singleton<ConstructLauncher>
    {
        private string versionURL = "https://editor.construct.net/versions.json";

        public void UpdateVersions()
        {
            try
            {
                var client = new RestClient(versionURL);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request);
                var json = response.Content;

                var versionInfo = JArray.Parse(json);
                foreach (var info in versionInfo)
                {
                    switch (info["branchName"].ToString())
                    {
                        case "Stable":
                            OptionsManager.CurrentOptions.StableUrl = info["launchURL"].ToString();
                            break;
                        case "Beta":
                            OptionsManager.CurrentOptions.BetaUrl = info["launchURL"].ToString();
                            break;
                    }
                }

                OptionsManager.SaveOptions();
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }
        }

        public void LaunchConstruct(bool safe = false)
        {
            if (OptionsManager.CurrentOptions.OpenConstructInBeta)
            {
                LaunchBeta(safe);
            }
            else
            {
                LaunchStable(safe);
            }
        }

        public void LaunchStable(bool safe = false)
        {
            var safeUrl = safe ? "/?safe-mode" : string.Empty;
            var url = $"{OptionsManager.CurrentOptions.StableUrl.Trim('/')}{safeUrl}";
            ProcessHelper.Insatnce.StartProcess("chrome.exe", url);
        }

        public void LaunchBeta(bool safe = false)
        {
            var safeUrl = safe ? "/?safe-mode" : string.Empty;
            var url = $"{OptionsManager.CurrentOptions.BetaUrl.Trim('/')}{safeUrl}";
            ProcessHelper.Insatnce.StartProcess("chrome.exe", url);
        }
    }
}
