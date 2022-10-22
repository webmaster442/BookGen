//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;

namespace BookGen.Update.Steps;

internal sealed class WelcomeStep : IUpdateStepSync
{
    public bool Execute(GlobalState state)
    {
        Console.Clear();
        Console.WriteLine(@"  ____              _     _____              _    _           _       _            ");
        Console.WriteLine(@" |  _ \            | |   / ____|            | |  | |         | |     | |           ");
        Console.WriteLine(@" | |_) | ___   ___ | | _| |  __  ___ _ __   | |  | |_ __   __| | __ _| |_ ___ _ __ ");
        Console.WriteLine(@" |  _ < / _ \ / _ \| |/ / | |_ |/ _ \ '_ \  | |  | | '_ \ / _| |/ _| | __/ _ \ '__|");
        Console.WriteLine(@" | |_) | (_) | (_) |   <| |__| |  __/ | | | | |__| | |_) | (_| | (_| | ||  __/ |   ");
        Console.WriteLine(@" |____/ \___/ \___/|_|\_\\_____|\___|_| |_|  \____/| .__/ \__,_|\__,_|\__\___|_|   ");
        Console.WriteLine(@"                                                   | |                             ");
        Console.WriteLine(@"                                                   |_|                             ");
        return true;
    }
}
