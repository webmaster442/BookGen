// See https://aka.ms/new-console-template for more information
using Bang.Internals;

using BookGen.Cli;

var bangs = BangLoader.Load();
var arguments = new ArgumentParser<Arguments>().Parse(args);
var validation = arguments.Validate();

if (!validation.IsOk)
{
    Console.WriteLine("Error: ");
    Console.WriteLine(validation);
    return;
}

if (arguments.ListBangs)
{
    List("Known bangs", bangs.KnownBangs());
    return;
}

if (!bangs.TryGetSearchUrls(arguments.BangName, arguments.SearchTerm, out var urls))
{
    Console.WriteLine($"Unknown bang or alias: {arguments.BangName}");
    return;
}

if (arguments.ShowUrls)
    List("Urls", urls);
else
    OpenUrls(urls);

void OpenUrls(IReadOnlyList<string> urls)
{
    throw new NotImplementedException();
}

void List(string v, IEnumerable<string> urls)
{
    throw new NotImplementedException();
}
