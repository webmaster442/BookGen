//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api
{
    /// <summary>
    /// BookGen Exit codes
    /// </summary>
    public enum ExitCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Succes = 0,
        /// <summary>
        /// Uncaught exception happened
        /// </summary>
        Exception = -1,
        /// <summary>
        /// Unknown command
        /// </summary>
        UnknownCommand = 1,
        /// <summary>
        /// Bad parameters for command
        /// </summary>
        BadParameters = 2,
        /// <summary>
        /// Bad config
        /// </summary>
        BadConfiguration = 3,
        /// <summary>
        /// Folder contains a bookgen.lock
        /// </summary>
        FolderLocked = 4,
        /// <summary>
        /// Not supported on executing platform
        /// </summary>
        PlatformNotSupported = 5,
    }
}
