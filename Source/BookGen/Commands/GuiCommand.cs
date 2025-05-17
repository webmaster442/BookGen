using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

namespace BookGen.Commands;

[CommandName("gui")]
internal class GuiCommand : Command<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _fileSystem;

    public GuiCommand(IWritableFileSystem writableFileSystem)
    {
        _fileSystem = writableFileSystem;
    }

    public override int Execute(BookGenArgumentBase arguments, string[] context)
    {
        _fileSystem.Scope = arguments.Directory;

        return ExitCodes.Succes;
    }
}
