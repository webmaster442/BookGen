namespace BookGen.CommandArguments;

internal sealed class TerminalInstallArguments : ArgumentsBase
{
    [Switch("c", "checkinstall")]
    public bool CheckInstall { get; set; }

    [Switch("t", "checkterminalinstall")]
    public bool CheckTerminalInstall { get; set; }
}
