using System;
using System.Linq;
using c3IDE.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class ResourceReaderTest
    {

        [DataTestMethod]
        [DataRow("SingleGlobal", 12)]
        public void VerifyResourceReaderLogResourceFiles(string pluginType, int expected)
        {
            var resources = ResourceReader.Insatnce.LogResourceFiles();
            var count = resources.Count(x => x.Contains(pluginType));
            Assert.AreEqual(count, expected);
        }

        [DataTestMethod]
        [DataRow("SingleGlobal")]
        public void VerifyResourceReaderGetResourceText(string pluginType)
        {
            var resources = ResourceReader.Insatnce.LogResourceFiles().Where(x => x.Contains(pluginType));
            foreach (var resource in resources)
            {
                var data = ResourceReader.Insatnce.GetResourceText(resource);
                Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            }
        }

        [TestMethod]
        public void VerifyResourceReaderGetResourceAsBase64()
        {
            var data = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Templates.TemplateFiles.icon.png");
            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
        }
    }
}
