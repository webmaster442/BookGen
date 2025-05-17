using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

namespace BookGen.Commands;

[CommandName("AddFrontMatter")]
internal class AddFrontMatter : Command<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;

    public AddFrontMatter(IWritableFileSystem writableFileSystem)
    {
        _writableFileSystem = writableFileSystem;
    }

    public override int Execute(BookGenArgumentBase arguments, string[] context)
    {
        _writableFileSystem.Scope = arguments.Directory;

        return ExitCodes.Succes;
    }
}
