using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Utilities.Search
{
    public class SearchResult : INotifyPropertyChanged
    {
        private bool _selected;
        private string _document;
        private string _line;
        private int _lineNumber;

        private const string RegexExperession = @"[ ]{2,}|\t";

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged(); }
        }

        public string Document
        {
            get { return _document; }
            set { _document = value; OnPropertyChanged();}
        }

        public string Line
        {
            get { return _line; }
            set { _line = value; OnPropertyChanged();}
        }

        public string StrippedText { get; }

        public int LineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; OnPropertyChanged();}
        }

        public IWindow Window { get; }

        public SearchResult(string doc, string text, int line, IWindow window)
        {
            Document = doc;
            Line = text;
            LineNumber = line;
            StrippedText = Regex.Replace(Line, RegexExperession, string.Empty);
            Window = window;
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
