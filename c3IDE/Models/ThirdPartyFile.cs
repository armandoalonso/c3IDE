using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace c3IDE.Models
{
    public class ThirdPartyFile : INotifyPropertyChanged
    {
        private string _fileName;
        private string _content;
        private string _pluginTemplate;
        private byte[] _bytes;
        private string _extention;

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        public string PluginTemplate
        {
            get => _pluginTemplate;
            set { _pluginTemplate = value; OnPropertyChanged(); }
        }

        public byte[] Bytes
        {
            get => _bytes;
            set { _bytes = value; OnPropertyChanged(); }
        }

        public string Extention
        {
            get => _extention;
            set { _extention = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
