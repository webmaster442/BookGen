//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Tests.Environment;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Domain;
using Webmaster442.HttpServerFramework.Handlers;

namespace BookGen.Tests
{
    [TestFixture, SingleThreaded]
    public class UT_TestServer
    {
        private HttpServer _server;
        private Mock<IServerLog> _log;

        private class TestHandler : IRequestHandler
        {
            private bool CanServe(string AbsoluteUri)
            {
                return AbsoluteUri == "/testme";
            }

            public async Task<bool> Handle(IServerLog log, HttpRequest request, HttpResponse response)
            {
                if (CanServe(request.Url))
                {
                    byte[] msg = Encoding.UTF8.GetBytes("TestHandler");
                    response.ResponseCode = HttpResponseCode.Ok;
                    response.ContentType = "text/plain";
                    await response.Write(msg);
                    return true;
                }
                return false;
            }
        }

        [SetUp]
        public void Setup()
        {
            _log = new Mock<IServerLog>();
            _server = new HttpServer(new HttpServerConfiguration
            {
                Port = 8080,
            }, _log.Object);

            _server.RegisterHandler(new TestHandler());
            _server.RegisterHandler(new FileServeHandler(TestEnvironment.GetTestFolder(), "/"));
            _server.Start();
        }

        [TearDown]
        public void Teardown()
        {
            _server.Stop();
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
