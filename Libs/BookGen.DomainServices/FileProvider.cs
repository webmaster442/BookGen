//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.DomainServices;
public static class FileProvider
{
    private const string BookGenFolder = ".bookgen";
    private const string AppSettingsFile = "BookGen.app.json";
    private const string LaucnherTempFile = "bookgenlauncher.temp.json";
    private const string LaucherFile = "bookgenlauncher.json";
    private const string TodoTempFile = "bookgenTodo.temp.json";
    private const string TodoFile = "bookgenTodo.json";

    private static string GetOrCreateBookGenConfigFolder()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var dir = Path.Combine(userProfile, BookGenFolder);
        if (!Directory.Exists(dir)) 
        {
            Directory.CreateDirectory(dir);
        }
        return dir;
    }

    public static string GetConfigFile()
    {
        return Path.Combine(GetOrCreateBookGenConfigFolder(), AppSettingsFile);
    }

    public static string GetLauncherFile()
        => Path.Combine(GetOrCreateBookGenConfigFolder(), LaucherFile);
    
    public static string GetLauncherTempFile()
        => Path.Combine(GetOrCreateBookGenConfigFolder(), LaucnherTempFile);

    public static string GetLauncherTodoFile() 
        => Path.Combine(GetOrCreateBookGenConfigFolder(), TodoFile);

    public static string GetLauncherTodoTempFile()
        => Path.Combine(GetOrCreateBookGenConfigFolder(), TodoTempFile);

    public static string GetWwwConfig()
        => Path.Combine(AppContext.BaseDirectory, "WwwConfig.xml");
}
