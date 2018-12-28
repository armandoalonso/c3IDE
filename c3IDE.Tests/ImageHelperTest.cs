using System;
using System.Drawing;
using c3IDE.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class ImageHelperTest
    {
        private string sampleBase64 =
        @"iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAIAAABLbSncAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwwAADsMBx2+oZAAAACRJREFUGFdjwA0WvajHAEobfaASb2VU4Ahdwt7jDBDRUWKjDwAEEksegWempAAAAABJRU5ErkJggg==";

        [TestMethod]
        public void VerifyImageHelperImageToBase64()
        {
            var base64 = ImageHelper.Insatnce.ImageToBase64("Resources/test_img.png");
            Assert.AreEqual(base64, sampleBase64);
        }


        [TestMethod]
        public void VerifyImageBase64ToImage()
        {
            var img = ImageHelper.Insatnce.Base64ToImage(sampleBase64);
            var newBase64 = ImageHelper.Insatnce.ImageToBase64(img);
            Assert.AreEqual(newBase64, sampleBase64);
        }
    }
}
