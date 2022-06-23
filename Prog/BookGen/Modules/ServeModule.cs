//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Framework.Server;
using BookGen.Gui.ArgumentParser;

namespace BookGen.Modules
{
    internal class ServeModule : ModuleWithState
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

        public override ModuleRunResult Execute(string[] arguments)
        {
            var args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Log.LogLevel = args.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

            CheckLockFileExistsAndExitWhenNeeded(args.Directory);

            using (var l = new FolderLock(args.Directory))
            {
                using (Webmaster442.HttpServerFramework.HttpServer? server = HttpServerFactory.CreateServerForServModule(CurrentState.ServerLog, args.Directory))
                {
                    server.Start();
                    Console.WriteLine("Serving: {0}", args.Directory);
                    Console.WriteLine("Server running on http://localhost:8081");
                    Console.WriteLine("Press a key to exit...");
                    Console.ReadLine();
                    server.Stop();
                }
            }

            return ModuleRunResult.Succes;
        }
    }
}
