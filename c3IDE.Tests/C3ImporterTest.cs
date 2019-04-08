using System;
using c3IDE.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class C3ImporterTest
    {
        [TestMethod]
        public void SimpleC3AddonImportTest()
        {
            var App = new App();
            OptionsManager.LoadOptions();
            C3AddonImporter.Insatnce.Import("TestFiles\\rex_moveto.c3addon");
        }

        [TestMethod]
        public void SimpleC3AddonImportEffectTest()
        {
            var App = new App();
            OptionsManager.LoadOptions();
            var x = C3AddonImporter.Insatnce.Import("TestFiles\\reflecty(effect).c3addon").Result;
        }
    }
}
