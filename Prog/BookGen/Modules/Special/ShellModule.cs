//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Framework;
using BookGen.Utilities;

namespace BookGen.Modules.Special
{
    internal class ShellModule : ModuleBase, IModuleCollection
    {
        public override string ModuleCommand => "Shell";

        public IEnumerable<ModuleBase>? Modules { get; set; }

        public override ModuleRunResult Execute(string[] arguments)
        {
            foreach (var item in DoComplete(arguments))
            {
                Console.WriteLine(item);
            }
            return ModuleRunResult.Succes;
        }

        internal IEnumerable<string> DoComplete(string[] args)
        {
            if (args.Length == 0)
                return Modules?.Select(m => m.AutoCompleteInfo.ModuleName) ?? Enumerable.Empty<string>();

            string request = args[0] ?? "";

            if (request.StartsWith(Constants.ProgramName, StringComparison.OrdinalIgnoreCase))
            {
                request = request.Substring(Constants.ProgramName.Length);
            }
            string[] words = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {
                ModuleBase? command = Modules?.FirstOrDefault(c => c.AutoCompleteInfo.ModuleName.StartsWith(words[0], StringComparison.OrdinalIgnoreCase));
                if (command != null)
                {
                    if (words.Length > 1)
                    {
                        var candidate = command.AutoCompleteInfo.ArgumentList.Where(c => c.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));
                        if (candidate.Any())
                            return candidate;
                        else
                            return ProgramConfigurator.GeneralArguments.Where(c => c.StartsWith(words.Last(), StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        return new string[] { command.AutoCompleteInfo.ModuleName };
                    }
                }
            }

            return new string[] { Constants.ProgramName };
        }

        public override string GetHelp()
        {
            return "Shell Autocomplete";
        }
    }
}
