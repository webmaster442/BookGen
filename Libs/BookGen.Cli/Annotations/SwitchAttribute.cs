namespace BookGen.Cli.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SwitchAttribute : Attribute
    {
        public string LongName { get; }
        public string ShortName { get; }

        public SwitchAttribute(string shortName, string longName)
        {
            LongName = longName;
            ShortName = shortName;
        }
    }
}
