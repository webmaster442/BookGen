//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.TaskRunner;

[XmlRoot("Tasks")]
public class BookGenTasks
{
    public BookGenTask[] Tasks { get; init; }

    [XmlAttribute]
    public string Version { get; init; }

    public BookGenTasks()
    {
        Version = "1.0";
        Tasks = Array.Empty<BookGenTask>();
    }
}
