//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.TaskRunner;

public sealed class Confirm : TaskItem
{
    [XmlAttribute]
    public string Message { get; init; }

    [XmlAttribute]
    public string Varialbe { get; init; }

    public Confirm()
    {
        Message = string.Empty;
        Varialbe = string.Empty;
    }
}
