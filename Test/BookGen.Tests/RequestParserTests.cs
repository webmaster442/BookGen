// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Webmaster442.HttpServerFramework.Domain;
using Webmaster442.HttpServerFramework.Internal;

namespace Webmaster442.HttpServerFramework.Tests
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
            MemoryStream result = new MemoryStream(2048);
            var data = Encoding.UTF8.GetBytes(input);
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
            using (var stream = CreateStream(input))
            {
                var output = _sut.ParseRequest(stream);
                Assert.AreEqual(expected.Url, output.Url);
                Assert.AreEqual(expected.Method, output.Method);
                Assert.AreEqual(expected.Version, output.Version);
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
            using (var stream = CreateStream(input))
            {
                var output = _sut.ParseRequest(stream);
                Assert.AreEqual(expected.Parameters.Count, output.Parameters.Count);
                foreach (var parameter in output.Parameters)
                {
                    Assert.AreEqual(expected.Parameters[parameter.Key], parameter.Value);
                }
            }
        }

        public static IEnumerable<TestCaseData> HeaderTestCases
        {
            get
            {
                yield return new TestCaseData("GET /hello.htm HTTP/1.1", new HttpRequest
                {
                });
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
            using (var stream = CreateStream(input))
            {
                var output = _sut.ParseRequest(stream);
                Assert.AreEqual(expected.Headers.Count, output.Headers.Count);
                foreach (var header in output.Headers)
                {
                    Assert.AreEqual(expected.Headers[header.Key], header.Value);
                }
            }
        }

        [Test]
        public void TestPostRequest()
        {
            string content = "POST /hello.htm HTTP/1.1\n"
                           + "Content-Type: application/x-www-form-urlencoded\n"
                           + "Content-Length: 32\n"
                           + "\n"
                           + "home=Cosby&favorite+flavor=flies";

            using (var stream = CreateStream(content))
            {
                var output = _sut.ParseRequest(stream);
                Assert.AreEqual(32, output.RequestSize);
                Assert.AreEqual(32, output.RequestContent.Length);

                Assert.AreEqual("home=Cosby&favorite+flavor=flies", Encoding.UTF8.GetString(output.RequestContent));
            }
        }
    }
}
