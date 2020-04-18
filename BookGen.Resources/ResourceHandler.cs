//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BookGen.Resources
{
    public static class ResourceHandler
    {
        private static string TranslateResourcePath(string path)
        {
            return path.Replace("/", ".");
        }

        public static Stream GetResourceStream<T>(string resource)
        {
            var assembly = typeof(T).GetTypeInfo().Assembly;

            var resources = assembly.GetManifestResourceNames();

            var resourceName = resources.First(s => s.EndsWith(TranslateResourcePath(resource), StringComparison.CurrentCultureIgnoreCase));

            return assembly.GetManifestResourceStream(resourceName);

        }

        public static string GetResourceFile<T>(string file)
        {
            using (var stream = GetResourceStream<T>(file))
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

        public static string GetResourceFile(string file)
        {
            using (var stream = GetResourceStream<KnownFile>(file))
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

        public static string GetFile(KnownFile file)
        {
            var location = KnownFileMap.Map[file];
            return GetResourceFile(location);
        }

        public static void ExtractKnownFile(KnownFile file, string targetDir, ILog log)
        {
            var location = KnownFileMap.Map[file];
            var filename = Path.GetFileName(KnownFileMap.Map[file]);

            try
            {
                using (var stream = GetResourceStream<KnownFile>(location))
                {
                    var targetName = Path.Combine(targetDir, filename);

                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    log.Detail("Extracting {0} to {1}", location,  targetName);

                    using (var target = File.Create(targetName))
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
