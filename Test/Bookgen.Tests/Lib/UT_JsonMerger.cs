//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Nodes;

using Bookgen.Lib.Confighandling;
using Bookgen.Lib.Domain.IO.Configuration;

namespace Bookgen.Tests.Lib;

[TestFixture]
public class UT_JsonMerger
{
    [Test]
    public void EnsureThat_Merge_Works()
    {
        var config1 = new Config
        {
            Book2LetterISO639Language = "en",
            BookAuthor = "Author One",
            PrintConfig = new PrintConfig
            {
                Images = new ImageConfig
                {
                    ImageQualityOnResize = 70,
                    ResizeAndRecodeImages = ImgRecodeOption.AsPng,
                    SvgRecode = SvgRecodeOption.AsPng,
                }
            },
            WordpressConfig = new WordpressConfig
            {
                CssClasses = new CssClasses
                {
                    H1 = "custom-h1",
                    H2 = "custom-h2",
                }
            },
        };

        var overlay = """
            {
                "BookAuthor": "Author Two",
                "PrintConfig": {
                    "Images": {
                        "ImageQualityOnResize": 85
                    }
                },
                "WordpressConfig": {
                    "CssClasses": {
                        "H2": "overridden-h2",
                        "H3": "custom-h3"
                    }
                }
            }
            """;

        var node1 = JsonSerializer.SerializeToNode(config1) as JsonObject;
        var nodeOverlay = JsonNode.Parse(overlay) as JsonObject;

        JsonMerger sut = new JsonMerger(node1!);
        sut.Merge(nodeOverlay!);

        Config? result = sut.Deserialize<Config>();

        Assert.That(result, Is.Not.Null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result!.Book2LetterISO639Language, Is.EqualTo("en"));
            Assert.That(result.BookAuthor, Is.EqualTo("Author Two"));
            Assert.That(result.PrintConfig.Images.ImageQualityOnResize, Is.EqualTo(85));
            Assert.That(result.PrintConfig.Images.ResizeAndRecodeImages, Is.EqualTo(ImgRecodeOption.AsPng));
            Assert.That(result.PrintConfig.Images.SvgRecode, Is.EqualTo(SvgRecodeOption.AsPng));
            Assert.That(result.WordpressConfig.CssClasses.H1, Is.EqualTo("custom-h1"));
            Assert.That(result.WordpressConfig.CssClasses.H2, Is.EqualTo("overridden-h2"));
            Assert.That(result.WordpressConfig.CssClasses.H3, Is.EqualTo("custom-h3"));
        }
    }
}
