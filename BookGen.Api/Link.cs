//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace BookGen.Api
{
    public sealed class Link : IEquatable<Link>
    {
        public string Text { get; }

        public string Url { get; }

        public Link(string text, string url)
        {
            Text = text;
            Url = url;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Link);
        }

        public bool Equals(Link? other)
        {
            return
                Text == other?.Text &&
                Url == other?.Url;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text, Url);
        }

        public static bool operator ==(Link left, Link right)
        {
            return EqualityComparer<Link>.Default.Equals(left, right);
        }

        public static bool operator !=(Link left, Link right)
        {
            return !(left == right);
        }

        public string GetHtml()
        {
            return $"<a href=\"{Url}\">{Text}</a>";
        }

        public string ConvertToLinkOnHost(string host)
        {
            var file = System.IO.Path.ChangeExtension(this.Url, ".html");
            return $"{host}{file}";
        }
    }
}
