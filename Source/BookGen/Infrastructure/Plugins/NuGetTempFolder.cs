//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO.Compression;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Plugins;

internal sealed class NuGetTempFolder : IDisposable
{
    private readonly string _tempPath;
    private readonly string _nuGetPackagePath;

    public NuGetTempFolder(string nuGetPackagePath)
    {
        string name = Path.GetFileNameWithoutExtension(nuGetPackagePath);
        int randomId = Random.Shared.Next();
        _tempPath = Path.Combine(Path.GetTempPath(), $"{name}_{randomId}");
        if (!Directory.Exists(_tempPath))
        {
            Directory.CreateDirectory(_tempPath);
        }

        _nuGetPackagePath = nuGetPackagePath;
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempPath))
        {
            Directory.Delete(_tempPath, true);
        }
    }

    public string GetFrameworkPath(string frameworkMoniker)
        => Path.Combine(_tempPath, "lib", frameworkMoniker);

    internal IEnumerable<string> GetDllFiles(string frameworkMoniker)
    {
        var probePath = Path.Combine(_tempPath, "lib", frameworkMoniker);

        return Directory.Exists(probePath)
            ? Directory.GetFiles(probePath, "*.dll", SearchOption.TopDirectoryOnly) 
            : Array.Empty<string>();
    }

    public static implicit operator string(NuGetTempFolder tempFolder)
        => tempFolder._tempPath;
}
