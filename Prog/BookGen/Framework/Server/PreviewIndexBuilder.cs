//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Framework.Server;

internal sealed class PreviewIndexBuilder
{
    private readonly string _directory;
    private readonly Dictionary<string, string[]> _markdownFiles;

    public PreviewIndexBuilder(string directory)
    {
        _directory = directory;
        _markdownFiles = new Dictionary<string, string[]>();
        var subdirs = Directory.GetDirectories(directory, "*.*", SearchOption.AllDirectories);

        foreach (var subdir in subdirs)
        {
            var markdownFiles = Directory.GetFiles(subdir, "*.md");
            if (markdownFiles.Length > 0)
            {
                _markdownFiles.Add(subdir, markdownFiles);
            }
        }
    }

    private IEnumerable<string> GetLink(string file)
    {
        var path = Path.GetFileName(file);
        var directory = Path.GetDirectoryName(file);
        if (directory != _directory)
        {
            path = file.Replace(_directory, "");
        }
        yield return $"<a target=\"_blank\" href=\"{path}\">Preview...</a>";
        yield return $"<a target=\"_blank\" href=\"{path}?edit=true\">Edit...</a>";
    }

    public string RenderIndex()
    {
        var html = new StringBuilder(4096);
        html.WriteHeader(1, "Index of: {0}", _directory);

        foreach (var entry in _markdownFiles)
        {
            html.WriteElement(HtmlElement.Details);
            html.WriteElement(HtmlElement.Summary);
            html.Append(Path.GetFileName(entry.Key));
            html.CloseElement(HtmlElement.Summary);
            html.WriteElement(HtmlElement.Table);
            html.WriteTableHeader("File name", "Actions");
            foreach (var file in entry.Value)
            {
                html.WriteTableRow(Path.GetFileName(file), string.Join(' ', GetLink(file)));
            }
            html.CloseElement(HtmlElement.Table);
            html.CloseElement(HtmlElement.Details);
        }
        return html.ToString();
    }
}
