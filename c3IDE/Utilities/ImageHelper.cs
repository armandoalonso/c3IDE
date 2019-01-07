using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public BitmapImage Base64ToBitmap(string base64)
        {
            var img = Base64ToImage(base64);
            var bmp = new Bitmap(img);

            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public string BitmapImageToBase64(BitmapImage imageC)
        {
            var memStream = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return Convert.ToBase64String(memStream.ToArray());
        }
    }
}
