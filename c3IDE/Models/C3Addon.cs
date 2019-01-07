﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using c3IDE.Templates;
using c3IDE.Utilities;
using LiteDB;

namespace c3IDE.Models
{
    public class C3Addon
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Class { get; set; }

        public string Company { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public PluginType Type { get; set; }

        public string IconBase64 { get; set; }

        [BsonIgnore]
        public BitmapImage IconImage => ImageHelper.Insatnce.Base64ToBitmap(IconBase64);

        public ITemplate Template { get; set; }

        public string AddonJson { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}

