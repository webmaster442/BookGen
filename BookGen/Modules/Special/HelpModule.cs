//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Framework;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Modules.Special
{
    internal class HelpModule : ModuleBase, IModuleCollection
    {
        public override string ModuleCommand => "Help";

        public IEnumerable<ModuleBase>? Modules { get; set; }

        public override ModuleRunResult Execute(string[] arguments)
        {
            if (Modules == null)
                throw new DependencyException("Modules is null");

            string? helpScope = arguments.Length > 0 ? arguments[0] : string.Empty;

            var foundMoudle = Modules.FirstOrDefault(m => string.Compare(m.ModuleCommand, helpScope, true) == 0);

            if (foundMoudle == null)
            {
                Console.WriteLine(HelpUtils.GetGeneralHelp());
            }
            else
            {
                Console.WriteLine(foundMoudle.GetHelp());
            }

            return ModuleRunResult.Succes;
        }

        public override string GetHelp()
        {
            return "Displays help for a given subcommand";
        }
    }
}
