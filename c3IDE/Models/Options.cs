using System;
using c3IDE.Utilities.ThemeEngine;
using LiteDB;

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
        public string DefaultAuthor { get; set; }
        public string ThemeKey { get; set; }
        public bool IncludeTimeStampOnExport { get; set; } = true;
        public bool OpenC3InWeb { get; set; } = true;
        public string C3DesktopPath { get; set; }
        public bool PinMenu { get; set; } = false;
        public bool CompileOnSave { get; set; }
        public bool ExportSingleFileProject { get; set; } = true;
        public bool OverwriteGuidOnImport { get; set; } = true;

        [BsonIgnore]
        public Theme ApplicationTheme => ThemeResolver.Insatnce.Resolve(ThemeKey);



        //public string HighlightKey { get; set; }

        //[JsonIgnore]
        //public IHighlightingDefinition HighlightingDefinition => SyntaxHighligtResolver.Insatnce.Resolve(HighlightKey);

        // [JsonIgnore]
        // public SolidBrush BackgroundColor => 
    }
}
