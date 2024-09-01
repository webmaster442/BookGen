//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Epub;

namespace BookGen.GeneratorSteps.Epub;

internal sealed class CreatePackageOpf : IGeneratorStep
{
    private readonly EpubSession _session;

    public CreatePackageOpf(EpubSession session)
    {
        _session = session;
    }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        log.Info("Creating OPS/package.opf...");

        var package = new Package
        {
            Version = "3.0",
            Uniqueidentifier = "q",
            Metadata = new Metadata
            {
                Meta = new List<Meta>
                {
                    new Meta
                    {
                        Property = "dcterms:modified",
                        Text= DateTime.Now.ToW3CZTimeFormat(),
                    },
                },
                Title = new List<Title>
                {
                    new Title
                    {
                        Id = "title",
                        Text = settings.Configuration.Metadata.Title
                    },
                },
                Creator = new List<Creator>
                {
                    new Creator
                    {
                        Id = "creator",
                        Text = settings.Configuration.Metadata.Author
                    },
                },
                Language = "en",
                Identifier = new Identifier
                {
                    Id = "q",
                    Text = "book",
                },
                Date = DateTime.Now.ToW3CTimeFormat(),
            },
            Manifest = CreateManifest(),
            Spine = CreateSpine()
        };

        FsPath path = settings.OutputDirectory.Combine("epubtemp\\OPS\\package.opf");

        var namespaces = new List<(string prefix, string namespac)>
        {
            ("", "http://www.idpf.org/2007/opf"),
            ("dc", "http://purl.org/dc/elements/1.1/")
        };

        path.SerializeXml(package, log, namespaces);
    }

    private Manifest CreateManifest()
    {
        var manifest = new Manifest
        {
            Item = new List<Item>(_session.GeneratedFiles.Count)
        };

        manifest.Item.Add(new Item
        {
            Id = "nav",
            Href = "nav.xhtml",
            Mediatype = "application/xhtml+xml",
            Properties = "nav",
        });
        manifest.Item.Add(new Item
        {
            Id = "ncx",
            Mediatype = "application/x-dtbncx+xml",
            Href = "toc.ncx"
        });

        foreach (string? file in _session.GeneratedFiles)
        {
            manifest.Item.Add(new Item
            {
                Id = file,
                Href = $"{file}.xhtml",
                Mediatype = "application/xhtml+xml",
                Properties = null,
            });
        }
        return manifest;
    }

    private Spine CreateSpine()
    {
        var spine = new Spine
        {
            Itemref = new List<Itemref>(_session.GeneratedFiles.Count),
            Toc = "ncx"
        };

        spine.Itemref.Add(new Itemref
        {
            Idref = "nav"
        });

        foreach (string? file in _session.GeneratedFiles)
        {
            spine.Itemref.Add(new Itemref
            {
                Idref = file
            });
        }
        return spine;
    }
}
