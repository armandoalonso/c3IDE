using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Utilities.ThemeEngine;
using ICSharpCode.AvalonEdit.Highlighting;
using LiteDB;
using Newtonsoft.Json;

namespace c3IDE.Models
{
    public class Options
    {
        [BsonId]
        public Guid Key { get; set; } = Guid.Parse("e0cddcac-e99d-4338-ac91-b56b0db58ed0");
        public string DataPath { get; set; }
        public string CompilePath { get; set; }
        public string ExportPath { get; set; }
        public string C3AddonPath { get; set; }
        public double FontSize { get; set; }
        public string FontFamily { get; set; }
        public string DefaultCompany { get; set; }
        public string DefaultAuthor { get; set; }
        public string ThemeKey { get; set; }

        [BsonIgnore]
        public Theme ApplicationTheme => ThemeResolver.Insatnce.Resolve(ThemeKey);

        //public string HighlightKey { get; set; }

        //[JsonIgnore]
        //public IHighlightingDefinition HighlightingDefinition => SyntaxHighligtResolver.Insatnce.Resolve(HighlightKey);

        // [JsonIgnore]
        // public SolidBrush BackgroundColor => 
    }
}
