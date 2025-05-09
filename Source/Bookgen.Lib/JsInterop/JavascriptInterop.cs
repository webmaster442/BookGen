using Microsoft.ClearScript.V8;

namespace Bookgen.Lib.JsInterop;

internal abstract class JavascriptInterop : IDisposable
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
        if (_disposed)
            throw new ObjectDisposedException(nameof(_engine));

        _engine.Dispose();
        _disposed = true;
    }

    protected void Execute(string code)
    {
        _engine.Execute(code);
    }

    protected string ExecuteAndGetResult(string code)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(_engine));

        object? result = _engine.Evaluate(code);
        return result as string ?? string.Empty;
    }
}
