//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Gui.ArgumentParser
{
    public static class SubcommandParser
    {
        public static string GetCommand(IList<string> argumentsToParse)
        {
            if (argumentsToParse.Count == 0)
            {
                return string.Empty;
            }
            string command = argumentsToParse[0];
            argumentsToParse.RemoveAt(0);
            return command;
        }
    }
}
