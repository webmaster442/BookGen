//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Domain;
using BookGen.Domain.Sitemap;
using BookGen.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BookGen.GeneratorSteps
{
    internal class CreateSitemap : IGeneratorStep
    {

        private const string w3cTime = "yyyy-MM-ddTHH:mm:ss.fffffffzzz";

        public void RunStep(GeneratorSettings settings, ILog log)
        {
            log.Info("Creating sitemap.xml...");

            UrlSet sitemap = new UrlSet();

            var pages = from file in settings.OutputDirectory.GetAllFiles()
                        where
                            Path.GetExtension(file) == ".html"
                        select
                            file.Replace(settings.OutputDirectory.ToString(), "");

            foreach (var page in pages)
            {
                var reallink = $"{settings.Configruation.HostName}{page.Replace("\\", "/")}";
                sitemap.Url.Add(CreateEntry(reallink));
                log.Detail("Creating sitemap entry for: {0}", page);
            }

            XmlSerializer xs = new XmlSerializer(typeof(UrlSet));
            var output = settings.OutputDirectory.Combine("sitemap.xml");
            using (var fs = File.Create(output.ToString()))
            {
                xs.Serialize(fs, sitemap);
            }
        }

        private Url CreateEntry(string page)
        {
            return new Url
            {
                Loc = page,
                Lastmod = DateTime.Now.ToString(w3cTime)
            };
        }
    }
}
