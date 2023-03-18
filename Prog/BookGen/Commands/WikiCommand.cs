namespace BookGen.Commands;

[CommandName("wiki")]
internal class WikiCommand : Command
{
    public override int Execute(string[] context)
    {
        return UrlOpener.OpenUrl(Constants.WikiUrl) ? Constants.Succes : Constants.GeneralError;
    }
}
