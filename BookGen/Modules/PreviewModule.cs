//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Framework.Server;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;
using System;

namespace BookGen.Modules
{
    internal class PreviewModule : ModuleWithState
    {
        public PreviewModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "preview";

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem(ModuleCommand,
                                                                                  "-v",
                                                                                  "--verbose",
                                                                                  "-d",
                                                                                  "--dir");

        public override ModuleRunResult Execute(string[] arguments)
        {
            const string url = "http://localhost:8082/";

            BookGenArgumentBase args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            using (var server = HttpServerFactory.CreateServerForPreview(CurrentState.Log, CurrentState.ServerLog, args.Directory))
            {
                server.Start();
                CurrentState.Log.Info("-------------------------------------------------");
                CurrentState.Log.Info("Test server running on: {0}", url);
                CurrentState.Log.Info("Serving from: {0}", args.Directory);

                if (Program.AppSetting.AutoStartWebserver)
                {
                    UrlOpener.OpenUrl(url);
                }

                Console.WriteLine(GeneratorRunner.ExitString);
                Console.ReadLine();
                server.Stop();
            }
            return ModuleRunResult.Succes;
        }
    }
}