using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Framework;

namespace BookGen.Commands
{
    [CommandName("confighelp")]
    internal class ConfigHelpCommand : Command
    {
        public override int Execute(string[] context)
        {
            Console.WriteLine(HelpUtils.DocumentConfiguration());
            return Constants.Succes;
        }
    }
}
