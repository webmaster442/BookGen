//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.TaskRunner;

public sealed class ShellCommand : TaskItem
{
    [XmlAttribute]
    public ShellType ShellType { get; init; }
    [XmlAttribute]
    public string Command { get; init; }
    [XmlAttribute]
    public string ConditionVariable { get; init; }

    public ShellCommand()
    {
        Command = string.Empty;
        ConditionVariable = string.Empty;
    }
}
