//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;

namespace BookGen.GeneratorSteps
{
    internal class CreateEpubStructure : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Creating epub structure...");

            log.Detail("Creating mimetype file...");
            var mime = settings.OutputDirectory.Combine("epubtemp\\mimetype");
            mime.WriteFile("application/epub+zip");

            log.Detail("Creating META-INF\\container.xml file...");
            var output = settings.OutputDirectory.Combine("epubtemp\\META-INF\\container.xml");

            var container = new Domain.Epub.Container
            {
                Version = "1.0",
                Xmlns = "urn:oasis:names:tc:opendocument:xmlns:container",
                Rootfiles = new Domain.Epub.Rootfiles
                {
                    Rootfile = new Domain.Epub.Rootfile
                    {
                        Fullpath = "OEBPS/content.opf",
                        Mediatype = "application/oebps-package+xml",
                    }
                }
            };

            output.SerializeXml(container);

            var oebps = settings.OutputDirectory.Combine("epubtemp\\OEBPS");
            oebps.CreateDir(log);
        }
    }
}
