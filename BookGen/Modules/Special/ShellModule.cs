﻿//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Modules.Special
{
    internal class ShellModule : ModuleBase, IModuleCollection
    {
        public override string ModuleCommand => "Shell";

        public const string ProgramName = "BookGen";

        public IEnumerable<ModuleBase>? Modules { get; set; }

        public override bool Execute(string[] arguments)
        {
            foreach (var item in DoComplete(arguments))
            {
                Console.WriteLine(item);
            }
            return true;
        }

        internal IEnumerable<string> DoComplete(string[] args)
        {
            if (args.Length == 0)
                return Modules?.Select(m => m.AutoCompleteInfo.ModuleName) ?? Enumerable.Empty<string>();

            string request = args[0] ?? "";

            if (request.StartsWith(ProgramName, StringComparison.OrdinalIgnoreCase))
            {
                request = request.Substring(ProgramName.Length);
            }
            string[] words = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {
                ModuleBase? command = Modules?.FirstOrDefault(c => c.AutoCompleteInfo.ModuleName.StartsWith(words[0], StringComparison.OrdinalIgnoreCase));
                if (command != null)
                {
                    if (words.Length > 1)
                    {
                        return command.AutoCompleteInfo.ArgumentList.Where(c => c.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        return new string[] { command.AutoCompleteInfo.ModuleName };
                    }
                }
            }

            return new string[] { ProgramName };
        }

        public override string GetHelp()
        {
            return "Shell Autocomplete";
        }
    }
}
