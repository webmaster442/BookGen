using System.Net;
using System.Text;

namespace BookGen.Api;

/// <summary>
/// A builder class for generating HTML content in a fluent manner.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HtmlBuilder"/> class with the specified initial capacity.
/// </remarks>
/// <param name="size">The initial capacity of the underlying <see cref="StringBuilder"/>.</param>
public sealed class HtmlBuilder(int size)
{
    private readonly StringBuilder _sb = new(size);
    private readonly Stack<string> _openTags = new();

    /// <summary>
    /// Creates a new instance of <see cref="HtmlBuilder"/> with the specified initial capacity.
    /// </summary>
    /// <param name="size">The initial capacity of the underlying <see cref="StringBuilder"/>.</param>
    /// <returns>A new instance of <see cref="HtmlBuilder"/>.</returns>
    public static HtmlBuilder Create(int size = 4096)
    {
        return new(size);
    }

    /// <summary>
    /// Creates an HTML element with the specified tag name and optional content.
    /// </summary>
    /// <param name="tag">The name of the HTML tag.</param>
    /// <param name="content">An optional action to generate the content of the element.</param>
    /// <returns>The current <see cref="HtmlBuilder"/> instance.</returns>
    public HtmlBuilder Element(string tag, Action<HtmlBuilder>? content = null)
    {
        _sb.Append('<').Append(tag).Append('>');

        if (content == null)
        {
            _openTags.Push(tag);
        }
        else
        {
            content(this);
            _sb.Append("</").Append(tag).Append('>');
        }

        return this;
    }
    /// <summary>
    /// Creates an HTML element with the specified tag name, attributes, and optional content.
    /// </summary>
    /// <param name="tag">The name of the HTML tag.</param>
    /// <param name="configure">An action to configure the attributes of the tag.</param>
    /// <param name="content">An optional action to generate the content of the element.</param>
    /// <returns>The current <see cref="HtmlBuilder"/> instance.</returns>
    public HtmlBuilder Element(string tag, Action<TagBuilder> configure, Action<HtmlBuilder>? content = null)
    {
        var tb = new TagBuilder(tag);
        configure(tb);

        _sb.Append('<').Append(tag);

        foreach (var attr in tb.Attributes)
        {
            _sb.Append(' ')
               .Append(attr.Key)
               .Append("=\"")
               .Append(WebUtility.HtmlEncode(attr.Value))
               .Append('"');
        }

        _sb.Append('>');

        if (content == null)
        {
            _openTags.Push(tag);
        }
        else
        {
            content(this);
            _sb.Append("</").Append(tag).Append('>');
        }

        return this;
    }

    /// <summary>
    /// Closes the most recently opened HTML element.
    /// </summary>
    /// <returns>The current <see cref="HtmlBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if there are no open elements to close.</exception>
    public HtmlBuilder End()
    {
        if (_openTags.Count == 0)
        {
            throw new InvalidOperationException("No open elements.");
        }

        string tag = _openTags.Pop();

        _sb.Append("</").Append(tag).Append('>');
        return this;
    }

    /// <summary>
    /// Appends text content to the current HTML element, encoding it to prevent XSS attacks.
    /// </summary>
    /// <param name="text">The text content to append.</param>
    /// <returns>The current <see cref="HtmlBuilder"/> instance.</returns>
    public HtmlBuilder Text(string text)
    {
        _sb.Append(WebUtility.HtmlEncode(text));
        return this;
    }

    /// <summary>
    /// Appends raw HTML content to the current HTML element without encoding.
    /// </summary>
    /// <param name="html">The raw HTML content to append.</param>
    /// <returns>The current <see cref="HtmlBuilder"/> instance.</returns>
    public HtmlBuilder Raw(string html)
    {
        _sb.Append(html);
        return this;
    }

    /// <summary>
    /// Creates a self-closing HTML element with the specified tag name and optional attributes.
    /// </summary>
    /// <param name="tag">The name of the HTML tag.</param>
    /// <param name="configure">An optional action to configure the attributes of the tag.</param>
    /// <returns>The current <see cref="HtmlBuilder"/> instance.</returns>
    public HtmlBuilder SelfClosing(string tag, Action<TagBuilder>? configure = null)
    {
        _sb.Append('<').Append(tag);

        if (configure != null)
        {
            var tb = new TagBuilder(tag);
            configure(tb);

            foreach (KeyValuePair<string, string> attr in tb.Attributes)
            {
                _sb.Append(' ')
                   .Append(attr.Key)
                   .Append("=\"")
                   .Append(WebUtility.HtmlEncode(attr.Value))
                   .Append('"');
            }
        }

        _sb.Append(" />");

        return this;
    }

    /// <summary>
    /// Returns the generated HTML content as a string. Throws an exception if there are unclosed HTML elements.
    /// </summary>
    /// <returns>The generated HTML content as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if there are unclosed HTML elements.</exception>
    public override string ToString()
    {
        return _openTags.Count != 0
            ? throw new InvalidOperationException("There are unclosed HTML elements.")
            : _sb.ToString();
    }

    /// <summary>
    /// A builder class for constructing HTML tags with attributes in a fluent manner.
    /// </summary>
    public sealed class TagBuilder
    {
        internal Dictionary<string, string> Attributes { get; } = [];

        internal TagBuilder(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// Gets the name of the HTML tag being constructed.
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Adds an attribute to the HTML tag being constructed.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>The current <see cref="TagBuilder"/> instance.</returns>
        public TagBuilder Attr(string name, string value)
        {
            Attributes[name] = value;
            return this;
        }

        /// <summary>
        /// Adds a class attribute to the HTML tag being constructed.
        /// </summary>
        /// <param name="className">The value of the class attribute.</param>
        /// <returns>The current <see cref="TagBuilder"/> instance.</returns>
        public TagBuilder Class(string className)
        {
            return Attr("class", className);
        }

        /// <summary>
        /// Adds an id attribute to the HTML tag being constructed.
        /// </summary>
        /// <param name="id">The value of the id attribute.</param>
        /// <returns>The current <see cref="TagBuilder"/> instance.</returns>
        public TagBuilder Id(string id)
        {
            return Attr("id", id);
        }

        /// <summary>
        /// Adds a style attribute to the HTML tag being constructed.
        /// </summary>
        /// <param name="style">The value of the style attribute.</param>
        /// <returns>The current <see cref="TagBuilder"/> instance.</returns>
        public TagBuilder Style(string style)
        {
            return Attr("style", style);
        }

        /// <summary>
        /// Adds a data-* attribute to the HTML tag being constructed.
        /// </summary>
        /// <param name="name">The name of the data attribute.</param>
        /// <param name="value">The value of the data attribute.</param>
        /// <returns>The current <see cref="TagBuilder"/> instance.</returns>
        public TagBuilder Data(string name, string value)
        {
            return Attr($"data-{name}", value);
        }
    }
}
