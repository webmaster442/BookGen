//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;
using Microsoft.ClearScript.V8;
using System;
using PreMailer.Net;

namespace BookGen.Core
{
    public sealed class JavaScriptInterop : IDisposable
    {
        private V8ScriptEngine? _engine;
        private readonly string _prismcss;

        public JavaScriptInterop()
        {
            _engine = new V8ScriptEngine();
            _engine.Execute(ResourceHandler.GetFile(KnownFile.PrismJs));
            _prismcss = ResourceHandler.GetFile(KnownFile.PrismCss);
        }

        private string InlineCss(string? soruce)
        {
            if (!string.IsNullOrEmpty(soruce))
            {
                var pm = new PreMailer.Net.PreMailer(soruce);
                var result = pm.MoveCssInline(false, string.Empty, _prismcss);
                return result.Html;
            }
            return string.Empty;
        }

        public string SyntaxHighlight(string code, string language)
        {
            if (_engine != null)
            {
                _engine.Script.code = code;
                var result = _engine?.Evaluate($"Prism.highlight(code, Prism.languages.{language}, '{language}');");
                return InlineCss(result as string);
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
