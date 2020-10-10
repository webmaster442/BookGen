//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Modules.Special
{
    internal class ShellModule : BaseModule, IModuleCollection
    {
        public override string ModuleCommand => "Shell";

        public IEnumerable<StateModuleBase>? Modules { get; set; }

        public override bool Execute(string[] arguments)
        {
            IEnumerable<string> results = DoComplete(arguments);

            foreach (var item in results)
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

            if (request.StartsWith("BookGen", StringComparison.OrdinalIgnoreCase))
            {
                request = request.Substring("BookGen".Length);
            }
            string[] words = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {
                StateModuleBase? command = Modules?.FirstOrDefault(c => c.AutoCompleteInfo.ModuleName.StartsWith(words[0], StringComparison.OrdinalIgnoreCase));
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

            return Enumerable.Empty<string>();
        }

        public override string GetHelp()
        {
            return "Shell Autocomplete";
        }
    }
}
