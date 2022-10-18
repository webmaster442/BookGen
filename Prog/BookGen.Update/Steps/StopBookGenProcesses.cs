using BookGen.Update.Infrastructure;
using System.Diagnostics;

namespace BookGen.Update.Steps;

internal sealed class StopBookGenProcesses : IUpdateStepAsync
{
    public async Task<bool> Execute(GlobalState state)
    {
        Console.WriteLine("Stopping all running BookGen instances...");
        await Task.Delay(3000);

        var processes = Process.GetProcessesByName("BookGen");
        if (processes != null)
        {
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        return true;
    }
}
