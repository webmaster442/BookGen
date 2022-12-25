//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;
using BookGen.Update.ShellCommands;

namespace BookGen.Update.Steps;

internal sealed class Finish : IUpdateStepAsync
{
    public string StatusMessage => string.Empty;



    public async Task<bool> Execute(GlobalState state)
    {
        var generator = new ShellFileGenerator();
        if (state.PostProcessFiles.Count > 0)
        {
            generator.AddFiles(state.PostProcessFiles);
            generator.Finish(state.Latest.Version);
            state.Cleanup();

            string commands = generator.Generate(state.ShellType);

            await File.WriteAllTextAsync(state.UpdateShellFileName, commands);
        }
        else
        {
            Console.WriteLine($"Updated Bookgen to version: {state.Latest.Version}");
        }

        return true;
    }
}
