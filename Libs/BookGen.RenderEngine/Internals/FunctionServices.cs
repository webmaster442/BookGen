using BookGen.Api;
using BookGen.Interfaces;

namespace BookGen.RenderEngine.Internals;

internal class FunctionServices
{
    public required ILog Log { get; init; }
    public required IAppSetting AppSetting { get; init; }
    public required TimeProvider TimeProvider { get; init; }
}
