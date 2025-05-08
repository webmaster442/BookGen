using BookGen.DomainServices.Markdown.TableOfContents;

namespace BookGen.Tests;

[TestFixture]
public class UT_LevelList
{
    private class TestLevelList : LevelList<TestLevelList>
    {
        public string Content { get; set; }

        public string ToFormatedString(string prefix = "", StringBuilder sb = null)
        {
            if (sb == null)
                sb = new StringBuilder();
            if (!IsLocator)
            {
                sb.Append(prefix);
                sb.Append(Content);
                sb.AppendLine();
            }
            prefix += "  ";
            foreach (var item in Children)
            {
                item.ToFormatedString(prefix, sb);
            }
            return sb.ToString();
        }
    }

    private TestLevelList _root;

    [SetUp]
    public void Setup()
    {
        _root = new TestLevelList 
        { 
            IsLocator = true,
            Content = "Root" 
        };
    }

    [Test]
    public void TestNormalList()
    {
        _root.Clear();

        _root.Append(new TestLevelList { Level = 1, Content = "# t1" });
        _root.Append(new TestLevelList { Level = 2, Content = "## t1.1" });
        _root.Append(new TestLevelList { Level = 2, Content = "## t1.2" });
        _root.Append(new TestLevelList { Level = 3, Content = "### t1.2.1" });
        _root.Append(new TestLevelList { Level = 2, Content = "## t1.3" });
        _root.Append(new TestLevelList { Level = 1, Content = "# t2" });
        _root.Append(new TestLevelList { Level = 2, Content = "## t2.1" });
        _root.Append(new TestLevelList { Level = 1, Content = "# t3" });
        _root.Append(new TestLevelList { Level = 1, Content = "# t4" });

        Assert.Multiple(() =>
        {
            Assert.That(_root.Count, Is.EqualTo(4));
            Assert.That(_root[0].Count, Is.EqualTo(3));
            Assert.That(_root[0][0].Count, Is.EqualTo(0));
            Assert.That(_root[0][1].Count, Is.EqualTo(1));
            Assert.That(_root[0].Content, Is.EqualTo("# t1"));
            Assert.That(_root[0][0].Content, Is.EqualTo("## t1.1"));
            Assert.That(_root[0][1].Content, Is.EqualTo("## t1.2"));
            Assert.That(_root[0][1][0].Content, Is.EqualTo("### t1.2.1"));
            Assert.That(_root[0][2].Content, Is.EqualTo("## t1.3"));
            Assert.That(_root[1].Content, Is.EqualTo("# t2"));
            Assert.That(_root[1][0].Content, Is.EqualTo("## t2.1"));
            Assert.That(_root[2].Content, Is.EqualTo("# t3"));
            Assert.That(_root[3].Content, Is.EqualTo("# t4"));
        });

    }


    [Test]
    public void TestEmtpyNode()
    {
        _root.Clear();

        _root.Append(new TestLevelList { Level = 2, Content = "## t0.1" });
        _root.Append(new TestLevelList { Level = 2, Content = "## t0.2" });
        _root.Append(new TestLevelList { Level = 2, Content = "## t0.3" });
        _root.Append(new TestLevelList { Level = 1, Content = "# t1" });
        _root.Append(new TestLevelList { Level = 1, Content = "# t2" });
        _root.Append(new TestLevelList { Level = 1, Content = "# t3" });

        Assert.Multiple(() =>
        {
            Assert.That(_root.Count, Is.EqualTo(4));
            Assert.That(_root[0].IsLocator, Is.True);
            Assert.That(_root[0].Count, Is.EqualTo(3));
            Assert.That(_root[0][2].Content, Is.EqualTo("## t0.3"));
        });
    }

    [Test]
    public void TestToFormattedString_Five_to_One()
    {
        _root.Clear();
        _root.Append(new TestLevelList { Level = 5, Content = "# t5" });
        _root.Append(new TestLevelList { Level = 4, Content = "# t4" });
        _root.Append(new TestLevelList { Level = 3, Content = "# t3" });
        _root.Append(new TestLevelList { Level = 2, Content = "# t2" });
        _root.Append(new TestLevelList { Level = 1, Content = "# t1" });
        _root.Append(new TestLevelList { Level = 2, Content = "# t2" });
        _root.Append(new TestLevelList { Level = 3, Content = "# t3" });
        _root.Append(new TestLevelList { Level = 4, Content = "# t4" });
        _root.Append(new TestLevelList { Level = 5, Content = "# t5" });
        var result = _root.ToFormatedString();
        var expected =
@"          # t5
        # t4
      # t3
    # t2
  # t1
    # t2
      # t3
        # t4
          # t5
";
        Assert.That(result, Is.EqualTo(expected));

    }

    [Test]
    public void TestToFormattedString_One_To_Five()
    {
        _root.Clear();
        _root.Append(new TestLevelList { Level = 1, Content = "# t1" });
        _root.Append(new TestLevelList { Level = 2, Content = "# t2" });
        _root.Append(new TestLevelList { Level = 3, Content = "# t3" });
        _root.Append(new TestLevelList { Level = 4, Content = "# t4" });
        _root.Append(new TestLevelList { Level = 5, Content = "# t5" });
        _root.Append(new TestLevelList { Level = 5, Content = "# r5" });
        _root.Append(new TestLevelList { Level = 4, Content = "# r4" });
        _root.Append(new TestLevelList { Level = 3, Content = "# r3" });
        _root.Append(new TestLevelList { Level = 2, Content = "# r2" });
        _root.Append(new TestLevelList { Level = 1, Content = "# r1" });
        var result = _root.ToFormatedString();
        var expected =
@"  # t1
    # t2
      # t3
        # t4
          # t5
          # r5
        # r4
      # r3
    # r2
  # r1
";
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void TestPatternX()
    {
        _root.Clear();
        _root.Append(new TestLevelList { Level = 1, Content = "# t1" });
        _root.Append(new TestLevelList { Level = 4, Content = "# t2" });
        _root.Append(new TestLevelList { Level = 2, Content = "# t3" });
        _root.Append(new TestLevelList { Level = 4, Content = "# t4" });
        _root.Append(new TestLevelList { Level = 1, Content = "# t5" });
        _root.Append(new TestLevelList { Level = 5, Content = "# t6" });
        var result = _root.ToFormatedString();
        var expected =
@"  # t1
        # t2
    # t3
        # t4
  # t5
          # t6
";
        Assert.That(result, Is.EqualTo(expected));
    }
}
