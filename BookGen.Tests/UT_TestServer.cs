using BookGen.Core.Contracts;
using BookGen.Framework.Server;
using BookGen.Tests.Environment;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace BookGen.Tests
{
    [TestFixture, SingleThreaded]
    public class UT_TestServer
    {
        private HttpTestServer _server;
        private Mock<ILog> _log;

        private class TestHandler : IRequestHandler
        {
            public bool CanServe(string AbsoluteUri)
            {
                return AbsoluteUri == "/testme";
            }

            public void Serve(HttpListenerResponse response)
            {
                byte[] msg = Encoding.UTF8.GetBytes("TestHandler");
                response.StatusCode = 200;
                response.ContentType = "text/plain";
                response.OutputStream.Write(msg, 0, msg.Length);
            }
        }

        [SetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
            _server = new HttpTestServer(TestEnvironment.GetTestFolder(), 8080, _log.Object, new TestHandler());
        }

        [TearDown]
        public void Teardown()
        {
            _server.Dispose();
            _server = null;
            _log = null;
        }


        private string DoRequest(string url)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        [Test]
        public void EnsureThat_FileRequest_ReturnsContent()
        {
            var response = DoRequest("http://localhost:8080/Test.js");

            Assert.IsNotNull(response);
            Assert.AreEqual("print(\"hello\");", response);
        }

        [Test]
        public void EnsureThat_VirtualRequest_ReturnsContent()
        {
            var response = DoRequest("http://localhost:8080/testme");

            Assert.IsNotNull(response);
            Assert.AreEqual("TestHandler", response);
        }
    }
}
