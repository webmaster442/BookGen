//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookGen.Framework.Scripts
{
    public class ScriptHandler
    {
        private readonly FsPath _scriptDir;
        private readonly ILog _log;
        private readonly Compiler _compiler;
        private readonly List<IScript> _scripts;

        public ScriptHandler(FsPath scripts, ILog log)
        {
            _scriptDir = scripts;
            _log = log;
            _compiler = new Compiler(log);
            _compiler.AddTypeReference<IScript>();
            _compiler.AddTypeReference<IReadonlyRuntimeSettings>();
            _scripts = new List<IScript>();
        }

        public void LoadScripts()
        {
            try
            {
                IEnumerable<FsPath> files = _scriptDir.GetAllFiles();
                IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> trees = _compiler.ParseToSyntaxTree(files);

                Assembly? assembly = _compiler.CompileToAssembly(trees);
                if (assembly != null)
                {
                    SearchAndAddTypes(assembly);
                }

            }
            catch (Exception ex)
            {
                _log.Warning(ex);
            }

        }

        public bool TryExecuteScript(string name, IReadonlyRuntimeSettings settings, out string result)
        {
            try
            {
                IScript? script = _scripts.FirstOrDefault(s => string.Compare(s.InvokeName, name, true) == 0);
                if (script == null)
                {
                    result = string.Empty;
                    return false;
                }

                result = script.ScriptMain(settings, _log);
                return true;
            }
            catch (Exception ex)
            {
                _log.Warning(ex);
                result = string.Empty;
                return false;
            }
        }

        private void SearchAndAddTypes(Assembly assembly)
        {
            var iscript = typeof(IScript);

            foreach (var IScriptType in assembly.GetTypes().Where(x => iscript.IsAssignableFrom(x)))
            {
                try
                {
                    if (Activator.CreateInstance(IScriptType) is IScript instance)
                    {
                        _scripts.Add(instance);
                    }
                }
                catch (Exception ex)
                {
                    _log.Warning(ex);
                }
            }
        }
    }
}
