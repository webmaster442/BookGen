//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Gui.ArgumentParser;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ArgumentParser
    {
        [TestCase("-v")]
        [TestCase("--verbose")]
        public void EnsureThat_ArgumentParser_Parses_Switch_Correctly(string sw)
        {
            var args = new BookGenArgumentBase();
            bool result = ArgumentParser.ParseArguments(new string[] { sw }, args);
            Assert.Multiple(() =>
            {
                Assert.That(args.Verbose, Is.True);
                Assert.That(result, Is.True);
            });
        }

        [TestCase("-d", "d:\\test")]
        [TestCase("--dir", "d:\\test12")]
        public void EnsureThat_ArgumentParser_Parses_SwitchWithValue_Correctly(string sw, string param)
        {
            var args = new BookGenArgumentBase();
            bool result = ArgumentParser.ParseArguments(new string[] { sw, param }, args);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(args.Directory, Is.EqualTo(param));
            });
        }
    }
}
