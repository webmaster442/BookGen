namespace BookGen.Cli.Annotations;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal sealed class ExitCodeAttribute : Attribute
{
    public int ExitCode { get; }
    public string Description { get; }

    public ExitCodeAttribute(int exitCode, string description)
    {
        ExitCode = exitCode;
        Description = description;
    }

}
