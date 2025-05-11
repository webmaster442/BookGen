//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shellprog.CommandCode.Organize;

using Microsoft.Extensions.Logging;

namespace BookGen.Shellprog;

[CommandName("organize")]
internal class OrganizeCommand : Command<OrganizeArguments>
{
    private readonly ILogger _log;

    public OrganizeCommand(ILogger log)
    {
        _log = log;
    }

    public override int Execute(OrganizeArguments arguments, string[] context)
    {
        try
        {
            var ruleLoader = new RuleLoader(arguments.Folder);
            var rules = ruleLoader.LoadRules();
            var engine = new RuleEngine(rules, _log);
            engine.Run(arguments.Folder, arguments.Simulate);
            return 0;
        }
        catch (Exception ex)
        {
            _log.LogCritical(ex, "Critical error");
            return -1;
        }
    }
}
