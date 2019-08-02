//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bookgen.Template.ShortCodeImplementations
{
    [Export(typeof(ITemplateShortCode))]
    public class SriDependency : ITemplateShortCode
    {
        private readonly ILog _log;
        private readonly IReadonlyRuntimeSettings _settings;

        public string Tag => nameof(SriDependency);

        [ImportingConstructor]
        public SriDependency(ILog log, IReadonlyRuntimeSettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            var file = arguments.GetArgumentOrThrow("file");

            var path = _settings.OutputDirectory.Combine(file);

            file = _settings.Configuration.HostName + file;

            var sri = SriGenerator.GetSRI(path);

            if (path.Extension == ".js")
            {
                return $"<script src=\"{file}\" integrity=\"{sri}\" crossorigin=\"anonymous\"></script>";
            }
            else if (path.Extension == ".css")
            {
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