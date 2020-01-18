//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Framework.Editor;
using BookGen.Framework.Server;
using System;
using System.Diagnostics;

namespace BookGen.Mdoules
{
    internal class EditorModule : ModuleBase
    {
        public EditorModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Editor";

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            var dir = tokenizedArguments.GetSwitchWithValue("d", "dir");

            string workdir = Environment.CurrentDirectory;

            if (!string.IsNullOrEmpty(dir))
                workdir = dir;

            ILog log = new ConsoleLog(Api.LogLevel.Info);

            IRequestHandler[] handlers = new IRequestHandler[]
            {
                new DynamicHandlers(workdir),
                new EditorIndexHandler(),
                new EmbededResourceRequestHandler()
            };

            using (var server = new HttpServer(workdir, 9090, log, handlers))
            {
                log.Info("Editor started on: http://localhost:9090");
                log.Info("Press a key to exit...");

                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = "http://localhost:9090";
                p.Start();

                Console.ReadLine();
            }

            return true;
        }
    }
}
