//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Linq;

namespace BookGen.Gui.ArgumentParser
{
    public static class SubcommandParser
    {
        public static string GetCommand(string[] input, out string[] rest)
        {
            if (input.Length == 0)
            {
                rest = new string[0];
                return string.Empty;
            }
            rest = input.Skip(1).ToArray();
            return input[0];
        }
    }
}
