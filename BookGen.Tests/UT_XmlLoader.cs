using BookGen.AssemblyDocument;
using BookGen.Tests.Environment;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_XmlLoader
    {
        [Test]
        public void EnsureThat_XmlLoader_XmlIsLoadable()
        {
            XmlLoader loader = new XmlLoader();
            bool result = loader.TryValidatedLoad(TestEnvironment.GetFile("BookGen.Tests.Assemblydoc.xml"), out var document);
            Assert.IsTrue(result);
            Assert.IsNotNull(document);

            Assert.AreEqual("BookGen.Tests.Assemblydoc", document.Assembly.name);
            Assert.Greater(document.Members.Items.Length, 0);

        }
    }

}
