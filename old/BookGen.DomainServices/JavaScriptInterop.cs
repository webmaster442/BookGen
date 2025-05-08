//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;
using Microsoft.ClearScript.V8;
using System.Diagnostics.CodeAnalysis;

namespace BookGen.DomainServices
{
    public sealed class JavaScriptInterop : IDisposable
    {
        private V8ScriptEngine? _engine;
        private bool _prismLoaded;

        public JavaScriptInterop()
        {
            _engine = new V8ScriptEngine();
            _prismLoaded = false;
        }

        [MemberNotNull(nameof(_engine))]
        private void InitWithScript(ref bool flag, KnownFile scriptFile)
        {
            if (_engine == null)
                throw new InvalidOperationException("After Dispose no operation is possible");

            if (!flag)
            {
                _engine.Execute(ResourceHandler.GetFile(scriptFile));
                flag = true;
            }
        }

        [MemberNotNull(nameof(_engine))]
        private string ExecuteAndGetResult(string code)
        {
            if (_engine == null)
                throw new InvalidOperationException("After Dispose no operation is possible");

            object? result = _engine.Evaluate(code);
            return result as string ?? string.Empty;
        }

        public string PrismSyntaxHighlight(string code, string language)
        {
            InitWithScript(ref _prismLoaded, KnownFile.PrismJs);
            _engine.Script.code = code;
            return ExecuteAndGetResult($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
        }

        public void Dispose()
        {
            if (_engine != null)
            {
                _engine.Dispose();
                _engine = null;
            }
        }
    }
}
