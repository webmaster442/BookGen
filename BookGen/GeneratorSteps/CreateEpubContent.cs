//-----------------------------------------------------------------------------
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

namespace BookGen.GeneratorSteps
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
                Xmlns = "http://www.idpf.org/2007/opf",
                Version = "2.0",
                Metadata = new Metadata
                {
                    Title = settings.Configruation.Metadata.Title,
                    Creator = settings.Configruation.Metadata.Author,
                    Identifier = new Identifier
                    {
                        Id = "bookid",
                        Text = "0"
                    },
                    Publisher = "",
                    Language = "en",
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
                            Content = settings.Configruation.Metadata.CoverImage
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
                            Href = "chapter_01.html",
                            Type = "text",
                            Title = settings.TocContents.Chapters.FirstOrDefault()
                        }
                    }
                }
            };
            GenerateItems(pack.Manifest.Item, settings);
            GenerateSpine(pack.Spine.Itemref, settings);
            output.SerializeXml(pack);
        }

        private void GenerateSpine(List<Itemref> itemref, RuntimeSettings settings)
        {
            int chaptercounter = 1;
            foreach (var chapter in settings.TocContents.Chapters)
            {
                itemref.Add(new Itemref
                {
                    Idref = $"chapter_{chaptercounter:D2}"
                });
                ++chaptercounter;
            }
        }

        private void GenerateItems(List<Item> item, RuntimeSettings settings)
        {
            int chaptercounter = 1;
            foreach (var chapter in settings.TocContents.Chapters)
            {
                var nitem = $"chapter_{chaptercounter:D2}";
                item.Add(new Item
                {
                    Href = nitem+".html",
                    Id = nitem,
                    Mediatype = "application/xhtml+xml"
                });
                ++chaptercounter;
            }
        }
    }
}
