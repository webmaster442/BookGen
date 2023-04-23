//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.TaskRunner;

public sealed class InputPrompt : TaskItem
{
    [XmlAttribute]
    public string Message { get; init; }

    [XmlAttribute]
    public string Varialbe { get; init; }

    public InputPrompt()
    {
        Message = string.Empty;
        Varialbe = string.Empty;
    }
}
