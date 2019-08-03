//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BookGen.Core
{
    public static class ResourceLocator
    {
        private static string TranslateResourcePath(string path)
        {
            return path.Replace("/", ".");
        }

        public static string GetResourceFile<AssemblyLocatorType>(string resource)
        {
            var assembly = typeof(AssemblyLocatorType).GetTypeInfo().Assembly;
            var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(TranslateResourcePath(resource), StringComparison.CurrentCultureIgnoreCase));
            using (var stream = assembly.GetManifestResourceStream(resourceName))
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
    }
}
