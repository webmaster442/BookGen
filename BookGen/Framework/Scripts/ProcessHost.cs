//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using System.Text;

namespace BookGen.Framework.Scripts
{
    internal abstract class ProcessHost
    {
        public abstract string ProcessFileName { get; }
        public abstract string ProcessArguments { get; }
        public virtual int Timeout => 5000;
        public virtual int ExpectedExitCode => 0;

        protected readonly ILog _log;

        public abstract string SerializeHostInfo(ScriptHost host);

        protected ProcessHost(ILog log)
        {
            _log = log;
        }

        public string Execute(FsPath scriptFile, ScriptHost host)
        {
            _log.Info("Trying to execute {0}", scriptFile);

            string? programPath = ProcessInterop.ResolveProgramFullPath(ProcessFileName);

            if (programPath == null)
            {
                _log.Warning("{0} was not found on path.", ProcessFileName);
                return $"{ProcessFileName} was not found on path";
            }

            StringBuilder stdin = new StringBuilder(16 * 1024);
            stdin.AppendLine(SerializeHostInfo(host));
            stdin.AppendLine(scriptFile.ReadFile(_log));

            var task = ProcessInterop.RunProcess(programPath, ProcessArguments, stdin.ToString(), Timeout);

            task.Wait();
            
            if (task.Result.exitcode == ExpectedExitCode)
            {
                return task.Result.output;
            }
            else
            {
                _log.Warning("Script run failed. Exit code: {0}", task.Result.exitcode);
                _log.Detail("Script output: {0}", task.Result.output);
                return $"Script run failed: {scriptFile}";
            }
        }
    }
}
