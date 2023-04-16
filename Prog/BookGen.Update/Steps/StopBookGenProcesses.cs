//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Update.Infrastructure;

namespace BookGen.Update.Steps;

internal sealed class StopBookGenProcesses : IUpdateStepAsync
{
    public string StatusMessage => "Shuting down running BookGen instances...";

    public async Task<bool> Execute(GlobalState state)
    {
        Console.WriteLine("Stopping all running BookGen instances...");
        await Task.Delay(3000);

        Process[] processes = Process.GetProcessesByName("BookGen");
        if (processes != null)
        {
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }

        return true;
    }
}
