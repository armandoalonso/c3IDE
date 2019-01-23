using c3IDE.Utilities.JsBeautifier;

namespace c3IDE.Utilities.Helpers
{
    public class FormatHelper : Singleton<FormatHelper>
    {
        private Beautifier jsBeautifier = new Beautifier();
        private JsonBeautifier jsonBeautifier = new JsonBeautifier();

        public string Json(string json, bool wrap = false)
        {
            if (wrap)
            {
                json = $"{{{json}}}";
                var returnString = jsonBeautifier.GetResults(json).Trim();
                returnString = returnString.Remove(0, 1);
                returnString = returnString.Remove(returnString.Length - 1, 1);
                return returnString.Trim();
            }
            else
            {
                return jsonBeautifier.GetResults(json).Trim();
            }
        }

        public string JsonCompress(string json, bool wrap = false)
        {
            if (wrap)
            {
                json = $"{{{json}}}";
                var returnString = jsonBeautifier.GetResultsCompressed(json).Trim();
                returnString = returnString.Remove(0, 1);
                returnString = returnString.Remove(returnString.Length - 1, 1);
                return returnString.Trim();
            }
            else
            {
                return jsonBeautifier.GetResultsCompressed(json).Trim();
            }
        }

        public string Javascript(string js)
        {
            return jsBeautifier.Beautify(js).Trim();
        }
    }
}
