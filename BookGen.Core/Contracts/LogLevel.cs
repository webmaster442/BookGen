//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Contracts
{
    /// <summary>
    /// Logging level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Critical errors
        /// </summary>
        Critical = 0,
        /// <summary>
        /// Warnings
        /// </summary>
        Warning = 1,
        /// <summary>
        /// Info messages
        /// </summary>
        Info = 2,
        /// <summary>
        /// Detailed info messages
        /// </summary>
        Detail = 3
    }
}
