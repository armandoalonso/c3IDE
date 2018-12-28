using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    public class TestBase
    {
        private readonly string _saveDirectory = Path.Combine(Path.GetFullPath(@"..\..\"), "Resources");

        private string GetFileResource(string fileName)
        {
            return File.ReadAllText($"Resources/{fileName}");
        }

        private void SaveFileResource(string filename, string text)
        {
            File.WriteAllText(Path.Combine(_saveDirectory, filename), text);
        }

        public void VerifyFile(string filename, string text, bool overwrite = false)
        {
            if (File.Exists(Path.Combine(_saveDirectory, filename)))
            {
                if (overwrite)
                {
                    SaveFileResource(filename, text);
                }
                else
                {
                    var fileText = GetFileResource(filename);
                    Assert.AreEqual(fileText, text);
                }
            }
            else
            {
                SaveFileResource(filename, text);
            }
        }
    }
}
