//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace BookGen.Template.ShortCodeImplementations
{
    [Export(typeof(ITemplateShortCode))]
    public class SriDependency : ITemplateShortCode
    {
        private readonly ILog _log;
        private readonly IReadonlyRuntimeSettings _settings;

        public string Tag => nameof(SriDependency);

        private readonly Dictionary<FsPath, string> _cache;
        private DateTime _lastAcces;

        [ImportingConstructor]
        public SriDependency(ILog log, IReadonlyRuntimeSettings settings)
        {
            _log = log;
            _settings = settings;
            _cache = new Dictionary<FsPath, string>();
            _lastAcces = DateTime.Now;
        }

        private string GetOrCreateSriForFile(FsPath filePath)
        {
            if ((DateTime.Now - _lastAcces).TotalMilliseconds > 5000)
            {
                _log.Detail("Clearing SRI Cache, because it's older than 5 seconds");
                _cache.Clear();
            }

            if (_cache.ContainsKey(filePath))
            {
                _log.Detail("SRI restored from cache for: {0}", filePath);
                _lastAcces = DateTime.Now;
                return _cache[filePath];
            }
            else
            {
                _log.Detail("Computing SRI and caching results for {0}...", filePath);
                string sri = HashUtils.GetSRI(filePath);
                _cache.Add(filePath, sri);
                _lastAcces = DateTime.Now;
                return sri;
            }
        }


        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            var file = arguments.GetArgumentOrThrow("file");

            var path = _settings.OutputDirectory.Combine(file);

            file = _settings.Configuration.HostName + file;

            var sri = GetOrCreateSriForFile(path);

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
}