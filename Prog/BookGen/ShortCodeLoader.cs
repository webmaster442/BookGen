﻿//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using BookGen.Interfaces.Configuration;

namespace BookGen;

internal sealed class ShortCodeLoader : IDisposable
{
    [ImportMany]
    public List<ITemplateShortCode> Imports { get; }

    [Export(typeof(ILog))]
    private readonly ILog _log;

    [Export(typeof(IReadonlyRuntimeSettings))]
    private readonly IReadonlyRuntimeSettings _settings;

    [Export(typeof(IReadOnlyTranslations))]
    private readonly IReadOnlyTranslations _tranlsations;

    [Export(typeof(IAppSetting))]
    private readonly IAppSetting _appSetting;

    private CompositionContainer? _container;

    public ShortCodeLoader(ILog log, IReadonlyRuntimeSettings settings, IAppSetting appSetting)
    {
        _log = log;
        _settings = settings;
        _tranlsations = settings.Configuration.Translations;
        _appSetting = appSetting;

        var catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new TypeCatalog(typeof(ILog),
                                             typeof(IReadonlyRuntimeSettings),
                                             typeof(IAppSetting)));
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(ShortCodeLoader).Assembly));
        Imports = [];
        _container = new CompositionContainer(catalog);
    }

    public void LoadAll()
    {
        try
        {
            if (_container != null)
            {
                _container.ComposeParts(this);
                _container.SatisfyImportsOnce(this);
            }
        }
        catch (Exception ex)
        {
            _log.Critical(ex);
        }
    }

    public void Dispose()
    {
        if (_container != null)
        {
            _container.Dispose();
            _container = null;
        }
    }
}
