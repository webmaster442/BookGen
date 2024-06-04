using BookGen.Api;
using BookGen.Interfaces;

namespace BookGen.RenderEngine;

public sealed class FunctionServices
{
    public required ILog Log { get; init; }
    public required IAppSetting AppSetting { get; init; }
    public required TimeProvider TimeProvider { get; init; }
    public required IReadonlyRuntimeSettings RuntimeSettings { get; init; }
}
