//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using BookGen.Cli.Dotenv;
using BookGen.Lib;

namespace BookGen.Infrastructure;

public sealed class ProgramPathResolver : IProgramPathResolver
{
    private readonly DotEnvSettings _appSettings;

    private const string NodeJsExecutableName = "node";
    private const string PythonExecutableName = "python";
    private const string RatexExecutableName = "ratex-svg";
    private const string MmdrExecutableName = "mmdr";
    private const string PlantUmlExecutableName = "plantuml";

    private const string NodeJsKey = "NodeJsPath";
    private const string PythonKey = "PythonPath";
    private const string RatexKey = "RatexPath";
    private const string MmdrKey = "MmdrPath";
    private const string PlantUmlKey = "PlantUmlPath";

    public ProgramPathResolver(DotEnvSettings appSettings)
    {
        _appSettings = appSettings;
    }

    private string? Resolve(string key, string binaryName)
    {
        string settingValue = _appSettings.GetValueOrDefault(key, string.Empty);
        if (!string.IsNullOrEmpty(settingValue))
        {
            var fullPath = Path.GetFullPath(settingValue);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        //if App path contains a valid path, return it
        string appBinaryPath = OperatingSystem.IsWindows()
            ? Path.Combine(AppContext.BaseDirectory, $"{binaryName}.exe")
            : Path.Combine(AppContext.BaseDirectory, binaryName);


        if (File.Exists(appBinaryPath))
            return appBinaryPath;

        //try to find the binary in the system path

        string binaryToSearch = OperatingSystem.IsWindows() ? $"{binaryName}.exe" : binaryName;
        foreach (string path in Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? Array.Empty<string>())
        {
            string fullPath = Path.Combine(path, binaryToSearch);
            if (File.Exists(fullPath))
                return fullPath;
        }

        return null;
    }

    public bool TryResolvePythonPath([NotNullWhen(true)] out string? path)
    {
        path = Resolve(PythonKey, PythonExecutableName);
        return path != null;
    }

    public bool TryResolveNodeJsPath([NotNullWhen(true)] out string? path)
    {
        path = Resolve(NodeJsKey, NodeJsExecutableName);
        return path != null;
    }

    public bool TryResolveRatex([NotNullWhen(true)] out string? path)
    {
        path = Resolve(RatexKey, RatexExecutableName);
        return path != null;
    }

    public bool TryResolveMmdr([NotNullWhen(true)] out string? path)
    {
        path = Resolve(MmdrKey, MmdrExecutableName);
        return path != null;
    }

    public bool TryResolvePlantUml([NotNullWhen(true)] out string? path)
    {
        path = Resolve(PlantUmlKey, PlantUmlExecutableName);
        return path != null;
    }
}
