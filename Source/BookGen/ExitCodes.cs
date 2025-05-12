namespace BookGen;

internal static class ExitCodes
{
    /// <summary>
    /// Succcesfull exit code = 0
    /// </summary>
    public const int Succes = 0;
    /// <summary>
    /// Aguments error exit code = 1
    /// </summary>
    public const int ArgumentsError = 1;
    /// <summary>
    /// Config error exit code = 2
    /// </summary>
    public const int ConfigError = 2;
    /// <summary>
    /// Folder lock exit code = 3
    /// </summary>
    public const int FolderLocked = 3;
    /// <summary>
    /// General error
    /// </summary>
    public const int GeneralError = int.MaxValue;
}
