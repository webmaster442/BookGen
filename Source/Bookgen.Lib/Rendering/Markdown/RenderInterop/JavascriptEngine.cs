//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Vfs;

using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Bookgen.Lib.Rendering.Markdown.RenderInterop;

internal sealed class JavascriptEngine : IDisposable
{
    private readonly V8ScriptEngine _engine;
    private bool _disposed;

    public JavascriptEngine(IAssetSource? moduleAssets)
    {
        _engine = new V8ScriptEngine();
        _engine.DocumentSettings.Loader = new JavascriptModuleLoader(moduleAssets);
        _engine.DocumentSettings.SearchPath = "/";
    }

    public void Dispose()
    {
        _engine.Dispose();
        _disposed = true;
    }

    public dynamic Script => _engine.Script;

    public void Execute(string code)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_engine));

        _engine.Execute(code);
    }

    public object Evaluate(string code)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_engine));

        return _engine.Evaluate(code);
    }

    public string ExecuteAndGetResult(string code)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_engine));

        object? result = _engine.Evaluate(code);

        if (result is ScriptObject) //if the result is a promise, we need to wait for it
        {
            var tsk = result.ToTask();
            result = tsk.GetAwaiter().GetResult();
        }

        return result as string
            ?? throw new InvalidOperationException($"Expected result to be a string but was: {result.GetType()}");
    }
}
