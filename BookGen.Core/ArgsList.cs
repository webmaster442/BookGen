//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Core
{
    public class ArgsList : IEnumerable<ArgumentItem>
    {
        private readonly List<ArgumentItem> _items;

        private ArgsList(int count)
        {
            _items = new List<ArgumentItem>(count);
        }

        public ArgumentItem GetArgument(string switchname, string longname)
        {
            return _items.FirstOrDefault(item => string.Compare(item.Switch, switchname, true) == 0
                                              || string.Compare(item.Switch, longname, true) == 0);
        }

        public static ArgsList Parse(string[] args)
        {
            ArgsList ret = new ArgsList(args.Length);
            int i = 0;
            while (i < args.Length)
            {
                var current = args[i].ToLower();
                var next = i + 1 < args.Length ? args[i + 1].ToLower() : "";
                bool nextIsswitch = next.StartsWith("-");
                bool currentIsSwitch = current.StartsWith("-");

                if (currentIsSwitch && !nextIsswitch)
                {
                    i += 2;
                    ret._items.Add(new ArgumentItem
                    {
                        Switch = ParseSwitch(current),
                        Value = next
                    });
                }
                else if (currentIsSwitch && nextIsswitch)
                {
                    ++i;
                    ret._items.Add(new ArgumentItem
                    {
                        Switch = ParseSwitch(current)
                    });
                }
                else if (!currentIsSwitch)
                {
                    ++i;
                    ret._items.Add(new ArgumentItem
                    {
                        Value = current
                    });
                }
            }
            return ret;
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

        public IEnumerator<ArgumentItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
