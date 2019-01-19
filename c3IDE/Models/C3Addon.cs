using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Media.Imaging;
using c3IDE.Templates;
using c3IDE.Utilities;
using LiteDB;
using Newtonsoft.Json;

namespace c3IDE.Models
{
    public class C3Addon
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Company { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string AddonFolder { get; set; }

        public PluginType Type { get; set; }
        //public string IconBase64 { get; set; }
        public string IconXml { get; set; }

        [BsonIgnore]
        public BitmapImage IconImage => ImageHelper.Insatnce.SvgToBitmapImage(ImageHelper.Insatnce.SvgFromXml(IconXml));
        //public BitmapImage IconImage => ImageHelper.Insatnce.Base64ToBitmap(IconBase64);

        [JsonIgnore]
        public ITemplate Template { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModified { get; set; }
        public string AddonJson { get; set; }
        public string PluginEditTime { get; set; }
        public string PluginRunTime { get; set; }
        public string TypeEditTime { get; set; }
        public string TypeRunTime { get; set; }
        public string InstanceEditTime { get; set; }
        public string InstanceRunTime { get; set; }
        public string LanguageProperties { get; set; }
        public string LanguageCategories { get; set; }
        [BsonIgnore]
        public List<string> Categories  {
            get
            {
                var set = new HashSet<string>();
                set.UnionWith(Actions?.Select(x => x.Value.Category) ?? new List<string>());
                set.UnionWith(Conditions?.Select(x => x.Value.Category) ?? new List<string>());
                set.UnionWith(Expressions?.Select(x => x.Value.Category) ?? new List<string>());
                return set.ToList();
            }
        }
        public Dictionary<string, Action> Actions { get; set; }
        public Dictionary<string, Condition> Conditions { get; set; }
        public Dictionary<string, Expression> Expressions { get; set; }
        public Dictionary<string, ThirdPartyFile> ThirdPartyFiles { get; set; }
    }
}

