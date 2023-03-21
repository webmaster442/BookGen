//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("confighelp")]
internal class ConfigHelpCommand : Command
{
    public override int Execute(string[] context)
    {
        var result = new StringBuilder(4096);
        result.AppendLine("Configuration properties for current version:");
        result.AppendLine();
        result.AppendLine();
        ClassDocumenter.DocumentType<Config>(out string properties, out string types);
        result.AppendLine(properties);
        result.AppendLine();
        result.AppendLine();
        result.AppendLine(types);

        var text = result.ToString();
        Console.WriteLine(text);
        return Constants.Succes;
    }
}
