using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Web;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using LiteDB;
using Newtonsoft.Json;

namespace c3IDE.Models
{
    public class ThirdPartyFile : INotifyPropertyChanged
    {
        private string _fileName;
        private string _content;
        private string _pluginTemplate;
        private byte[] _bytes;
        private string _extention;
        private string _fileType;
        private bool _compress;
        private bool _c3folder, _c2folder, _rootfolder;

        public string FileName
        {
            get => $"{_fileName}";
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
            set
            {
                _pluginTemplate = value;
                OnPropertyChanged();
            }
        }

        public byte[] Bytes
        {
            get => _bytes;
            set
            {
                _bytes = value;
                OnPropertyChanged();
            }
        }

        public string Extention
        {
            get => _extention;
            set
            {
                _extention = value;
                OnPropertyChanged();
            }
        }

        public bool C3Folder
        {
            get => _c3folder;
            set
            {
                _c3folder = value;
                OnPropertyChanged();
            }
        }

        public bool C2Folder
        {
            get => _c2folder;
            set
            {
                _c2folder = value;
                OnPropertyChanged();
            }
        }

        public bool Rootfolder
        {
            get => _rootfolder;
            set
            {
                _rootfolder = value;
                OnPropertyChanged();
            }
        }

        public string MimeType => MimeMapping.GetMimeMapping(FileName);

        public string FileType
        {
            get => _fileType;
            set { _fileType = value; OnPropertyChanged(); }
        }

        public bool Compress { get => _compress; set { _compress = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
