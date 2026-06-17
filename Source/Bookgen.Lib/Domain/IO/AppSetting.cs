using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO;

public sealed record class AppSetting
{
    public string? Editor { get; set; }

    [WhenNotEmptyFileMustExist]
    public string? NodeJsPath { get; set; }

    [WhenNotEmptyFileMustExist]
    public string? PythonPath { get; set; }

    public static AppSetting CreateDefault()
    {
        return new AppSetting
        {
            Editor = "notepad.exe",
            NodeJsPath = string.Empty,
            PythonPath = string.Empty,
        };
    }
}
