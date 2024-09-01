//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.TaskRunner;

using Microsoft.Extensions.Logging;

namespace BookGen.DomainServices;

public sealed class BookGenTaskRunner
{
    private readonly Dictionary<string, bool> _confirmVars;
    private readonly Dictionary<string, string> _promptResults;
    private readonly ILogger _log;

    public BookGenTaskRunner(ILogger log)
    {
        _confirmVars = new Dictionary<string, bool>();
        _promptResults = new Dictionary<string, string>();
        _log = log;
    }

    public void SetConfirmResult(string varialbe, bool result)
    {
        _confirmVars[varialbe] = result;
    }

    public void SetPromptResult(string varialbe, string result)
    {
        _promptResults[varialbe] = result;
    }

    public void RunTask(TaskItem item)
    {
        if (item is ShellCommands commands)
        {
            if (!string.IsNullOrEmpty(commands.ConditionVariable))
            {
                if (!_confirmVars.ContainsKey(commands.ConditionVariable))
                    throw new InvalidOperationException($"{commands.ConditionVariable} hasn't been set");

                if (_confirmVars[commands.ConditionVariable])
                    Run(commands.Commands, commands.ShellType);
            }
            else
            {
                Run(commands.Commands, commands.ShellType);
            }
        }
        else
        {
            throw new NotSupportedException($"{item.GetType().FullName} is not supported");
        }
    }

    private void Run(string commands, ShellType shellType)
    {
        SetVariables();
        var tempFileName = GenerateName();

        switch (shellType)
        {
            case ShellType.Powershell:
                tempFileName = Path.ChangeExtension(tempFileName, ".ps1");
                File.WriteAllText(tempFileName, commands);
                ProcessRunner.RunPowershellScript(tempFileName, _log);
                break;
            case ShellType.Cmd:
                tempFileName = Path.ChangeExtension(tempFileName, ".cmd");
                File.WriteAllText(tempFileName, commands);
                ProcessRunner.RunCmdScript(tempFileName, _log);
                break;
        }

        if (File.Exists(tempFileName))
            File.Delete(tempFileName);

        CleanupVariables();
    }

    private static string GenerateName()
    {
        char[] name = new char[15];
        for (int i=0; i<name.Length; i++)
        {
            name[i] = (char)Random.Shared.Next('a', 'z');
        }
        return new string(name);
    }

    private void CleanupVariables()
    {
        foreach (var variable in _promptResults.Keys)
        {
            Environment.SetEnvironmentVariable(variable, null);
        }
    }

    private void SetVariables()
    {
        foreach (var variable in _promptResults.Keys)
        {
            Environment.SetEnvironmentVariable(variable, _promptResults[variable]);
        }
    }
}
