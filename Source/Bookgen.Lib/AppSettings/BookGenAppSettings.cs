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
    }

    public string Editor
    {
        get => Get<string>();
        set => Set(value);
    }
}
