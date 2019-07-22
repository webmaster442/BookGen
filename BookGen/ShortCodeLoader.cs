//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template.ShortCodeImplementations;
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
        public List<ITemplateShortCode> Imports { get; private set; }

        private readonly ILog _log;
        private readonly CompositionContainer _container;

        public ShortCodeLoader(ILog log)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(DelegateShortCode).Assembly));
            Imports = new List<ITemplateShortCode>();
            _log = log;
            _container = new CompositionContainer(catalog);
        }

        public void LoadAll()
        {
            try
            {
                _container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                _log.Critical(ex);
            }
        }
    }
}
