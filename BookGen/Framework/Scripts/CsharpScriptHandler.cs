//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookGen.Framework.Scripts
{
    /// <summary>
    /// Handles script loading, compiling, and executing
    /// </summary>
    internal class CsharpScriptHandler
    {
        private readonly ILog _log;
        private IScriptHost? _host;
        private readonly Compiler _compiler;
        private readonly List<IScript> _scripts;
        private readonly HashSet<string> _scriptNames;


        public CsharpScriptHandler(ILog log)
        {
            _log = log;
            _compiler = new Compiler(log);
            _compiler.AddTypeReference<IReadOnlyDictionary<string,string>>();
            _compiler.AddTypeReference<IScript>();
            _compiler.AddTypeReference<IScriptHost>();
            _scripts = new List<IScript>();
            _scriptNames = new HashSet<string>();
        }

        public int LoadScripts(FsPath scriptsDir)
        {
            try
            {
                var files = scriptsDir.GetAllFiles("*.cs");
                IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> trees = _compiler.ParseToSyntaxTree(files);

                Assembly? assembly = _compiler.CompileToAssembly(trees);
                if (assembly != null)
                {
                    int count = SearchAndAddTypes(assembly);
                    return count;
                }

                return 0;
            }
            catch (Exception ex)
            {
                _log.Warning(ex);
                return 0;
            }

        }

        public bool IsKnownScript(string name)
        {
            return _scriptNames.Contains(name);
        }

        public void SetHostFromRuntimeSettings(IReadonlyRuntimeSettings runtimeSettings)
        {
            _host = new ScriptHost(runtimeSettings, _log);
        }

        public string ExecuteScript(string name, IArguments arguments)
        {
            if (_host == null)
                throw new DependencyException(nameof(_host));

            try
            {
                IScript? script = _scripts.FirstOrDefault(s => string.Compare(s.InvokeName, name, true) == 0);
                if (script == null)
                {
                    _log.Warning("Script not found: {0}", name);
                    return string.Empty;
                }

                return script.ScriptMain(_host, arguments);
            }
            catch (Exception ex)
            {
                _log.Warning(ex);
                return string.Empty;
            }
        }

        private int SearchAndAddTypes(Assembly assembly)
        {
            var iscript = typeof(IScript);
            int loaded = 0;
            foreach (var IScriptType in assembly.GetTypes().Where(x => iscript.IsAssignableFrom(x)))
            {
                try
                {
                    if (Activator.CreateInstance(IScriptType) is IScript instance)
                    {
                        _scripts.Add(instance);
                        _scriptNames.Add(instance.InvokeName);
                        ++loaded;
                    }
                }
                catch (Exception ex)
                {
                    _log.Warning(ex);
                }
            }
            return loaded;
        }
    }
}
