//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace BookGen.Core
{
    public class ArgumentParser
    {
        private readonly List<ArgumentItem> _arguments;

        public ArgumentParser(string[] args)
        {
            _arguments = new List<ArgumentItem>(args.Length);
            DoParse(args);
        }

        private void DoParse(string[] args)
        {
            int i = 0;
            bool nextIsswitch, currentIsSwitch;
            while (i < args.Length)
            {
                string current = args[i].ToLower();
                string next = (i + 1) < args.Length ? args[i + 1].ToLower() : string.Empty;
                nextIsswitch = next.StartsWith("-");
                currentIsSwitch = current.StartsWith("-");

                if (currentIsSwitch)
                {
                    if (nextIsswitch)
                    {
                        ++i;
                        _arguments.Add(new ArgumentItem(ParseSwitch(current)));
                    }
                    else
                    {
                        i += 2;
                        _arguments.Add(new ArgumentItem(ParseSwitch(current), next));
                    }
                }
                else
                {
                    ++i;
                    _arguments.Add(new ArgumentItem(string.Empty, current));
                }
            }
        }

        private static string ParseSwitch(string current)
        {
            if (current.StartsWith("--"))
                return current.Substring(2);
            else if (current.StartsWith("-"))
                return current.Substring(1);
            else
                return current;
        }

        private static void NormalizeNames(ref string switchname, ref string longname)
        {
            if (switchname.StartsWith("-")) switchname = switchname.Substring(1);
            if (longname.StartsWith("--")) longname = longname.Substring(2);
        }

        public bool GetSwitch(string switchname, string longname)
        {
            NormalizeNames(ref switchname, ref longname);

            var args = SelectArgsWithMatchingNames(switchname, longname);

            return args.FirstOrDefault(a => a.IsStandaloneSwitch) != null;
        }

        public string GetSwitchWithValue(string switchname, string longname)
        {
            NormalizeNames(ref switchname, ref longname);

            var args = SelectArgsWithMatchingNames(switchname, longname);

            return args.FirstOrDefault(a => a.IsArgumentedSwitch)?.Value ?? string.Empty;
        }

        public IEnumerable<string> GetValues()
        {
            return _arguments.Where(a => a.IsValue).Select(a => a.Value);
        }

        public bool WasHelpRequested()
        {
            return GetSwitch("h", "help");
        }

        private IEnumerable<ArgumentItem> SelectArgsWithMatchingNames(string switchname, string longname)
        {
            return from argument in _arguments
                    where
                        string.Compare(argument.Switch, switchname, true) == 0 ||
                        string.Compare(argument.Switch, longname, true) == 0
                    select
                        argument;
        }

        public IEnumerable<ArgumentItem> ArgumentItems => _arguments;
    }
}
