//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.DomainServices;
using System.IO;

namespace BookGen.Framework.Scripts
{
    public abstract class ScriptProcess
    {
        protected ILog _log;

        protected ScriptProcess(ILog log)
        {
            _log = log;
        }

        protected string ExecuteScriptProcess(string programWithoutExtension, string searchPath, string fileToExecute, int timeout)
        {
            string program = ProcessInterop.AppendExecutableExtension(programWithoutExtension);
            string? programPath = ProcessInterop.ResolveProgramFullPath(program, searchPath);

            if (programPath == null)
            {
                _log.Warning("{0} was not found on path.", program);
                return $"{program} was not found on path";
            }

            string? scriptPath = Path.GetDirectoryName(fileToExecute);

            if (scriptPath != null)
                SerializeHostInfo(scriptPath);

            try
            {
                var (exitcode, output) = ProcessRunner.RunProcess(programPath, fileToExecute, timeout);

                if (exitcode != 0)
                {
                    _log.Warning("Script run failed. Exit code: {0}", exitcode);
                    _log.Detail("Script output: {0}", output);
                    return $"Script run failed: {fileToExecute}";
                }
                else
                {
                    return output;
                }

            }
            catch (Exception ex)
            {
                _log.Warning("Script run failed with Exception: {0}", ex.Message);
                _log.Detail("Stack Trace: {0}", ex.StackTrace ?? "");
                return $"Script run failed with Exception: {ex.Message}";
            }
        }

        protected abstract void SerializeHostInfo(string scriptPath);
    }
}
