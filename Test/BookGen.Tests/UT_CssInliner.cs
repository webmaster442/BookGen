//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Css;
using System.Xml.Linq;

namespace BookGen.Tests
{
    [TestFixture]
    internal class UT_CssInliner
    {
        [Test]
        public void InlineTest()
        {
            var html = XElement.Parse(@"<html>
                                        <head>
                                            <title>Hello, World Page!</title>
                                            <style>
                                                .redClass { 
                                                    background: red; 
                                                    color: purple; 
                                                }
                                            </style>
                                        </head>
                                        <body>
                                            <div class=""redClass"">Hello, World!</div>
                                        </body>
                                    </html>").ToString();

            var expected = XElement.Parse(@"<html>
                                            <head>
                                                <title>Hello, World Page!</title>
                                            </head>
                                            <body>
                                                <div class=""redClass"" style=""background: red; color: purple;"">Hello, World!</div>
                                            </body>
                                        </html>").ToString();

            var result = CssInliner.Inline(html);

            Assert.AreEqual(expected, result);
        }

    }
}
