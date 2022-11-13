//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.ProjectHandling
{
    public sealed class TocValidator : Validator
    {
        private readonly ITableOfContents _toc;
        private readonly FsPath _workdir;

        public TocValidator(ITableOfContents toc, string workdir)
        {
            _toc = toc;
            _workdir = new FsPath(workdir);
        }

        private static bool IsValidFileName(string name)
        {
            foreach (char chr in name)
            {
                if (!char.IsLetterOrDigit(chr)
                    && chr != '-'
                    && chr != '.'
                    && chr != '_'
                    && chr != '/')
                {
                    return false;
                }
            }
            return true;
        }

        public override void Validate()
        {
            if (!_toc.Chapters.Any())
                AddError(Properties.Resources.TOCNoChapters);

            foreach (string? file in _toc.Files)
            {
                FsPath? source = _workdir.Combine(file);
                if (!source.IsExisting)
                    AddError(Properties.Resources.TOCFileNotExists, file);

                if (!IsValidFileName(file))
                    AddError(Properties.Resources.TOCInvalidFilePathChars, file);
            }
        }
    }
}
