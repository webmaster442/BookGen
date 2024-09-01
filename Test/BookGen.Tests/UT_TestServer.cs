//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Http;
using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Domain;
using Webmaster442.HttpServerFramework.Handlers;

namespace BookGen.Tests
{
    [TestFixture, SingleThreaded]
    public class UT_TestServer
    {
        private HttpServer _server;
        private ILogger _log;

        private class TestHandler : IRequestHandler
        {
            private static bool CanServe(string AbsoluteUri)
            {
                return AbsoluteUri == "/testme";
            }

            public async Task<bool> Handle(ILogger log, HttpRequest request, HttpResponse response)
            {
                if (CanServe(request.Url))
                {
                    byte[] msg = Encoding.UTF8.GetBytes("TestHandler");
                    response.ResponseCode = HttpResponseCode.Ok;
                    response.ContentType = "text/plain";
                    await response.WriteAsync(msg);
                    return true;
                }
                return false;
            }
        }

        [SetUp]
        public void Setup()
        {
            _log = Substitute.For<ILog>();
            _server = new HttpServer(new HttpServerConfiguration
            {
                Port = 8080,
                EnableLastAccesTime = false,
            }, _log);

            _server.RegisterHandler(new TestHandler());
            _server.RegisterHandler(new FileServeHandler(TestEnvironment.GetTestFolder(), false, _server.Configuration, "/"));
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

        private static string DoRequest(string url)
        {
            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult())
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        return content;
                    }
                }
            }
            return string.Empty;
        }

        [Test]
        public void EnsureThat_FileRequest_ReturnsContent()
        {
            string response = DoRequest("http://localhost:8080/Test.js");

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.EqualTo("print(\"hello\");"));
        }

        [Test]
        public void EnsureThat_VirtualRequest_ReturnsContent()
        {
            string response = DoRequest("http://localhost:8080/testme");

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.EqualTo("TestHandler"));
        }
    }
}
