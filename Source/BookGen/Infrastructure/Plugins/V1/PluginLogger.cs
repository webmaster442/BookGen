using System.Diagnostics.CodeAnalysis;

using BookGen.Api.V1;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Plugins.V1;

internal sealed class PluginLogger(ILogger logger) : IPluginLogger
{
    public void LogCritical(Exception? exception, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? message, params object?[] args)
        => logger.LogCritical(exception, message, args);

    public void LogCritical([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? message, params object?[] args)
        => logger.LogCritical(message, args);

    public void LogDebug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? message, params object?[] args)
        => logger.LogDebug(message, args);

    public void LogError([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? message, params object?[] args)
        => logger.LogError(message, args);

    public void LogInformation([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? message, params object?[] args)
        => logger.LogInformation(message, args);

    public void LogWarning(Exception? exception, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? message, params object?[] args)
        => logger.LogWarning(exception, message, args);

    public void LogWarning([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? message, params object?[] args)
        => logger.LogWarning(message, args);
}
