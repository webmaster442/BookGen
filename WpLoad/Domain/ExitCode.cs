namespace WpLoad.Domain
{
    /// <summary>
    /// Program exit codes
    /// </summary>
    internal enum ExitCode
    {
        /// <summary>
        /// No issues
        /// </summary>
        Success = 0,
        /// <summary>
        /// Execution failed for some reason
        /// </summary>
        Fail = 1,
        /// <summary>
        /// Bad or unknown parameters were given to the program
        /// </summary>
        BadParameters = 2,
        /// <summary>
        /// Program has crashed
        /// </summary>
        Crash = -1,
        /// <summary>
        /// Succeded, but with warning
        /// </summary>
        Warning = 3,
    }
}
