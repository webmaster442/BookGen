//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;
using BookGen.WebGui.Domain;

namespace BookGen.WebGui.Services;

public interface IFileService
{
    IList<BrowserItem> GetFiles(string id);
    bool IsPreviewSupported(string id);
    bool IsMarkdown(string id);
    Stream GetContent(string id);
    string GetTextContent(string id);
    FsPath GetDirectoryOf(string id);
    string GetFileNameOf(string id);
    string GetMimeTypeOf(string id);
}