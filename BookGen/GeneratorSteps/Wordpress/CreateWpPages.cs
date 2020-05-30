//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Domain.Wordpress;
using BookGen.Framework;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BookGen.GeneratorSteps.Wordpress
{
    internal class CreateWpPages : ITemplatedStep
    {
        private readonly Session _session;
#if DEBUG
        private readonly HashSet<int> _usedids;
#endif

        public CreateWpPages(Session session)
        {
            _session = session;
#if DEBUG
            _usedids = new HashSet<int>();
#endif
        }

        public TemplateProcessor? Template { get; set; }
        public IContent? Content { get; set; }

        private Item CreateItem(int uid, int parent, int order, string content, string title, string path, TemplateOptions TemplateOptions)
        {
#if DEBUG
            if (_usedids.Contains(uid))
            {
                //UID mismatch. Some kind of generator error
                Debugger.Break();
            }
            _usedids.Add(uid);
#endif

            var result = new Item
            {
                Content = content,
                Title = title,
                PubDate = DateTime.Now.ToWpTimeFormat(),
                Post_date = DateTime.Now.ToWpPostDate(),
                Post_date_gmt = DateTime.UtcNow.ToWpPostDate(),
                Menu_order = order,
                Ping_status = "closed",
                Comment_status = TemplateOptions[TemplateOptions.WordpressCommentStatus],
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
                Post_type = TemplateOptions[TemplateOptions.WordpressItemType],
                Link = path,
                Creator = TemplateOptions[TemplateOptions.WordpressAuthorLogin],
                Description = "",
                Guid = new Domain.Wordpress.Guid
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

        private string CreateFillerPage(IEnumerable<Link> links)
        {
            var builder = new StringBuilder();
            builder.Append("<ul>\n");
            foreach(var link in links)
            {
                builder.AppendFormat("<li>{0}</li>\n", link.Text);
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

            bool parentpageCreate = settings.CurrentBuildConfig.TemplateOptions.TryGetOption(TemplateOptions.WordpressCreateParent, out bool createparent) && createparent;
            bool createfillers = settings.CurrentBuildConfig.TemplateOptions.TryGetOption(TemplateOptions.WordpressCreateFillerPages, out bool filler) && filler;

            int mainorder = 0;
            int uid = 2000;
            int globalparent = 0;

            if (parentpageCreate)
            {
                string fillerPage = createfillers ? CreateFillerPage(settings.TocContents.GetLinksForChapter()) : "";
                string title = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpresCreateParentTitle];
                string path = $"{host}{Encode(title)}";
                Item parent = CreateItem(uid, 0, mainorder, fillerPage, title, path, settings.CurrentBuildConfig.TemplateOptions);
                _session.CurrentChannel.Item.Add(parent);
                globalparent = uid;
                ++uid;
            }

            foreach (var chapter in settings.TocContents.Chapters)
            {
                string fillerPage = createfillers ? CreateFillerPage(settings.TocContents.GetLinksForChapter(chapter)) : "";
                string path = $"{host}{Encode(chapter)}";
                int parent_uid = uid;

                Item parent = CreateItem(uid, globalparent, mainorder, fillerPage, chapter, path, settings.CurrentBuildConfig.TemplateOptions);
                _session.CurrentChannel.Item.Add(parent);
                int suborder = 0;
                uid++;
                
                foreach (var file in settings.TocContents.GetLinksForChapter(chapter).Select(l => l.Url))
                {
                    log.Detail("Processing {0}...", file);
                    var input = settings.SourceDirectory.Combine(file);
                    var raw = input.ReadFile(log);
                    Content.Content = MarkdownRenderers.Markdown2Wordpress(raw, settings);

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
