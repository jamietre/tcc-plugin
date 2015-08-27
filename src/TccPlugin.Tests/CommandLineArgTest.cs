using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TccPlugin.Parser;

namespace TccPlugin.Tests
{
    [TestClass]
    public class CommandLineArgTest
    {
        [TestMethod]
        public void option_flag()
        {
            var sut = new CommandLineArg("/a");
            Assert.IsTrue(sut.IsOption);
            Assert.IsTrue(sut.IsFlag);
            Assert.AreEqual("A", sut.Option);
            Assert.AreEqual(null, sut.Value);
            Assert.AreEqual("/A", sut.ToString());
        }

        [TestMethod]
        public void option_value()
        {
            var sut = new CommandLineArg("/a:foo");
            Assert.IsTrue(sut.IsOption);
            Assert.IsFalse(sut.IsFlag);
            Assert.AreEqual("A", sut.Option);
            Assert.AreEqual("foo", sut.Value);
            Assert.AreEqual("/A:foo", sut.ToString());
        }

        [TestMethod]
        public void option_just_slash()
        {
            var sut = new CommandLineArg("/");
            Assert.IsFalse(sut.IsOption);
            Assert.IsFalse(sut.IsFlag);
            Assert.AreEqual(null, sut.Option);
            Assert.AreEqual("/", sut.Value);
            Assert.AreEqual("/", sut.ToString());
        }

        [TestMethod]
        public void value()
        {
            string arg = "d:\\filePath";
            var sut = new CommandLineArg(arg);
            Assert.IsFalse(sut.IsOption);
            Assert.AreEqual(null, sut.Option);
            Assert.AreEqual(arg, sut.Value);
            Assert.AreEqual(arg, sut.ToString());
        }

    }
}
