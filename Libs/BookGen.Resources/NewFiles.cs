//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace BookGen.Resources;
public sealed class NewFiles
{
    private readonly Dictionary<FileKey, string> _files;

    internal record class FileKey
    {
        public FileKey(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; }
        public string Path { get; }
    }

    public NewFiles()
    {
        _files = new Dictionary<FileKey, string>
        {
            { new FileKey("html", "/NewFiles/html.html"), "Blank HTML5 page" },
            { new FileKey("markdown", "/NewFiles/markdown.md"), "Blank markdown document" },
            { new FileKey("mvpcss", "/NewFiles/mvpcss.css"), "A minimalist stylesheet for HTML elements - https://andybrewer.github.io/mvp/" },
            { new FileKey("newcss", "/NewFiles/newcss.css"), "new.css is a classless CSS framework to write modern websites using only HTML. - https://newcss.net/" },
            { new FileKey("simple", "/NewFiles/simple.css"), "Simple.css is a CSS framework that makes semantic HTML look good, really quickly. - https://simplecss.org/" },
        };
    }

    public string GetHelp()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Available templates:");
        foreach (var file in _files)
        {
            sb.AppendLine($"* {file.Key.Name}")
                .AppendLine($"  {file.Value}");
        }
        return sb.ToString();
    }

    public bool TryGetFile(string name, out string content)
    {
        var key = _files.Keys.FirstOrDefault(k => k.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (key != null)
        {
            content = ResourceHandler.GetResourceFile(key.Path);
            return true;
        }
        content = string.Empty;
        return false;
    }
}
