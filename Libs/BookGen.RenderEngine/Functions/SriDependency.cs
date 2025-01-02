//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;
using BookGen.Interfaces;
using BookGen.RenderEngine.Internals;

using Microsoft.Extensions.Logging;

namespace BookGen.RenderEngine.Functions;
internal sealed class SriDependency : Function, IInjectable
{
    private IReadonlyRuntimeSettings _settings = null!;
    private ILogger _log = null!;

    public void Inject(FunctionServices functionServices)
    {
        _settings = functionServices.RuntimeSettings;
        _log = functionServices.Log;
    }

    public override string Execute(FunctionArguments arguments)
    {
        string? file = arguments.GetArgumentOrThrow<string>("file");

        FsPath? path = _settings.OutputDirectory.Combine(file);

        file = _settings.Configuration.HostName + file;

        string? sri = ComputeSRI(path);

        if (path.Extension == ".js")
        {
            _log.LogDebug("Creating SRI script tag for: {path}", path);
            return $"<script src=\"{file}\" integrity=\"{sri}\" crossorigin=\"anonymous\"></script>";
        }
        else if (path.Extension == ".css")
        {
            _log.LogDebug("Creating SRI css tag for: {path}", path);
            return $"<link rel=\"stylesheet\" href=\"{file}\" integrity=\"{sri}\" crossorigin=\"anonymous\"/>";
        }
        else
        {
            _log.LogWarning("Unsupprted file type for SRI linking: {extension}", path.Extension);
        }

        return string.Empty;
    }
    private string ComputeSRI(FsPath filePath)
    {
        _log.LogDebug("Computing SRI and caching results for {filePath}...", filePath);
        return CryptoUitils.GetSRI(filePath);
    }

    protected override FunctionInfo GetInformation()
    {
        return new FunctionInfo
        {
            Name = "SriDependency",
            Description = "Creates a script or link tag with SRI hash",
            ArgumentInfos = new ArgumentInfo[]
            {
                new()
                {
                    Name = "file",
                    Description = "css or js file name",
                    Optional = false,
                }
            }
        };
    }
}
