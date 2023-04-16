//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.AssemblyDocumenter.Units;

/// <summary>
/// The member kind.
/// </summary>
internal enum MemberKind
{
    /// <summary>
    /// Not supported member kind.
    /// </summary>
    NotSupported,

    /// <summary>
    /// Type.
    /// </summary>
    Type,

    /// <summary>
    /// Constructor.
    /// </summary>
    Constructor,

    /// <summary>
    /// Constants
    /// </summary>
    Constants,

    /// <summary>
    /// Property.
    /// </summary>
    Property,

    /// <summary>
    /// Method.
    /// </summary>
    Method,
}
