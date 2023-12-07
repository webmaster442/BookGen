//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Web;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Domain.Www;
using BookGen.Shell.Www;

using Spectre.Console;

namespace BookGen.Shell.Commands;

[CommandName("www")]
internal class WwwCommand : AsyncCommand
{
    private class WwwArguments
    {
        private readonly string[] _args;

        public WwwArguments(string[] args)
        {
            _args = args;
        }

        public bool IsHelpRequested
            => _args.Any(x => x == "-h" || x == "--help");

        public bool HasArguments
            => _args.Length > 0;

        public bool FirstParamIsUrl
            => HasArguments && Uri.TryCreate(_args[0], UriKind.RelativeOrAbsolute, out _);

        public bool IsBangFormat
            => _args.Length >= 2;
    }

    private static void OpenUrl(string url)
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

    private static void LaunchBangs(WwwBang[] bangs, string[] args)
    {
        var found = bangs.Where(bang => bang.Activator == args[0]
                                || bang.AltActivator == args[0]);
        if (!found.Any())
        {
            AnsiConsole.WriteLine($"[red]Bang not found: {args[0].EscapeMarkup()}[/]");
            return;
        }
        foreach (var bang in found)
        {
            string url = string.Join(bang.Delimiter, args.Select(x => HttpUtility.UrlEncode(x)));
            OpenUrl(url);
        }
    }

    public override async Task<int> Execute(string[] context)
    {
        var config = new ConfigLoader();
        var arguments = new WwwArguments(context);

        if (arguments.HasArguments)
        {
            if (arguments.IsHelpRequested)
            {
                HelpGenerator.PrintHelp(config.Bangs);
            }
            else if (arguments.FirstParamIsUrl)
            {
                OpenUrl(context[0]);
            }
            else if (arguments.IsBangFormat)
            {
                LaunchBangs(config.Bangs, context);
            }
            else throw new UnreachableException("This shouldn't happen");
        }
        else
        {
            var menu = new FavoritesMenu(config.Favorites);
            await menu.Run(AnsiConsole.Console, OpenUrl);
        }

        return 0;
    }
}
