//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

        private static Stream GetResourceStream(string resource)
        {
            var assembly = typeof(KnownFile).GetTypeInfo().Assembly;

            var resources = assembly.GetManifestResourceNames();

            var resourceName = resources.First(s => s.EndsWith(TranslateResourcePath(resource), StringComparison.CurrentCultureIgnoreCase));

            return assembly.GetManifestResourceStream(resourceName);

        }

        public static string GetFile(string file)
        {
            using (var stream = GetResourceStream(file))
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
            return GetFile(location);
        }

        public static void ExtractKnownFile(KnownFile file, string targetDir)
        {
            var location = KnownFileMap.Map[file];
            var filename = Path.GetFileName(KnownFileMap.Map[file]);

            using (var stream = GetResourceStream(location))
            {
                using (var target = File.Create(Path.Combine(targetDir, filename)))
                {
                    stream.CopyTo(target);
                }
            }
        }
    }
}
