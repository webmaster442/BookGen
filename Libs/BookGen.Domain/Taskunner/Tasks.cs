//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.TaskRunner;

public class Tasks
{
    [XmlArray]
    [XmlArrayItem(nameof(Confirm), typeof(Confirm))]
    [XmlArrayItem(nameof(InputPrompt), typeof(InputPrompt))]
    [XmlArrayItem(nameof(ShellCommand), typeof(ShellCommand))]
    [XmlArrayItem(nameof(ShellCommands), typeof(ShellCommands))]
    public TaskItem[] Items { get; init; }

    public Tasks()
    {
        Items = Array.Empty<TaskItem>();
    }
}
