﻿//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Domain.Epub;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubContent : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Creating epub content.opf...");
            var output = settings.OutputDirectory.Combine("epubtemp\\OEBPS\\content.opf");
            Package pack = new Package
            {
                Uniqueidentifier = "bookid",
                Version = "2.0",
                Metadata = new Metadata
                {
                    Title = settings.Configuration.Metadata.Title,
                    Creator = settings.Configuration.Metadata.Author,
                    Identifier = new Identifier
                    {
                        Id = "bookid",
                        Text = "0"
                    },
                    Publisher = "",
                    Language = "en",
                    Rights = string.Empty,
                    Meta = new List<MetaOpf>
                    {
                        new MetaOpf
                        {
                            Name = "epub-converter",
                            Content = "BookGen",
                        },
                        new MetaOpf
                        {
                            Name = "cover",
                            Content = settings.Configuration.Metadata.CoverImage
                        }
                    }
                },
                Manifest = new Manifest
                {
                    Item = new List<Item>
                    {
                        new Item
                        {
                            Href = "toc.ncx",
                            Id = "ncx",
                            Mediatype = "application/x-dtbncx+xml"
                        }
                    }
                },
                Spine = new Spine
                {
                    Toc = "ncx",
                    Itemref = new List<Itemref>()
                },
                Guide = new Guide
                {
                    Reference = new List<Reference>
                    {
                        new Reference
                        {
                            Href = "page_001.html",
                            Type = "text",
                            Title = settings.TocContents.Chapters.FirstOrDefault()
                        }
                    }
                }
            };
            GenerateItems(pack.Manifest.Item, settings);
            GenerateSpine(pack.Spine.Itemref, settings);
            var namespaces = new List<(string, string)>
            {
                ("", "http://www.idpf.org/2007/opf"),
                ("dc", "http://purl.org/dc/elements/1.1/"),
                ("opf", "http://www.idpf.org/2007/opf")
            };
            output.SerializeXml(pack, namespaces);
        }

        private void GenerateSpine(List<Itemref> itemref, RuntimeSettings settings)
        {
            int chaptercounter = 1;
            foreach (var chapter in settings.TocContents.Files)
            {
                itemref.Add(new Itemref
                {
                    Idref = $"page_{chaptercounter:D3}"
                });
                ++chaptercounter;
            }
        }

        private void GenerateItems(List<Item> item, RuntimeSettings settings)
        {
            int chaptercounter = 1;
            foreach (var chapter in settings.TocContents.Files)
            {
                var nitem = $"page_{chaptercounter:D3}";
                item.Add(new Item
                {
                    Href = nitem + ".html",
                    Id = nitem,
                    Mediatype = "application/xhtml+xml"
                });
                ++chaptercounter;
            }
        }
    }
}
