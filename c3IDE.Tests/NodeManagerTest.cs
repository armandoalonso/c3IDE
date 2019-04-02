using System;
using c3IDE.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class NodeManagerTest
    {
        [TestMethod]
        public void NodeManagerTest_GetVersion()
        {
            var App = new App();
            OptionsManager.LoadOptions();
            var version = NodeManager.GetNodeVersion();

        }
    }
}
