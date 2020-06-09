//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Domain.VsTasks;
using Fizzler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Modules.Special
{
    internal class ShellModule : BaseModule, IModuleCollection
    {
        public override string ModuleCommand => "Shell";

        public IEnumerable<StateModuleBase>? Modules { get; set; }

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            var args = tokenizedArguments.Raw;

            if (args.Length > 2
                && string.Equals(args[0], "BookGen", StringComparison.OrdinalIgnoreCase)
                && string.Equals(args[1], "Shell", StringComparison.OrdinalIgnoreCase))
            {
                args = args.Skip(2).ToArray();
            }

            IEnumerable<string> results = DoComplete(args);

            foreach (var item in results)
            {
                Console.WriteLine(item);
            }
            return true;
        }

        internal IEnumerable<string> DoComplete(string[] args)
        {
            if (args.Length == 0)
                return Modules.Select(m => m.AutoCompleteInfo.ModuleName);

            string request = args[0] ?? "";

            if (request.StartsWith("BookGen", StringComparison.OrdinalIgnoreCase))
            {
                request = request.Substring("BookGen".Length);
            }
            string[] words = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {
                StateModuleBase? command = Modules.FirstOrDefault(c => c.AutoCompleteInfo.ModuleName.StartsWith(words[0], StringComparison.OrdinalIgnoreCase));
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
