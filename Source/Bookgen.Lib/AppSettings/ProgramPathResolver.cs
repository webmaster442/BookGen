//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Bookgen.Lib.Domain.IO;

namespace Bookgen.Lib.AppSettings;

public sealed class ProgramPathResolver : IProgramPathResolver
{
    private readonly IReadOnlyAppSettings _appSettings;

    private const string NodeJsExecutableName = "node";
    private const string PythonExecutableName = "python";
    private const string RatexExecutableName = "ratex-svg";
    private const string MmdrExecutableName = "mmdr";
    private const string PlantUmlExecutableName = "plantuml";

    public ProgramPathResolver(IReadOnlyAppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    private string? Resolve(Expression<Func<AppSetting, string?>> selector, string binaryName)
    {
        // if App settings contains a valid path, return it
        string? setting = _appSettings.Get<string?>(selector.Compile());
        if (setting != null && _appSettings.IsSettingValid<string?>(selector, out _))
        {
            return setting;
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
        };

        return null;
    }

    public bool TryResolvePythonPath([NotNullWhen(true)] out string? path)
    {
        path = Resolve(x => x.PythonPath, PythonExecutableName);
        return path != null;
    }

    public bool TryResolveNodeJsPath([NotNullWhen(true)] out string? path)
    {
        path = Resolve(x => x.NodeJsPath, NodeJsExecutableName);
        return path != null;
    }

    public bool TryResolveRatex([NotNullWhen(true)] out string? path)
    {
        path = Resolve(x => x.RatexPath, RatexExecutableName);
        return path != null;
    }

    public bool TryResolveMmdr([NotNullWhen(true)] out string? path)
    {
        path = Resolve(x => x.MmdrPath, MmdrExecutableName);
        return path != null;
    }

    public bool TryResolvePlantUml([NotNullWhen(true)] out string? path)
    {
        path = Resolve(x => x.PlantUmlPath, PlantUmlExecutableName);
        return path != null;
    }
}
