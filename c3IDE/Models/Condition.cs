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
    public class Condition: INotifyPropertyChanged
    {
        private string _id;
        private string _category;
        private string _ace;
        private string _language;
        private string _code;
        private string _highlight;
        private string _listname;


        private string _trigger;
        private string _faketrigger;
        private string _static;
        private string _looping;
        private string _invertible;
        private string _triggercompatible;

        private string _displaytext;
        private string _desc;

        public string Id
        {
            get => _id.ToLower();
            set { _id = value.ToLower(); OnPropertyChanged(); InvokePropertyChanged("ScriptName"); }
        }

        public string Category
        {
            get => _category;
            set { _category = value.ToLower(); OnPropertyChanged(); }
        }

        public string ListName
        {
            get => _listname;
            set { _listname = value; OnPropertyChanged(); }
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
            set { _highlight = value.ToLower(); OnPropertyChanged(); }
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

        public string Trigger
        {
            get => _trigger;
            set { _trigger = value.ToLower(); OnPropertyChanged(); }
        }

        public string FakeTrigger
        {
            get => _faketrigger;
            set { _faketrigger = value.ToLower(); OnPropertyChanged(); }
        }

        public string Static
        {
            get => _static;
            set { _static = value.ToLower(); OnPropertyChanged(); }
        }

        public string Looping
        {
            get => _looping;
            set { _looping = value.ToLower(); OnPropertyChanged(); }
        }

        public string Invertible
        {
            get => _invertible;
            set { _invertible = value.ToLower(); OnPropertyChanged(); }
        }

        public string TriggerCompatible
        {
            get => _triggercompatible;
            set { _triggercompatible = value.ToLower(); OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Condition Copy(string newId)
        {
            Condition ace = (Condition)this.MemberwiseClone();
            var oldId = ace.Id;
            var oldScript = ace.ScriptName;
            ace.Id = newId;
            ace.Ace = ace.Ace.Replace(oldId, newId);
            ace.Language = ace.Language.Replace(oldId, newId);
            ace.Code = ace.Code.Replace(oldScript, ace.ScriptName);
            return ace;
        }
    }
}
