//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.TaskRunner;

public sealed class ShellCommands : TaskItem
{
    [XmlAttribute]
    public ShellType ShellType { get; init; }
    [XmlAttribute]
    public string ConditionVariable { get; init; }
    [XmlText]
    public string Commands { get; init; }

    public ShellCommands()
    {
        ConditionVariable = string.Empty;
        Commands= string.Empty;
    }
}