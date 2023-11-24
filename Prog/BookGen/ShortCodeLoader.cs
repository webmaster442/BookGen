//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using BookGen.ShortCodes;
using BookGen.Interfaces.Configuration;
using System.Reflection;
using System;

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

    [Export(typeof(TimeProvider))]
    private readonly TimeProvider _timeProvider;

    private CompositionContainer? _container;

    public ShortCodeLoader(ILog log, IReadonlyRuntimeSettings settings, IAppSetting appSetting, TimeProvider timeProvider)
    {
        _log = log;
        _settings = settings;
        _tranlsations = settings.Configuration.Translations;
        _appSetting = appSetting;
        _timeProvider = timeProvider;

        var catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new TypeCatalog(typeof(ILog),
                                             typeof(IReadonlyRuntimeSettings),
                                             typeof(IAppSetting),
                                             typeof(TimeProvider)));

        catalog.Catalogs.Add(new AssemblyCatalog(typeof(BuiltInShortCodeAttribute).Assembly));
        foreach (var plugin in LoadPlugins(_log))
        {
            catalog.Catalogs.Add(plugin);
        }

        Imports = [];
        _container = new CompositionContainer(catalog);
    }

    private static List<AssemblyCatalog> LoadPlugins(ILog log)
    {
        log.Detail("Searching for plugins...");
        var folder = Path.Combine(AppContext.BaseDirectory, "ShortCodes");
        if (!Directory.Exists(folder))
        {
            log.Warning("ShortCodes folder doesn't exist. Skipping custom shortcode loading");
            return [];
        }

        string[] files = Directory.GetFiles(folder, "*.dll", SearchOption.TopDirectoryOnly);

        List<AssemblyCatalog> catalogs = new(files.Length);
        foreach (var file in files)
        {
            log.Detail("Loading {0}...", file);
            try
            {
                var assembly = Assembly.LoadFile(file);
                catalogs.Add(new AssemblyCatalog(assembly));
            }
            catch (Exception ex)
            {
                log.Warning("Load failed: {0}", ex.Message);
            }
        }

        return catalogs;
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
