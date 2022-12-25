//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;
using BookGen.Update.ShellCommands;
using System.Runtime.InteropServices;
using static BookGen.Update.ShellCommands.ShellFileGenerator;

namespace BookGen.Update.Steps;

internal sealed class Finish : IUpdateStepAsync
{
    public string StatusMessage => string.Empty;

    public async Task<bool> Execute(GlobalState state)
    {
        ShellFileGenerator generator = new ShellFileGenerator();
        generator.AddFiles(state.PostProcessFiles);
        generator.Finish(state.Latest.Version);
        state.Cleanup();

        ShellType currentShell = Detect();

        string name = currentShell == ShellType.Bash ? "update.sh" : "update.ps1";
        var commands = generator.Generate(currentShell);
        var fileName = Path.Combine(state.TargetDir, name);

        await File.WriteAllTextAsync(fileName, commands);
       
        return true;
    }

    private static ShellType Detect()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)
            || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return ShellType.Bash;
        }
        else
        {
            return ShellType.Powershell;
        }
    }
}
