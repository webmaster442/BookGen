//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.Shell;
using BookGen.Framework.Server;
using BookGen.Utilities;
using System;

namespace BookGen.Mdoules
{
    internal class ServeModule : ModuleBase
    {
        public ServeModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Serve";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem("Serve", "-d", "--dir");
            }
        }

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            var dir = tokenizedArguments.GetSwitchWithValue("d", "dir");

            if (string.IsNullOrEmpty(dir))
                dir = Environment.CurrentDirectory;


            var log = new ConsoleLog(Api.LogLevel.Detail);

            using (var server = new HttpServer(dir, 8081, log))
            {
                Console.WriteLine("Serving: {0}", dir);
                Console.WriteLine("Server running on http://localhost:8081");
                Console.WriteLine("Press a key to exit...");
                Console.ReadLine();
            }


            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(ServeModule));
        }
    }
}
