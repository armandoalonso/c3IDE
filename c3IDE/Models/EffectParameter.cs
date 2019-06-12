using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    [Serializable]
    public class EffectParameter : INotifyPropertyChanged
    {
        private string _key;
        private string _json;
        private string _lang;
        private string _uniform;

        public string Key
        {
            get => _key;
            set { _key = value; OnPropertyChanged(); }
        }

        public string Json
        {
            get => _json;
            set { _json = value; OnPropertyChanged(); }
        }

        public string Lang
        {
            get => _lang;
            set { _lang = value; OnPropertyChanged(); }
        }

        public string VariableDeclaration
        {
            get => _uniform;
            set{  _uniform = value; OnPropertyChanged(); }
        }

        public string Uniform
        {
            get
            {
                var ti = new CultureInfo("en-US", false).TextInfo;
                var uni = ti.ToTitleCase(_key.Replace("-", " ").ToLower()).Replace(" ", string.Empty);
                return char.ToLowerInvariant(uni[0]) + uni.Substring(1);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
