//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Contracts;
using System.ComponentModel.Composition;

namespace BookGen.Framework.Scripts
{
    [Export(typeof(ITemplateShortCode))]
    internal sealed class NodeJsHost : ProcessHost, ITemplateShortCode
    {
        private readonly IReadonlyRuntimeSettings _settings;

        [ImportingConstructor]
        public NodeJsHost(ILog log, IReadonlyRuntimeSettings settings) : base(log)
        {
            _settings = settings;
        }

        protected override string ProcessFileName => ProcessInterop.AppendExecutableExtension("node");

        protected override string ProcessArguments => string.Empty;

        protected override string SerializeHostInfo(ScriptHost host)
        {
            return JsonInliner.InlineJs(nameof(host), host, _log);
        }

        public string Tag => "NodeJs";

        public bool CanCacheResult => false;

        public string Generate(IArguments arguments)
        {
            var file = new FsPath(arguments.GetArgumentOrThrow<string>("file"));
            return base.Execute(file, new ScriptHost(_settings, _log));
        }
    }
}
