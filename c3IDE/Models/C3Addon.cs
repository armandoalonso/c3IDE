using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Media.Imaging;
using c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using LiteDB;
using Newtonsoft.Json;

namespace c3IDE.Models
{
    public class C3Addon : IEquatable<C3Addon>
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

        [BsonIgnore]
        public string TypeName
        {
            get
            {
                switch (Type)
                {
                    case PluginType.SingleGlobalPlugin:
                    case PluginType.DrawingPlugin:
                        return "(Plugin)";
                    case PluginType.Behavior:
                        return "(Behavior)";
                    case PluginType.Effect:
                        return "(Effect)";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
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

        //effect property
        public string EffectLanguage { get; set; }
        public string EffectCode { get; set; }

        [BsonIgnore]
        public List<string> Categories  {
            get
            {
                var set = new HashSet<string>();
                set.UnionWith(Actions?.Select(x =>   x.Value.Category) ?? new List<string>());
                set.UnionWith(Conditions?.Select(x => x.Value.Category) ?? new List<string>());
                set.UnionWith(Expressions?.Select(x => x.Value.Category) ?? new List<string>());
                return set.ToList();
            }
        }
        public Dictionary<string, Action> Actions { get; set; }
        public Dictionary<string, Condition> Conditions { get; set; }
        public Dictionary<string, Expression> Expressions { get; set; }
        public Dictionary<string, ThirdPartyFile> ThirdPartyFiles { get; set; }

        public bool Equals(C3Addon other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((C3Addon)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

