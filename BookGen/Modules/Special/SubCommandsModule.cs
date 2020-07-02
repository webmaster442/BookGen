//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Framework;
using System;
using System.Collections.Generic;

namespace BookGen.Modules.Special
{
    internal class SubCommandsModule : BaseModule, IModuleCollection
    {
        public override string ModuleCommand => "SubCommands";

        public IEnumerable<StateModuleBase>? Modules { get; set; }

        public override bool Execute(string[] arguments)
        {
            if (Modules == null)
                throw new DependencyException("Modules is null");


            Console.WriteLine("Available sub commands: \r\n");
            foreach (var module in Modules)
            {
                Console.WriteLine(module.ModuleCommand);
            }
            return true;
        }

        public override string GetHelp()
        {
            return string.Empty;
        }
    }
}
