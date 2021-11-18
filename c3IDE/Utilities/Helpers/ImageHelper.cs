using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;
using Svg;
using Image = System.Drawing.Image;

namespace c3IDE.Utilities.Helpers
{
    public class ImageHelper : Singleton<ImageHelper>
    {
        public static Dictionary<string, BitmapImage> IconCache = new Dictionary<string, BitmapImage>();
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
            var ms = new MemoryStream(Convert.FromBase64String(base64));
            var img = Image.FromStream(ms);
            ms.Dispose();
            return img;
        }

        public BitmapImage Base64ToBitmap(string base64)
        {
            var ms = new MemoryStream();
            try
            {
                var img = Base64ToImage(base64);
                var bmp = new Bitmap(img);

                bmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                ms.Dispose();
                return bitmapImage;
            }
            catch (Exception e)
            {
                ms.Dispose();
                Console.WriteLine(e);
                return new BitmapImage();
            }
        }

        public string BitmapImageToBase64(BitmapImage imageC)
        {
            var memStream = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            var ret = Convert.ToBase64String(memStream.ToArray());
            memStream.Dispose();
            return ret;
        }

        public byte[] BitmapImageToBytes(BitmapImage image)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                var data = ms.ToArray();
                return data;
            }
        }

        public SvgDocument SvgFromFile(string path)
        {
             var svgDocument = SvgDocument.Open(path);
            return svgDocument;           
        }

        public string SvgToBase64(SvgDocument svg)
        {
            using (var stream = new MemoryStream())
            {
                svg.Write(stream);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public SvgDocument SvgFromXml(string xml)
        {
            var xmlStream = new MemoryStream(Encoding.ASCII.GetBytes(xml));
            try
            {
                xmlStream.Position = 0;
                var svgDoc = SvgDocument.Open<SvgDocument>(xmlStream);
                xmlStream.Dispose();
                return svgDoc;
            }
            catch (Exception e)
            {
                xmlStream.Dispose();
                Console.WriteLine(e);
                return new SvgDocument();
            }
        }

        public SvgDocument Base64ToSvg(string base64)
        {
            var xmlDoc = new XmlDocument();
            var ms = new MemoryStream(Convert.FromBase64String(base64));
            xmlDoc.Load(ms);
            var svg = new SvgDocument();
            SvgDocument.Open(xmlDoc);
            ms.Dispose();
            return svg;
        }

        public BitmapImage XmlToBitmapImage(string xml)
        {
            if (IconCache.ContainsKey(xml)) return IconCache[xml];
            var bmp = SvgToBitmapImage(SvgFromXml(xml));
            IconCache[xml] = bmp;
            return bmp;
        }

        public BitmapImage SvgToBitmapImage(SvgDocument svg)
        {
            //var bmp = svg.Draw(150,150);
            var ms = new MemoryStream();
            try
            {
                var bmp = svg.Draw();

                bmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                ms.Dispose();
                return bitmapImage;
            }
            catch (Exception e)
            {
                ms.Dispose();
                Console.WriteLine(e);
                return new BitmapImage();
            }
        }
    }
}
