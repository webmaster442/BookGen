//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Resources;
using BookGen.Ui.ArgumentParser;
using System.Text;

namespace BookGen.Modules
{
    internal class InstallPSAutocompleteModule : ModuleWithState
    {
        protected InstallPSAutocompleteModule(ProgramState currentState) : base(currentState)
        {
        }

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem(ModuleCommand);

        public override string ModuleCommand => "InstallPsAutocomplete";

        public override bool Execute(string[] arguments)
        {
            var args = new InstallPsArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }


            FsPath target = new FsPath(args.Files[0]);

            StringBuilder contents = new StringBuilder(4096);

            if (args.Dotnet)
            {
                var dnc = ResourceHandler.GetResourceFile<KnownFile>("Powershell/completer.dn.ps1");
                contents.Append(dnc);
            }

            var completer = ResourceHandler.GetResourceFile<KnownFile>("Powershell/completer.ps1");
            contents.Append(completer);

            target.WriteFile(CurrentState.Log, contents.ToString());

            return true;
        }
    }
}
