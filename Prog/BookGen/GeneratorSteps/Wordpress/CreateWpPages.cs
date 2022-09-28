//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;
using BookGen.Domain.Configuration;
using BookGen.Domain.Wordpress;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;
using BookGen.Interfaces;
using System.Diagnostics;
using System.Globalization;

namespace BookGen.GeneratorSteps.Wordpress
{
    internal sealed class CreateWpPages : ITemplatedStep
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

        public ITemplateProcessor? Template { get; set; }
        public IContent? Content { get; set; }

        private Item CreateItem(int uid,
                                int parent,
                                int order,
                                string content,
                                string title,
                                string path,
                                IReadOnlyTemplateOptions templateOptions)
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
                Comment_status = templateOptions[TemplateOptions.WordpressCommentStatus],
                Is_sticky = "0",
                Postmeta = new List<Postmeta>
                        {
                            new Postmeta { Meta_key = "", Meta_value = "" }
                        },
                Post_password = "",
                Status = "publish",
                Post_name = EncodeTitle(title),
                Post_id = uid,
                Post_parent = parent,
                Post_type = templateOptions[TemplateOptions.WordpressItemType],
                Link = path,
                Creator = templateOptions[TemplateOptions.WordpressAuthorLogin],
                Description = "",
                Guid = new Domain.Wordpress.Guid
                {
                    IsPermaLink = false,
                    Text = $"{templateOptions[TemplateOptions.WordpressTargetHost]}?page_id={uid}",
                },
            };
            return result;
        }

        private static void CreateTagsForItem(Item result, ITagUtils tags, string file, string tagCategory)
        {
            ISet<string>? fileTags = tags.GetTagsForFile(file);
            result.Category = new List<PostCategory>(fileTags.Count);
            foreach (string? tag in fileTags)
            {
                result.Category.Add(new PostCategory
                {
                    Domain = tagCategory,
                    Value = tag,
                    Nicename = tags.GetUrlNiceName(tag)
                });
            }
        }

        private static string EncodeTitle(string title)
        {
            string? normalizedString = title.Trim().Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string CreateFillerPage(IEnumerable<Link> links)
        {
            var builder = new StringBuilder();
            builder.Append("<ul>\n");
            foreach (Link? link in links)
            {
                builder.AppendFormat("<li>{0}</li>\n", link.Text);
            }
            builder.Append("</ul>\n");
            return builder.ToString();
        }

        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating Wordpress export content...");
            _session.CurrentChannel.Item = new List<Item>();

            string? host = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressTargetHost];

            bool parentpageCreate = settings.CurrentBuildConfig.TemplateOptions.TryGetOption(TemplateOptions.WordpressCreateParent, out bool createparent) && createparent;
            bool createfillers = settings.CurrentBuildConfig.TemplateOptions.TryGetOption(TemplateOptions.WordpressCreateFillerPages, out bool filler) && filler;

            int mainorder = 0;
            int uid = 2000;
            int globalparent = 0;

            if (parentpageCreate)
            {
                string fillerPage = createfillers ? CreateFillerPage(settings.TocContents.GetLinksForChapter()) : "";
                string title = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpresCreateParentTitle];
                string path = $"{host}{EncodeTitle(title)}";
                Item parent = CreateItem(uid, 0, mainorder, fillerPage, title, path, settings.CurrentBuildConfig.TemplateOptions);
                _session.CurrentChannel.Item.Add(parent);
                globalparent = uid;
                ++uid;
            }

            using var pipeline = new BookGenPipeline(BookGenPipeline.Wordpress);
            pipeline.InjectRuntimeConfig(settings);

            foreach (string? chapter in settings.TocContents.Chapters)
            {
                string fillerPage = createfillers ? CreateFillerPage(settings.TocContents.GetLinksForChapter(chapter)) : "";
                string path = $"{host}{EncodeTitle(chapter)}";
                int parent_uid = uid;

                Item parent = CreateItem(uid, globalparent, mainorder, fillerPage, chapter, path, settings.CurrentBuildConfig.TemplateOptions);
                _session.CurrentChannel.Item.Add(parent);
                int suborder = 0;
                uid++;

                foreach (string? file in settings.TocContents.GetLinksForChapter(chapter).Select(l => l.Url))
                {
                    log.Detail("Processing {0}...", file);
                    FsPath? input = settings.SourceDirectory.Combine(file);
                    string? raw = input.ReadFile(log);
                    Content.Content = pipeline.RenderMarkdown(raw);

                    string? title = MarkdownUtils.GetDocumentTitle(raw, log);

                    string subpath = $"{host}{EncodeTitle(chapter)}/{EncodeTitle(title)}";

                    Item? result = CreateItem(uid,
                                            parent_uid,
                                            suborder,
                                            Template.Render(),
                                            title,
                                            subpath,
                                            settings.CurrentBuildConfig.TemplateOptions);

                    string tagCategory = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressTagCategory];

                    CreateTagsForItem(result, settings.Tags, file, tagCategory);

                    _session.CurrentChannel.Item.Add(result);
                    ++suborder;
                    ++uid;
                }
                ++mainorder;
            }
        }
    }
}
