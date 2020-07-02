//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework.Server;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System;

namespace BookGen.Modules
{
    internal class ServeModule : StateModuleBase
    {
        public ServeModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Serve";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem("Serve", "-d", "--dir", "-v", "--verbose");
            }
        }

        public override bool Execute(string[] arguments)
        {
            BookGenArgumentBase args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            Api.LogLevel logLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            var log = new ConsoleLog(logLevel);

            using (var server = new HttpServer(args.Directory, 8081, log))
            {
                Console.WriteLine("Serving: {0}", args.Directory);
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
