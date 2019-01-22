using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace c3IDE.Models
{
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

        public string Id
        {
            get => _id.ToLower();
            set { _id = value.ToLower(); OnPropertyChanged(); InvokePropertyChanged("ScriptName");}
        }

        public string Category
        {
            get => _category;
            set { _category = value.ToLower(); OnPropertyChanged(); }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
