﻿//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace WpLoad.Infrastructure
{
    /// <summary>
    /// Represents a subcommand
    /// </summary>
    internal interface ICommandBase
    {
        /// <summary>
        /// Command invoke name. Will be matched in a case insensitive manner
        /// </summary>
        string CommandName { get; }
    }
}
