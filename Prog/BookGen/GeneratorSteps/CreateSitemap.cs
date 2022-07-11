//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Sitemap;
using BookGen.Interfaces;

namespace BookGen.GeneratorSteps
{
    internal class CreateSitemap : IGeneratorStep
    {
        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            log.Info("Creating sitemap.xml...");

            var sitemap = new UrlSet();

            IEnumerable<string>? pages = from file in settings.OutputDirectory.GetAllFiles()
                                         where
                                             file.Extension == ".html"
                                         select
                                             file.ToString().Replace(settings.OutputDirectory.ToString(), "");

            foreach (string? page in pages)
            {
                string? reallink = $"{settings.Configuration.HostName}{page.Replace("\\", "/")}";
                sitemap.Url.Add(CreateEntry(reallink));
                log.Detail("Creating sitemap entry for: {0}", page);
            }

            FsPath? output = settings.OutputDirectory.Combine("sitemap.xml");
            output.SerializeXml(sitemap, log);
        }

        private Url CreateEntry(string page)
        {
            return new Url
            {
                Loc = page,
                Lastmod = DateTime.Now.ToW3CTimeFormat()
            };
        }
    }
}
