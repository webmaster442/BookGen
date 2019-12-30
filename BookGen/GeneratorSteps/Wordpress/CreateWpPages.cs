//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Domain.wordpress;
using BookGen.Framework;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BookGen.GeneratorSteps.Wordpress
{
    internal class CreateWpPages : ITemplatedStep
    {
        private readonly Session _session;

        public CreateWpPages(Session session)
        {
            _session = session;
        }

        public Template? Template { get; set; }
        public IContent? Content { get; set; }

        private Item CreateItem(int uid, int parent, int order, string content, string title, string path, TemplateOptions TemplateOptions)
        {
            Item result = new Item
            {
                Content = content,
                Title = title,
                PubDate = DateTime.Now.ToWpTimeFormat(),
                Post_date = DateTime.Now.ToWpPostDate(),
                Post_date_gmt = DateTime.UtcNow.ToWpPostDate(),
                Menu_order = order,
                Ping_status = "closed",
                Comment_status = "closed",
                Is_sticky = "0",
                Postmeta = new List<Postmeta>
                        {
                            new Postmeta { Meta_key = "", Meta_value = "" }
                        },
                Post_password = "",
                Status = "publish",
                Post_name = Encode(title),
                Post_id = uid,
                Post_parent = parent,
                Post_type = "page",
                Link = path,
                Creator = TemplateOptions[TemplateOptions.WordpressAuthorLogin],
                Description = "",
                Guid = new Domain.wordpress.Guid
                {
                    IsPermaLink = false,
                    Text = $"{TemplateOptions[TemplateOptions.WordpressTargetHost]}?page_id={uid}",
                }
            };
            return result;
        }

        private string Encode(string title)
        {
            var normalizedString = title.Trim().Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private string CreateFillerPage(IEnumerable<HtmlLink> links)
        {
            var builder = new StringBuilder();
            builder.Append("<ul>\n");
            foreach(var link in links)
            {
                builder.AppendFormat("<li>{0}</li>\n", link.DisplayString);
            }
            builder.Append("</ul>\n");
            return builder.ToString();
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating Wordpress export content...");
            _session.CurrentChannel.Item = new List<Item>();

            var host = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressTargetHost];

            int mainorder = 0;
            int uid = 2000;
            foreach (var chapter in settings.TocContents.Chapters)
            {
                string fillerPage = CreateFillerPage(settings.TocContents.GetLinksForChapter(chapter));
                string path = $"{host}{Encode(chapter)}";
                int parent_uid = uid;
                Item parent = CreateItem(parent_uid, 0, mainorder, fillerPage, chapter, path, settings.CurrentBuildConfig.TemplateOptions);
                _session.CurrentChannel.Item.Add(parent);
                int suborder = 0;
                foreach (var file in settings.TocContents.GetLinksForChapter(chapter).Select(l => l.Link))
                {
                    log.Detail("Processing {0}...", file);
                    var input = settings.SourceDirectory.Combine(file);
                    var raw = input.ReadFile(log);
                    Content.Content = MarkdownRenderers.Markdown2EpubHtml(raw, settings);

                    var title = MarkdownUtils.GetTitle(raw);

                    string subpath = $"{host}{Encode(chapter)}/{Encode(title)}";
                    var result = CreateItem(uid, parent_uid, suborder, Template.Render(), title, subpath, settings.CurrentBuildConfig.TemplateOptions);

                    _session.CurrentChannel.Item.Add(result);
                    ++suborder;
                    ++uid;
                }
                ++mainorder;
            }
        }
    }
}
