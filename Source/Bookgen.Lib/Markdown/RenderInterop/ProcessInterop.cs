using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Bookgen.Lib.Markdown.RenderInterop;

internal static class ProcessInterop
{
    private static string GetBinary(string name)
    {
        return OperatingSystem.IsWindows()
            ? Path.Combine(AppContext.BaseDirectory, $"{name}.exe")
            : Path.Combine(AppContext.BaseDirectory, name);
    }

    public static string RunRatex(string input)
    {
        throw new NotImplementedException();
    }
}
