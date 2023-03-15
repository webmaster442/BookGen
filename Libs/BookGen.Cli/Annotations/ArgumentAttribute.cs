namespace BookGen.Cli.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ArgumentAttribute : Attribute
    {
        public int Index { get; }

        public ArgumentAttribute(int index) 
        {
            Index = index;
        }
    }
}
