//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class CreateCacheManifest : IGeneratorStep
    {
        private IEnumerable<string> _cacheExtensions
        {
            get
            {
                yield return ".html";
                yield return ".htm";
                yield return ".jpg";
                yield return ".jpeg";
                yield return ".png";
                yield return ".svg";
                yield return ".js";
                yield return ".css";
            }
        }

        public void RunStep(GeneratorSettings settings)
        {
            Console.WriteLine("Generating Cache manifest...");
            var buffer = new StringBuilder();

            var cacheable = from file in settings.OutputDirectory.GetAllFiles()
                            where
                                _cacheExtensions.Contains(Path.GetExtension(file))
                            select 
                                file.Replace(settings.OutputDirectory.ToString(), "");

            buffer.Append("CACHE MANIFEST\n");
            buffer.AppendFormat("#rev {0}\n", DateTime.UtcNow.ToBinary());

            var output = settings.OutputDirectory.Combine("cache.appcache");

            foreach (var file in cacheable)
            {
                buffer.AppendFormat("{0}{1}\n", settings.Configruation.HostName, file.Replace("\\", "/"));

            }
            output.WriteFile(buffer.ToString());
        }
    }
}
