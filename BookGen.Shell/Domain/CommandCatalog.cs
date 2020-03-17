//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Shell.Domain
{
    public static class CommandCatalog
    {
        public static IEnumerable<AutoCompleteItem> Commands
        {
            get
            {
                yield return new AutoCompleteItem(
                    "AssemblyDocument",
                    "-a",
                    "--assembly",
                    "-x",
                    "--xml",
                    "-o",
                    "--output");

                yield return new AutoCompleteItem(
                    "Build",
                    "-n",
                    "--nowait",
                    "-v",
                    "--verbose",
                    "-d",
                    "--dir",
                    "-a",
                    "--action",
                    "Test",
                    "BuildPrint",
                    "BuildWeb",
                    "BuildEpub",
                    "BuildWordpress",
                    "Clean",
                    "ValidateConfig");

                yield return new AutoCompleteItem("ConfigHelp");

            }
        }
    }
}
