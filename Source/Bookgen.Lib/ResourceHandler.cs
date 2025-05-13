//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace Bookgen.Lib;

public static class ResourceHandler
{
    private static string TranslateResourcePath(string path)
    {
        return path.Replace("/", ".");
    }

    private static Stream? GetResourceStream<T>(string resource)
    {
        Assembly? assembly = typeof(T).GetTypeInfo().Assembly;

        string[]? resources = assembly.GetManifestResourceNames();

        string? resourceName = resources.First(s => s.EndsWith(TranslateResourcePath(resource), StringComparison.CurrentCultureIgnoreCase));

        return assembly.GetManifestResourceStream(resourceName);
    }

    public static IReadOnlyList<string> GetResourceFileLines<T>(string file)
    {
        List<string> lines = new();

        using (Stream? stream = GetResourceStream<T>(file))
        {
            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }
            using (var reader = new StreamReader(stream))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line);
            }
        }
        return lines;
    }
}
