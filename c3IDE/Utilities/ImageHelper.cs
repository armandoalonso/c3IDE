using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Utilities
{
    public class ImageHelper : Singleton<ImageHelper>
    {
        public string ImageToBase64(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                var imgArray = ms.ToArray();
                return Convert.ToBase64String(imgArray);
            }
        }

        public string ImageToBase64(string imgPath)
        {
            var img = Image.FromFile(imgPath);
            return ImageToBase64(img);
        }

        public Image Base64ToImage(string base64)
        {
            var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64)));
            return img;
        }
    }
}
