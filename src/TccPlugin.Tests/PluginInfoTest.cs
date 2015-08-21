using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TccPlugin.Tests
{
    [TestClass]
    public class PluginInfoTest
    {
        [TestMethod]
        public void ReflectedFunctionList()
        {
            var pi = new PluginInfo();
            CollectionAssert.AreEquivalent("UNKNOWN_CMD,CD,DIR,@testfunc,_testvar,*key".Split(','),
                pi.Functions.Split(','));
        }
    }
}
