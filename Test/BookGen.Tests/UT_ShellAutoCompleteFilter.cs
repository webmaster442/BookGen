//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests;

[TestFixture]
public class UT_ShellAutoCompleteFilter
{
    [Test]
    public void EnsureThat_ShellAutoCompleteFilter_DoFilter_ReturnsGood()
    {
        var results = ShellAutoCompleteFilter.DoFilter(["git add", "git add ."], "git a", 5);
        Assert.That(results, Is.EqualTo(new[] { "add", "add ." }).AsCollection);
    }
}
