//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

using BookGen.Api;
using BookGen.DomainServices;

namespace BookGen.RenderEngine.Internals;

internal sealed class ScriptProcess
{
    private readonly ILog _log;

    public ScriptProcess(ILog log)
    {
        _log = log;
    }

    private static string? ResolveProgramFullPath(string programName, string additional = "")
    {
        string? pathVar = Environment.GetEnvironmentVariable("path");

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
            _log.Warning("{0} was not found on path.", program);
            return $"{program} was not found on path";
        }

        try
        {
            (int exitcode, string output) = ProcessRunner.RunProcess(programPath, fileToExecute, timeout);

            if (exitcode != 0)
            {
                _log.Warning("Script run failed. Exit code: {0}", exitcode);
                _log.Detail("Script output: {0}", output);
                return $"Script run failed: {fileToExecute}";
            }
            else
            {
                return output;
            }
        }
        catch (Exception ex)
        {
            _log.Warning("Script run failed with Exception: {0}", ex.Message);
            _log.Detail("Stack Trace: {0}", ex.StackTrace ?? "");
            return $"Script run failed with Exception: {ex.Message}";
        }
    }
}
