//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Http;

internal sealed class LoggerProvider : ILoggerProvider
{
    private readonly ILogger _logger;

    public LoggerProvider(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger CreateLogger(string categoryName)
        => _logger;

    public void Dispose() { }
}
