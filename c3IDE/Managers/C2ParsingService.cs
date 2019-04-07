using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;
using RestSharp;
using RestSharp.Extensions;


namespace c3IDE.Managers
{
    public class C2ParsingService : Singleton<C2ParsingService>
    {
        public C2Addon Execute(string edittime)
        {
            try
            {
                var client = new RestClient("https://addon-parser-armaldio.webcreationclub.now.sh/parse/c2");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "multipart/form-data");
                request.AddFile("file", Encoding.ASCII.GetBytes(edittime), "edittime.js", "application/javascript");
                IRestResponse response = client.Execute(request); 
                return Parse(response.Content);
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"c2 parsing service failed => {ex.Message}");
                return null;
            }
        }

        public C2Addon Parse(string json)
        {
            var c2addon = new C2Addon();

            //todo: do some parsing

            return c2addon;
        }
    }
}
