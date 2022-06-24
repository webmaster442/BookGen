﻿//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Infrastructure;

namespace BookGen.Modules.Special
{
    internal class SubCommandsModule : ModuleBase, IModuleCollection
    {
        public override string ModuleCommand => "SubCommands";

        public IEnumerable<ModuleBase>? Modules { get; set; }

        public override ModuleRunResult Execute(string[] arguments)
        {
            if (Modules == null)
                throw new DependencyException("Modules is null");


            Console.WriteLine("Available sub commands: \r\n");
            foreach (ModuleBase? module in Modules)
            {
                Console.WriteLine(module.ModuleCommand);
            }
            return ModuleRunResult.Succes;
        }

        public override string GetHelp()
        {
            return "List available subcommands";
        }
    }
}