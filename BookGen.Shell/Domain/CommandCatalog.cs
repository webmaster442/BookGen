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

                yield return new AutoCompleteItem(
                    "Editor",
                    "-d",
                    "--dir");

                yield return new AutoCompleteItem("Gui",
                    "-d",
                    "--dir",
                    "-v",
                    "--verbose");

                yield return new AutoCompleteItem("Help");

                yield return new AutoCompleteItem("Init",
                    "-d",
                    "--dir");

                yield return new AutoCompleteItem("Md2HTML",
                    "-i",
                    "--input",
                    "-o",
                    "--output",
                    "-c",
                    "--css");

                yield return new AutoCompleteItem(
                    "Serve",
                    "-d",
                    "--dir");

                yield return new AutoCompleteItem(
                    "Settings",
                    "get",
                    "list",
                    "set");

                yield return new AutoCompleteItem("SubCommands");

                yield return new AutoCompleteItem(
                    "Update",
                    "-p",
                    "--prerelease",
                    "-s",
                    "--searchonly");

                yield return new AutoCompleteItem("Version");
            }
        }
    }
}
