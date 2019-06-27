//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System.Linq;

namespace BookGen.Core
{
    public class TocValidator : Validator
    {
        private readonly ITOC _toc;
        private readonly FsPath _workdir;

        public TocValidator(ITOC toc, string workdir)
        {
            _toc = toc;
            _workdir = new FsPath(workdir);
        }

        private bool IsValidFileName(string name)
        {
            foreach (var chr in name)
            {
                if (!char.IsLetterOrDigit(chr) 
                    && chr != '-'
                    && chr != '.'
                    && chr != '_' )
                {
                    return false;
                }
            }
            return true;
        }

        public override void Validate()
        {
            if (!_toc.Chapters.Any())
                AddError("TOC file contains no chapters");

            foreach (var file in _toc.Files)
            {
                var source =  _workdir.Combine(file);
                if (!source.IsExisting)
                    AddError("Source file in toc doesn't exist: {0}", file);

                if (!IsValidFileName(file))
                    AddError("File path contains invalid chars: {0}. Valid chars: Numbers, letters, _, ., _", file);
            }
        }
    }
}
