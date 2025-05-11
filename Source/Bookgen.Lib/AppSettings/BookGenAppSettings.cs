namespace Bookgen.Lib.AppSettings;

public sealed class BookGenAppSettings : AppSettingsBase
{
    public BookGenAppSettings() : base("BookGen.json")
    {
    }

    protected override HashSet<string> NotNeededKeys => [];

    protected override void InitDefaults()
    {
        Editor = "notepad.exe";
        PythonPath = string.Empty;
        NodeJsPath = string.Empty;
    }

    public string Editor
    {
        get => Get<string>();
        set => Set(value);
    }

    public string NodeJsPath
    {
        get => Get<string>();
        set => Set(value);
    }

    public string PythonPath
    {
        get => Get<string>();
        set => Set(value);
    }
}
