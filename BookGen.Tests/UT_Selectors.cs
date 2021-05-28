using BookGen.AssemblyDocument;
using BookGen.AssemblyDocument.XmlDoc;
using BookGen.Tests.Assemblydoc;
using BookGen.Tests.Environment;
using NUnit.Framework;
using System;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_Selectors
    {
        private Doc _document;
        private bool _result;

        [OneTimeSetUp]
        public void Setup()
        {
            XmlLoader loader = new XmlLoader();
            _result = loader.TryValidatedLoad(TestEnvironment.GetFile("BookGen.Tests.Assemblydoc.xml"), out _document);
        }

        [Test]
        public void EnsureThat_XmlLoader_LoadedCorrectly()
        {

            Assert.IsTrue(_result);
            Assert.IsNotNull(_document);

            Assert.AreEqual("BookGen.Tests.Assemblydoc", _document.Assembly.name);
            Assert.Greater(_document.Members.Items.Length, 0);
        }


        [Test]
        public void EnsureThat_Selectors_GetTypeSummary_ReturnsCorrect()
        {
            var result = _document.GetTypeSummary(typeof(TestBaseA));
            Assert.AreEqual("Base class A\nSecond line", result);
        }

        [TestCase(typeof(TestBaseA), "")]
        [TestCase(typeof(TestBaseB), "Remarks")]
        public void EnsureThat_Selectors_GetTypeRemarks_ReturnsCorrect(Type t, string expected)
        {
            var result = _document.GetTypeRemarks(t);
            Assert.AreEqual(expected, result);
        }

        [TestCase("GetProperty", "A gettable property")]
        [TestCase("GetProperty2", "An other gettable property")]
        public void EnsureThat_Selectors_GetPropertySelectorName_ReturnsCorrect(string propName, string expected)
        {
            var prop = typeof(TestClass).GetProperty(propName,
                                         System.Reflection.BindingFlags.Public
                                         | System.Reflection.BindingFlags.NonPublic
                                         | System.Reflection.BindingFlags.Instance);


            var result = _document.GetPropertySummary(prop);
            Assert.AreEqual(expected, result);
        }
    }
}
