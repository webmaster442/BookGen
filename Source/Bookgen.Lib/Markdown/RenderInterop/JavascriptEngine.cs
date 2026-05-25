using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Bookgen.Lib.Markdown.RenderInterop;

internal sealed class JavascriptEngine : IDisposable
{
    private readonly V8ScriptEngine _engine;
    private bool _disposed;

    public JavascriptEngine()
    {
        _engine = new V8ScriptEngine();
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
