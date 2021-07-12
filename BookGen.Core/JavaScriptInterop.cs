//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;
using Microsoft.ClearScript.V8;
using System;

namespace BookGen.Core
{
    public sealed class JavaScriptInterop : IDisposable
    {
        private V8ScriptEngine? _engine;

        public JavaScriptInterop()
        {
            _engine = new V8ScriptEngine();
            _engine.Execute(ResourceHandler.GetFile(KnownFile.PrismJs));
        }

        private void InlineCss()
        {

        }

        public string SyntaxHighlight(string code, string language)
        {
            if (_engine != null)
            {
                _engine.Script.code = code;
                var result = _engine?.Evaluate($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
                return result as string ?? string.Empty;
            }
            return string.Empty;
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
