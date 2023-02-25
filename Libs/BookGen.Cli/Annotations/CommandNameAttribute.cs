namespace BookGen.Cli.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CommandNameAttribute : Attribute
    {
        public string Name { get; }

        public CommandNameAttribute(string name)
        {
            Name = name;
        }
    }
}
