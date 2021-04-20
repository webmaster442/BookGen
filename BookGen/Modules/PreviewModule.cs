//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Framework.Server;
using BookGen.Ui.ArgumentParser;
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

        public override bool Execute(string[] arguments)
        {
            const string url = "http://localhost:8082/";

            BookGenArgumentBase args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            using (var server = new HttpServer(args.Directory,
                                               8082,
                                               CurrentState.Log,
                                               new PreviewStaticHandler(),
                                               new PreviewRenderHandler(args.Directory, CurrentState.Log)))
            {
                CurrentState.Log.Info("-------------------------------------------------");
                CurrentState.Log.Info("Test server running on: {0}", url);
                CurrentState.Log.Info("Serving from: {0}", args.Directory);

                if (Program.AppSetting.AutoStartWebserver)
                {
                    GeneratorRunner.StartUrl(url);
                }

                Console.WriteLine(GeneratorRunner.ExitString);
                Console.ReadLine();

            }
            return true;
        }
    }
}
