// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.IO;
using Webmaster442.HttpServerFramework;
using Webmaster442.HttpServerFramework.Domain;
using Webmaster442.HttpServerFramework.Internal;

namespace BookGen.Tests
{
    public class RequestParserTests
    {
        private RequestParser _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new RequestParser();
        }

        private static MemoryStream CreateStream(string input)
        {
            var result = new MemoryStream(2048);
            byte[] data = Encoding.UTF8.GetBytes(input);
            result.Write(data);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        public static IEnumerable<TestCaseData> BasicTestCases
        {
            get
            {
                yield return new TestCaseData("GET /hello.htm HTTP/1.1", new HttpRequest
                {
                    Method = RequestMethod.Get,
                    Url = "/hello.htm",
                    Version = "HTTP/1.1",
                });
                yield return new TestCaseData("POST /hello.htm HTTP/1.1\r\nContent-Length: 0", new HttpRequest
                {
                    Method = RequestMethod.Post,
                    Url = "/hello.htm",
                    Version = "HTTP/1.1",
                    Headers = new Dictionary<string, string>
                    {
                        { KnownHeaders.ContentLength, "0" },
                    }
                });
            }
        }

        [TestCaseSource(nameof(BasicTestCases))]
        public void TestBasicRequestParsing(string input, HttpRequest expected)
        {
            using (MemoryStream stream = CreateStream(input))
            {
                HttpRequest output = _sut.ParseRequest(stream);
                Assert.Multiple(() =>
                {
                    Assert.That(output.Url, Is.EqualTo(expected.Url));
                    Assert.That(output.Method, Is.EqualTo(expected.Method));
                    Assert.That(output.Version, Is.EqualTo(expected.Version));
                });
            }
        }

        public static IEnumerable<TestCaseData> UrlParameterTestCases
        {
            get
            {
                yield return new TestCaseData("GET /hello.htm HTTP/1.1", new HttpRequest());
                yield return new TestCaseData("GET /hello.htm?foo=bar HTTP/1.1", new HttpRequest
                {
                    Parameters = new Dictionary<string, string>
                    {
                        { "foo", "bar" },
                    }
                });
                yield return new TestCaseData("GET /hello.htm?foo=bar&q=test&asd=agf HTTP/1.1", new HttpRequest
                {
                    Parameters = new Dictionary<string, string>
                    {
                        { "foo", "bar" },
                        { "q", "test" },
                        { "asd", "agf" },
                    }
                });
            }
        }

        [TestCaseSource(nameof(UrlParameterTestCases))]
        public void TestUrlParameterParsing(string input, HttpRequest expected)
        {
            using (MemoryStream stream = CreateStream(input))
            {
                HttpRequest output = _sut.ParseRequest(stream);

                Assert.Multiple(() =>
                {
                    Assert.That(output.Parameters, Has.Count.EqualTo(expected.Parameters.Count));
                    foreach (KeyValuePair<string, string> parameter in output.Parameters)
                    {
                        Assert.That(parameter.Value, Is.EqualTo(expected.Parameters[parameter.Key]));
                    }
                });
            }
        }

        public static IEnumerable<TestCaseData> HeaderTestCases
        {
            get
            {
                yield return new TestCaseData("GET /hello.htm HTTP/1.1", new HttpRequest());
                yield return new TestCaseData("GET /hello.htm?foo=bar HTTP/1.1\n"
                                             + "User-Agent: Mozilla/4.0 (compatible; MSIE5.01; Windows NT)\n"
                                             + "Host: www.test.com",
                new HttpRequest
                {
                    Headers = new Dictionary<string, string>
                    {
                        { "User-Agent", "Mozilla/4.0 (compatible; MSIE5.01; Windows NT)" },
                        { "Host", "www.test.com" }
                    }
                });
            }
        }

        [TestCaseSource(nameof(HeaderTestCases))]
        public void TestHeaderParsing(string input, HttpRequest expected)
        {
            using (MemoryStream stream = CreateStream(input))
            {
                HttpRequest output = _sut.ParseRequest(stream);

                Assert.Multiple(() =>
                {
                    Assert.That(output.Headers, Has.Count.EqualTo(expected.Headers.Count));
                    foreach (KeyValuePair<string, string> header in output.Headers)
                    {
                        Assert.That(header.Value, Is.EqualTo(expected.Headers[header.Key]));
                    }
                });
            }
        }

        [Test]
        public void TestPostRequest()
        {
            const string content = "POST /hello.htm HTTP/1.1\n"
                           + "Content-Type: application/x-www-form-urlencoded\n"
                           + "Content-Length: 32\n"
                           + "\n"
                           + "home=Cosby&favorite+flavor=flies";

            using (MemoryStream stream = CreateStream(content))
            {
                HttpRequest output = _sut.ParseRequest(stream);
                Assert.Multiple(() =>
                {
                    Assert.That(output.RequestSize, Is.EqualTo(32));
                    Assert.That(output.RequestContent, Has.Length.EqualTo(32));
                    Assert.That(Encoding.UTF8.GetString(output.RequestContent), Is.EqualTo("home=Cosby&favorite+flavor=flies"));
                });
            }
        }
    }
}
