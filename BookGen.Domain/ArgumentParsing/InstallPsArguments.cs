//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;

namespace BookGen.Domain.ArgumentParsing
{
    internal class InstallPsArguments : ArgumentsBase
    {
        [Switch("dn", "dotnet")]
        public bool Dotnet { get; set; }

        public override bool Validate()
        {
            return Files.Length == 1;
        }
    }
}
