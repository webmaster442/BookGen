//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System.Reflection;

namespace BookGen.Resources
{
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

        private static string GetResourceFile(string file)
        {
            using (Stream? stream = GetResourceStream<KnownFile>(file))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
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

        public static string GetFile(KnownFile file)
        {
            string? location = KnownFileMap.Map[file];
            return GetResourceFile(location);
        }

        public static Stream GetFileStream(KnownFile file) 
        {
            string? location = KnownFileMap.Map[file];
            return GetResourceStream<KnownFile>(location) ?? throw new InvalidOperationException();
        }

        public static void ExtractKnownFile(KnownFile file, string targetDir, ILog log)
        {
            string? location = KnownFileMap.Map[file];
            string? filename = Path.GetFileName(KnownFileMap.Map[file]);

            try
            {
                using (Stream? stream = GetResourceStream<KnownFile>(location))
                {
                    if (stream == null)
                    {
                        throw new InvalidOperationException($"Resource not found for: {file}");
                    }

                    string? targetName = Path.Combine(targetDir, filename);

                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    log.Detail("Extracting {0} to {1}", location, targetName);

                    using (FileStream? target = File.Create(targetName))
                    {
                        stream.CopyTo(target);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warning(ex);
            }
        }
    }
}
