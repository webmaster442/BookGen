namespace Bookgen.Lib.Domain.VsCode;

public static class VsCodeVars
{
    /// <summary>
    /// Path of the user's home folder
    /// </summary>
    public const string UserHome = "${userHome}";
    /// <summary>
    /// Path of the folder opened in VS Code
    /// </summary>
    public const string WorkspaceFolder = "${workspaceFolder}";
    /// <summary>
    /// Name of the folder opened in VS Code without any slashes (/)
    /// </summary>
    public const string WorkspaceFolderBasename = "${workspaceFolderBasename}";
    /// <summary>
    /// Currently opened file
    /// </summary>
    public const string File = "${file}";
    /// <summary>
    /// Currently opened file's workspace folder
    /// </summary>
    public const string FileWorkspaceFolder = "${fileWorkspaceFolder}";
}