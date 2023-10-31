//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Web;

using BookGen.Domain.Www;

using Spectre.Console;

using Www;

var config = new ConfigLoader();
var argParser = new WwwArgumentParser(args);

static void OpenUrl(string url)
{
    if (!url.StartsWith("http://") && !url.StartsWith("https://"))
        throw new ArgumentException("invalid url", nameof(url));

    using (var process = new Process())
    {
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = url;
        process.Start();
    }
}

static void LaunchBang(WwwBang[] bangs, string[] args)
{
    var bang = bangs.FirstOrDefault(bang => bang.Activator == args[0]);
    if (bang == null)
    {
        AnsiConsole.WriteLine($"[red]Bang not found: {args[0].EscapeMarkup()}[/]");
        return;
    }
    string url = string.Join(bang.Delimiter, args.Select(x => HttpUtility.UrlEncode(x)));
    OpenUrl(url);
}

if (argParser.HasArguments)
{
    if (argParser.FirstParamIsUrl)
    {
        OpenUrl(args[0]);
    }
    else if (argParser.IsBangFormat)
    {
        LaunchBang(config.Bangs, args);
    }
    else throw new UnreachableException("This shouldn't happen");
}
else
{
    new MainMenu(config.Favorites, config.Bangs).Run();
}