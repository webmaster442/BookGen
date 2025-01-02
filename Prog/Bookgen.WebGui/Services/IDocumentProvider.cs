//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.WebGui.Domain;

namespace BookGen.WebGui.Services;

public interface IDocumentProvider
{
    string GetDocument(Document document);
}