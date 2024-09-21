//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace BookGen.Commands;

[CommandName("webgui")]
internal sealed class WebGuiCommand : Command
{
    public override int Execute(string[] context)
    {
        string fileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?  "BookGen.WebGui.exe" : "BookGen.WebGui";

        var program = Path.Combine(AppContext.BaseDirectory, fileName);
        ProcessRunner.RunProcess(program, $"-d {Environment.CurrentDirectory}");

        return Constants.Succes;
    }
}
