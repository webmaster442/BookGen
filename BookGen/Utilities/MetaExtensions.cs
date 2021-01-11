﻿//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Domain;
using System.Text;
using System.Web;

namespace BookGen.Utilities
{
    public static class MetaExtensions
    {
        public static MetaTag FillWithConfigDefaults(this MetaTag metaTag, Config config)
        {
            metaTag.Author = config.Metadata.Author;
            metaTag.Title = config.Metadata.Title;
            metaTag.ImageUrl = config.Metadata.CoverImage;

            return metaTag;
        }

        public static string GetHtmlMeta(this MetaTag tag)
        {
            var metaData = new StringBuilder();
            metaData.AppendFormat("<meta name=\"author\" content=\"{0}\">\n", HttpUtility.HtmlEncode(tag.Author));
            metaData.AppendFormat("<meta property=\"og:site_name\" content=\"{0}\">\n", HttpUtility.HtmlEncode(tag.Title));
            metaData.AppendFormat("<meta property=\"og:image\" content=\"{0}\">\n", HttpUtility.HtmlEncode(tag.ImageUrl));
            metaData.AppendFormat("<meta property=\"og:title\" content=\"{0}\">\n", HttpUtility.HtmlEncode(tag.Title));
            metaData.AppendFormat("<meta property=\"og:url\" content=\"{0}\">\n", HttpUtility.HtmlEncode(tag.Url));
            metaData.AppendFormat("<meta property=\"og:description\" content=\"{0}\">\n", HttpUtility.HtmlEncode(tag.Description));
            metaData.Append("<meta property=\"og:type\" content=\"article\">\n");

            return metaData.ToString();
        }
    }
}
