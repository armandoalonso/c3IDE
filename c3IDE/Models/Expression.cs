using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;


namespace c3IDE.Models
{
    public class Expression : INotifyPropertyChanged
    {
        private string _id;
        private string _category;
        private string _translatedname;
        private string _returntype;
        private string _ace;
        private string _language;
        private string _code;
        private string _desc;
        private string _isvaradic;

        public string Id
        {
            get => _id.ToLower();
            set
            {
                _id = value.ToLower();
                OnPropertyChanged();
            }
        }

        public string Category
        {
            get => _category;
            set { _category = string.IsNullOrWhiteSpace(value) ? "" : value.ToLower(); OnPropertyChanged(); }
        }

        public string ScriptName => TranslatedName;

        public string IsVariadicParameters
        {
            get => _isvaradic;
            set
            {
                _isvaradic = value?.ToLower();
                OnPropertyChanged();
            }
        }

        public string Ace
        {
            get => _ace;
            set
            {
                _ace = value;
                OnPropertyChanged();
            }
        }

        public string TranslatedName
        {
            get => _translatedname;
            set
            {
                _translatedname = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _desc;
            set
            {
                _desc = value;
                OnPropertyChanged();
            }
        }

        public string ReturnType
        {
            get => _returntype;
            set
            {
                _returntype = value?.ToLower();
                OnPropertyChanged();
            }
        }

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged();
            }
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

        public Expression Copy(string newId)
        {
            Expression ace = (Expression)this.MemberwiseClone();
            var oldId = ace.Id;
            var oldScript = ace.TranslatedName;
            ace.Id = newId;

            var ti = new CultureInfo("en-US", false).TextInfo;
            ace.TranslatedName = ti.ToTitleCase(newId.Replace("-", " ").ToLower()).Replace(" ", string.Empty);

            ace.Ace = ace.Ace.Replace(oldId, newId);
            ace.Ace = ace.Ace.Replace(oldScript, ace.TranslatedName);
            ace.Language = ace.Language.Replace(oldId, newId);
            ace.Language = ace.Language.Replace(oldScript, ace.TranslatedName);
            ace.Code = ace.Code.Replace(oldScript, ace.ScriptName);
            return ace;
        }
    }
}