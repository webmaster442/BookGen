//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// Register processes to be notified immediately of updates of the feed
/// </summary>
[Serializable]
public class Cloud
{
    /// <summary>
    /// Domain
    /// </summary>
    [XmlAttribute("domain")]
    public required string Domain { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    [XmlAttribute("port")]
    public required int Port { get; set; }

    /// <summary>
    /// Path
    /// </summary>
    [XmlAttribute("path")]
    public required string Path { get; set; }

    /// <summary>
    /// Register procedure name
    /// </summary>
    [XmlAttribute("registerProcedure")]
    public required string RegisterProcedure { get; set; }

    /// <summary>
    /// either xml-rpc or soap.
    /// </summary>
    [XmlAttribute("protocol")]
    public required string Protocol { get; set; }
}
