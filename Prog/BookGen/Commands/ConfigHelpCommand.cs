//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;

namespace BookGen.Commands;

[CommandName("confighelp")]
internal class ConfigHelpCommand : Command
{
    public override int Execute(string[] context)
    {
        throw new NotImplementedException();
        //Console.WriteLine(HelpUtils.DocumentConfiguration());
        return Constants.Succes;
    }
}
