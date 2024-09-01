//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO.Compression;

using BookGen.CommandArguments;
using BookGen.Framework;
using BookGen.Gui;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("pack")]
internal class PackCommand : Command<PackArguments>
{
    private readonly ILogger _log;
    private readonly ProgramInfo _programInfo;

    public PackCommand(ILogger log, ProgramInfo programInfo)
    {
        _log = log;
        _programInfo = programInfo;
    }

    public override int Execute(PackArguments arguments, string[] context)
    {
        _programInfo.EnableVerboseLogingIfRequested(arguments);

        ProjectLoader loader = new(arguments.Directory, _log, _programInfo);

        List<string> filesToPack = [];

        if (!loader.LoadProject())
        {
            return Constants.GeneralError;
        }

        //files
        filesToPack.AddRange(loader.Toc.Files.Select(f => Path.Combine(arguments.Directory, f)));
        //images
        filesToPack.AddRange(Directory.GetFiles(Path.Combine(arguments.Directory, loader.Configuration.ImageDir), "*.*", SearchOption.AllDirectories));
        //Index
        filesToPack.Add(Path.Combine(arguments.Directory, loader.Configuration.Index));
        //Summary
        filesToPack.Add(Path.Combine(arguments.Directory, loader.Configuration.TOCFile));
        //project files
        var projectFiles = ProjectFilesLocator.Locate(new FsPath(arguments.Directory));

        projectFiles.AddToPackListIfExist(filesToPack);

        ConsoleProgressbar progressbar = new(0, filesToPack.Count, !_programInfo.JsonLogging);

        try
        {
            int index = 1;
            progressbar.SwitchBuffers();
            using (var stream = File.Create(arguments.OutputFile))
            {
                using (ZipArchive archive = new(stream, ZipArchiveMode.Create))
                {
                    foreach (var file in filesToPack)
                    {
                        ++index;
                        string entryName = Path.GetRelativePath(arguments.Directory, file);
                        archive.CreateEntryFromFile(file, entryName, CompressionLevel.Optimal);
                        progressbar.Report(index, "Packing: {0}", entryName);
                    }
                }
            }
            progressbar.SwitchBuffers();
        }
        catch (Exception ex)
        {
            progressbar.SwitchBuffers();
            _log.LogCritical(ex, "Critical Error");
        }

        _log.LogInformation("Pack finished. Result file: {file}", arguments.OutputFile);
        
        return Constants.Succes;
    }
}
