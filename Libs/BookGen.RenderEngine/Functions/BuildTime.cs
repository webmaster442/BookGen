using BookGen.RenderEngine.Internals;

namespace BookGen.RenderEngine.Functions;

internal sealed class BuildTime : Function, IInjectable
{
    internal TimeProvider Provider { get; set; } = null!;


    public override string Execute(FunctionArguments arguments)
    {
        var format = arguments.GetArgumentOrFallback("format", "yy-MM-dd hh:mm:ss");
        return Provider.GetLocalNow().ToString(format);
    }

    public void Inject(FunctionServices functionServices)
    {
        Provider = functionServices.TimeProvider;
    }

    protected override FunctionInfo GetInformation()
    {
        return new FunctionInfo
        {
            Name = "BuildTime",
            Description = "Inserts the current build time",
            CanCacheResult = false,
            ArgumentInfos =
            [
                new Internals.ArgumentInfo
                {
                    Name = "format",
                    Optional = true,
                    Description = "The format of the time. Default is yy-MM-dd hh:mm:ss"
                }
            ]
        };
    }
}
