//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Epub;
using BookGen.Interfaces;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubStructure : IGeneratorStep
    {
        private static void CreateMimeTypeFile(IReadonlyRuntimeSettings settings, ILog log)
        {
            log.Detail("Creating mimetype file...");
            FsPath? mime = settings.OutputDirectory.Combine("epubtemp\\mimetype");
            mime.WriteFile(log, "application/epub+zip");
        }

        private static void CreateFolderStructure(IReadonlyRuntimeSettings settings, ILog log)
        {
            string[] directories = { "epubtemp\\META-INF", "epubtemp\\OPS" };

            foreach (string? directory in directories)
            {
                FsPath path = settings.OutputDirectory.Combine(directory);
                path.CreateDir(log);
            }
        }

        private static void CreateContainerXML(IReadonlyRuntimeSettings settings, ILog log)
        {
            log.Info("Creating META-INF/container.xml");

            var container = new Container
            {
                Rootfiles = new Rootfiles
                {
                    Rootfile = new Rootfile
                    {
                        Mediatype = "application/oebps-package+xml",
                        Fullpath = "OPS/package.opf"
                    }
                },
                Version = "1.0",
                Xmlns = "urn:oasis:names:tc:opendocument:xmlns:container"
            };

            FsPath path = settings.OutputDirectory.Combine("epubtemp\\META-INF\\container.xml");

            var namespaces = new List<(string prefix, string namespac)>
            {
                ("", "urn:oasis:names:tc:opendocument:xmlns:container")
            };

            path.SerializeXml(container, log, namespaces);
        }


        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            log.Info("Creating epub structure...");

            CreateMimeTypeFile(settings, log);
            CreateFolderStructure(settings, log);
            CreateContainerXML(settings, log);
        }
    }
}
