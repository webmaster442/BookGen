//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.TaskRunner;

namespace BookGen.DomainServices;

public class BookGenTaskRunner
{
    private readonly Dictionary<string, bool> _confirmVars;
    private readonly Dictionary<string, string> _promptResults;

    public BookGenTaskRunner()
    {
        _confirmVars = new Dictionary<string, bool>();
        _promptResults = new Dictionary<string, string>();
    }

    public void SetConfirmResult(string varialbe, bool result)
    {
        _confirmVars[varialbe] = result;
    }

    public void SetPromptResult(string varialbe, string result)
    {
        _promptResults[varialbe] = result;
    }

    public async Task RunTask(TaskItem item)
    {
        if (item is ShellCommands commands)
        {
            if (!string.IsNullOrEmpty(commands.ConditionVariable))
            {
                if (!_confirmVars.ContainsKey(commands.ConditionVariable))
                    throw new InvalidOperationException($"{commands.ConditionVariable} hasn't been set");

                if (_confirmVars[commands.ConditionVariable])
                    await Run(commands.Commands, commands.ShellType);
            }
            else
            {
                await Run(commands.Commands, commands.ShellType);
            }
        }
        else
        {
            throw new NotSupportedException($"{item.GetType().FullName} is not supported");
        }
    }

    private async static Task Run(string commands, ShellType shellType)
    {
        var tempFileName = Path.GetTempFileName();
        File.WriteAllText(tempFileName, commands);
        
        switch (shellType)
        {
            case ShellType.Powershell:
                await ProcessRunner.RunPowershellScript(tempFileName);
                break;
            case ShellType.Cmd:
                await ProcessRunner.RunCmdScript(tempFileName);
                break;
        }

        if (File.Exists(tempFileName))
            File.Delete(tempFileName);
    }
}
