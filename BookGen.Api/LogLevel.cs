//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api
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
        /// Detailed info messages. Only visible when verbose 
        /// </summary>
        Detail = 3
    }
}
