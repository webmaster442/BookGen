//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.DomainServices;
using BookGen.Interfaces;
using BookGen.RenderEngine.Internals;

namespace BookGen.RenderEngine.Functions;
internal sealed class SriDependency : Function, IInjectable
{
    private IReadonlyRuntimeSettings _settings = null!;
    private ILog _log = null!;

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
            _log.Detail("Creating SRI script tag for: {0}", path);
            return $"<script src=\"{file}\" integrity=\"{sri}\" crossorigin=\"anonymous\"></script>";
        }
        else if (path.Extension == ".css")
        {
            _log.Detail("Creating SRI css tag for: {0}", path);
            return $"<link rel=\"stylesheet\" href=\"{file}\" integrity=\"{sri}\" crossorigin=\"anonymous\"/>";
        }
        else
        {
            _log.Warning("Unsupprted file type for SRI linking: {0}", path.Extension);
        }

        return string.Empty;
    }
    private string ComputeSRI(FsPath filePath)
    {
        _log.Detail("Computing SRI and caching results for {0}...", filePath);
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
