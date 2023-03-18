﻿//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.Composition;

namespace BookGen.Framework.Shortcodes;

[Export(typeof(ITemplateShortCode))]
public sealed class SriDependency : ITemplateShortCode
{
    private readonly ILog _log;
    private readonly IReadonlyRuntimeSettings _settings;

    public string Tag => nameof(SriDependency);

    public bool CanCacheResult => true;

    [ImportingConstructor]
    public SriDependency(ILog log, IReadonlyRuntimeSettings settings)
    {
        _log = log;
        _settings = settings;
    }

    private string ComputeSRI(FsPath filePath)
    {
        _log.Detail("Computing SRI and caching results for {0}...", filePath);
        string sri = CryptoUitils.GetSRI(filePath);
        return sri;
    }

    public string Generate(IArguments arguments)
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
}