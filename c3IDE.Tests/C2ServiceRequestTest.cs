using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class C2ServiceRequestTest
    {
        [TestMethod]
        public void C2ServiceRequestHappyPath()
        {
            var text = File.ReadAllText("TestFiles\\rex_gfsm_edittime.js"); 
            var json = C2ParsingService.Insatnce.Execute(text);
        }

        [TestMethod]
        public void C2ServiceParseHappyPath()
        {
            var text = File.ReadAllText("TestFiles\\yannjson_parsed.json");
            var c2 = C2ParsingService.Insatnce.Parse(text);
        }
    }
}
