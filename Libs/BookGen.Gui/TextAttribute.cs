//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui;

[AttributeUsage(AttributeTargets.Field)]
public sealed class TextAttribute : Attribute
{
    public string Id { get; }

    public TextAttribute(string id)
    {
        Id = id;
    }
}
