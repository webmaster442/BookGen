//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

using BookGen.Shell.Shared;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Templates;

internal sealed class ScriptProcess
{
    private readonly ILogger _log;

    public ScriptProcess(ILogger log)
    {
        _log = log;
    }

    private static string? ResolveProgramFullPath(string programName, string additional = "")
    {
        string? pathVar = System.Environment.GetEnvironmentVariable("path");

        if (pathVar == null)
            return null;

        var searchFolders = new List<string>(20);

        if (AppDomain.CurrentDomain.BaseDirectory != null)
            searchFolders.Add(AppDomain.CurrentDomain.BaseDirectory);

        searchFolders.AddRange(pathVar.Split(';'));

        if (!string.IsNullOrEmpty(additional))
            searchFolders.Add(additional);

        foreach (string folder in searchFolders)
        {
            string programFile = Path.Combine(folder, programName);

            if (File.Exists(programFile))
            {
                return programFile;
            }
        }

        return null;
    }

    private static string AppendExecutableExtension(string file)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return file;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return $"{file}.exe";
        }
        else
        {
            throw new InvalidOperationException("Unknown host operating system");
        }
    }

    public string ExecuteScriptProcess(string programWithoutExtension,
                                       string searchPath,
                                       string fileToExecute,
                                       int timeout)
    {
        string program = AppendExecutableExtension(programWithoutExtension);
        string? programPath = ResolveProgramFullPath(program, searchPath);

        if (programPath == null)
        {
            _log.LogWarning("{program} was not found on path.", program);
            return $"{program} was not found on path";
        }

        try
        {
            (int exitcode, string output, _) = ProcessRunner.RunProcess(programPath, fileToExecute, timeout);

            if (exitcode != 0)
            {
                _log.LogWarning("Script run failed. Exit code: {exitcode}", exitcode);
                _log.LogDebug("Script output: {output}", output);
                return $"Script run failed: {fileToExecute}";
            }
            else
            {
                return output;
            }
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Script run failed with Exception.");
            return $"Script run failed with Exception: {ex.Message}";
        }
    }
}
