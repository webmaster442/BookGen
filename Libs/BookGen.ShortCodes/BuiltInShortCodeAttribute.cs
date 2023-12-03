namespace BookGen.ShortCodes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class BuiltInShortCodeAttribute : Attribute
{
    public bool DisplayInHelp { get; set; } = true;
}
