//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Shell.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Shell
{
    public static class ShellProgram
    {
        public static IEnumerable<string> DoComplete(string[] args)
        {
            if (args.Length == 0)
                return CommandCatalog.Commands.Select(c => c.ModuleName);

            string request = args[0] ?? "";

            if (request.StartsWith("BookGen", StringComparison.OrdinalIgnoreCase))
            {
                request = request.Substring("BookGen".Length);
            }
            string[] words = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {
                var command = CommandCatalog.Commands.FirstOrDefault(c => c.ModuleName.StartsWith(words[0], StringComparison.OrdinalIgnoreCase));
                if (words.Length > 1)
                {
                    return command.ArgumentList.Where(c => c.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    return new string[] { command.ModuleName };
                }
            }

            return Enumerable.Empty<string>();
        }

        public static void Main(string[] args)
        {
            var results = DoComplete(args);
            WriteEnumerable(results);
        }

        private static void WriteEnumerable(IEnumerable<string> enumerable)
        {
            foreach (var item in enumerable)
            {
                Console.WriteLine(item);
            }
        }
    }
}