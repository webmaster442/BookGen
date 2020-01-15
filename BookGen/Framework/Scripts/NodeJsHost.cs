//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Contracts;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;

namespace BookGen.Framework.Scripts
{
    [Export(typeof(ITemplateShortCode))]
    internal sealed class NodeJsHost : ITemplateShortCode
    {
        private readonly IReadonlyRuntimeSettings _settings;
        private readonly ILog _log;

        private const string processName = "node";
        private const int ScriptTimeOut = 10000;

        [ImportingConstructor]
        public NodeJsHost(ILog log, IReadonlyRuntimeSettings settings)
        {
            _log = log;
            _settings = settings;
        }

        private string SerializeHostInfo()
        {
            ScriptHost host = new ScriptHost(_settings, _log);
            return JsonInliner.InlineJs(nameof(host), host, _log);
        }

        public string Tag => "NodeJs";

        public bool CanCacheResult => false;

        public string Generate(IArguments arguments)
        {
            var file = new FsPath(arguments.GetArgumentOrThrow<string>("file"));

            _log.Info("Trying to execute {0}", file);

            var nodeProgram = ProcessInterop.AppendExecutableExtension(processName);

            string? programPath = ProcessInterop.ResolveProgramFullPath(nodeProgram);

            if (programPath == null)
            {
                _log.Warning("{0} was not found on path.", processName);
                return $"{processName} was not found on path";
            }

            StringBuilder script = new StringBuilder(16 * 1024);
            script.AppendLine(SerializeHostInfo());
            script.AppendLine(file.ReadFile(_log));

            var temp = new FsPath(Path.GetTempFileName());
            temp.WriteFile(_log, script.ToString());

            var (exitcode, output) = ProcessInterop.RunProcess("node", temp.ToString(), ScriptTimeOut);

            if (temp.IsExisting)
                File.Delete(temp.ToString());

            if (exitcode != 0)
            {
                _log.Warning("Script run failed. Exit code: {0}", exitcode);
                _log.Detail("Script output: {0}", output);
                return $"Script run failed: {temp}";
            }
            else
            {
                return output;
            }
        }
    }
}
