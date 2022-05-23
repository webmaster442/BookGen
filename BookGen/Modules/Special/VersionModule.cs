//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;

namespace BookGen.Modules.Special
{
    internal class VersionModule : ModuleBase
    {
        public override string ModuleCommand => "Version";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-bd",
                                            "--builddate",
                                            "-api",
                                            "--apiversion");
            }
        }

        public override ModuleRunResult Execute(string[] arguments)
        {
            //VersionArguments
            var args = new VersionArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            if (args.IsDefault)
            {
                Console.WriteLine("BookGen Build date: {0:yyyy.MM.dd}", Program.CurrentState.BuildDate.Date);
                Console.WriteLine("Build timestamp: {0:HH:mm:ss}", Program.CurrentState.BuildDate);
                Console.WriteLine("Config API version: {0}", Program.CurrentState.ProgramVersion);
            }
            if (args.BuildDate)
            {
                Console.WriteLine("{0:yyyy.MM.dd}", Program.CurrentState.BuildDate.Date);
            }
            if (args.ApiVersion)
            {
                Console.WriteLine("{0}", Program.CurrentState.ProgramVersion);
            }
            return ModuleRunResult.Succes;
        }

        public override string GetHelp()
        {
            return "Print program version.";
        }
    }
}
