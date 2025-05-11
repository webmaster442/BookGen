//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Shell.Shared;

namespace BookGen.Tests.Shell.Shared;

[TestFixture]
public class UT_ShellAutoCompleteFilter
{
    [Test]
    public void EnsureThat_ShellAutoCompleteFilter_DoFilter_ReturnsGood()
    {
        var results = ShellAutoCompleteFilter.DoFilter(["git add", "git add ."], "git a", 5);
        Assert.That(results, Is.EqualTo(new[] { "add", "add ." }).AsCollection);
    }

    [TestCase("git me", 6, "merge master")]
    [TestCase("git merge", 9, "merge master")]
    public void EnsureThat_ShellAutoCompleteFilter_DoFilter_ReturnsGood_Mutiple_Positions(string input, int position, string expected)
    {
        string[] data =
        [
            "git merge master",
        ];
        var results = ShellAutoCompleteFilter.DoFilter(data, input, position);
        Assert.That(results, Is.EqualTo(new[] { expected }).AsCollection);
    }

    [Test]
    public void EnsureThat_GetWordPositions_Works()
    {
        var results = ShellAutoCompleteFilter.GetWordPositions("git merge master").ToArray();
        Assert.That(results, Is.EqualTo(new[] { (0, 3), (4, 9), (10, 16) }).AsCollection);
    }
}
