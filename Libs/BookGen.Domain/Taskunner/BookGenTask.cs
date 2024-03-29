﻿//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.TaskRunner;

public class BookGenTask
{
    [XmlArray()]
    [XmlArrayItem(nameof(Confirm), typeof(Confirm))]
    [XmlArrayItem(nameof(InputPrompt), typeof(InputPrompt))]
    [XmlArrayItem(nameof(ShellCommands), typeof(ShellCommands))]
    public TaskItem[] Items { get; init; }

    [XmlAttribute]
    public string Name { get; init; }

    public BookGenTask()
    {
        Items = Array.Empty<TaskItem>();
        Name = string.Empty;
    }
}
