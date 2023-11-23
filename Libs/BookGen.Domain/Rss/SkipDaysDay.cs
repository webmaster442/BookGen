//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Rss;

/// <summary>
/// Specifies the days where aggregators should skip updating the feed
/// </summary>
[Serializable]
[XmlType(AnonymousType = true)]
public enum SkipDaysDay
{
    /// <summary>
    /// Monday
    /// </summary>
    Monday,
    /// <summary>
    /// Tuesday
    /// </summary>
    Tuesday,
    /// <summary>
    /// Wednesday
    /// </summary>
    Wednesday,
    /// <summary>
    /// Thursday
    /// </summary>
    Thursday,
    /// <summary>
    /// Friday
    /// </summary>
    Friday,
    /// <summary>
    /// Saturday
    /// </summary>
    Saturday,
    /// <summary>
    /// Sunday
    /// </summary>
    Sunday,
}
