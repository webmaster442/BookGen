//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Bookgen.Lib.JsInterop;

public abstract class JavascriptInterop : IDisposable
{
    protected readonly V8ScriptEngine _engine;
    private bool _disposed;

    public JavascriptInterop()
    {
        _engine = new V8ScriptEngine();
    }

    ~JavascriptInterop()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_engine));

        _engine.Dispose();
        _disposed = true;
    }

    protected void Execute(string code)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_engine));

        _engine.Execute(code);
    }

    protected object Evaluate(string code)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(_engine));

        return _engine.Evaluate(code);
    }

    protected string ExecuteAndGetResult(string code)
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
