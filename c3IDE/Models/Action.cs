using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;


namespace c3IDE.Models
{
    [Serializable]
    public class Action : INotifyPropertyChanged
    {
        private string _id;
        private string _category;
        private string _ace;
        private string _language;
        private string _code;
        private string _highlight;
        private string _displaytext;
        private string _desc;
        private string _listname;
        private string _async;

        public string Id
        {
            get => _id.ToLower();
            set { _id = value.ToLower(); OnPropertyChanged(); InvokePropertyChanged("ScriptName");}
        }

        public string Category
        {
            get => _category;
            set { _category = string.IsNullOrWhiteSpace(value) ? "" : value.ToLower(); OnPropertyChanged(); }
        }

        public string ScriptName
        {
            get
            {
                var ti = new CultureInfo("en-US", false).TextInfo;
                return ti.ToTitleCase(_id.Replace("-", " ").ToLower()).Replace(" ", string.Empty);
            }
        }
    
        public string Ace
        {
            get => _ace;
            set { _ace = value; OnPropertyChanged(); }
        }

        public string DisplayText
        {
            get => _displaytext;
            set { _displaytext = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _desc;
            set { _desc = value; OnPropertyChanged(); }
        }

        public string Highlight
        {
            get => _highlight;
            set { _highlight = value?.ToLower(); OnPropertyChanged(); }
        }

        public string Language
        {
            get => _language;
            set { _language = value; OnPropertyChanged(); }
        }

        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(); }
        }

        public string ListName
        {
            get => _listname;
            set { _listname = value; OnPropertyChanged(); }
        }

        public string Async
        {
            get => _async;
            set { _async = value; OnPropertyChanged(); }
        }

        //only used during import of c2addon
        public string C2Id { get; set; } = string.Empty;

        public string Deprecated { get; set; } = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Action Copy(string newId)
        {
            var ace = (Action) this.MemberwiseClone();
            var oldId = ace.Id;
            var oldScript = ace.ScriptName;
            ace.Id = newId;
            ace.Ace = ace.Ace.Replace(oldId, newId);
            ace.Ace = ace.Ace.Replace(oldScript, ace.ScriptName);
            ace.Language = ace.Language.Replace(oldId, newId);
            ace.Code = ace.Code.Replace(oldScript, ace.ScriptName);
            return ace;
        }    
    }
}
