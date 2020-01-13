//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Api.Configuration;
using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace BookGen
{
    public class ShortCodeLoader
    {
        [ImportMany]
        public List<ITemplateShortCode> Imports { get; }

        [Export(typeof(ILog))]
        private readonly ILog _log;

        [Export(typeof(IReadonlyRuntimeSettings))]
        private readonly IReadonlyRuntimeSettings _settings;

        [Export(typeof(IReadOnlyTranslations))]
        private readonly IReadOnlyTranslations _tranlsations;

        private readonly CompositionContainer _container;

        public ShortCodeLoader(ILog log, IReadonlyRuntimeSettings settings)
        {
            _log = log;
            _settings = settings;
            _tranlsations = settings.Configuration.Translations;

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(typeof(ILog), typeof(IReadonlyRuntimeSettings)));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ShortCodeLoader).Assembly));
            Imports = new List<ITemplateShortCode>();
            _container = new CompositionContainer(catalog);
        }

        public void LoadAll()
        {
            try
            {
                _container.ComposeParts(this);
                _container.SatisfyImportsOnce(this);
            }
            catch (Exception ex)
            {
                _log.Critical(ex);
            }
        }
    }
}
