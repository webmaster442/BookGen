using BookGen.Interfaces;

using Microsoft.Extensions.Logging;

namespace BookGen.RenderEngine;

public sealed class FunctionServices
{
    public required ILogger Log { get; init; }
    public required IAppSetting AppSetting { get; init; }
    public required TimeProvider TimeProvider { get; init; }
    public required IReadonlyRuntimeSettings RuntimeSettings { get; init; }
}
