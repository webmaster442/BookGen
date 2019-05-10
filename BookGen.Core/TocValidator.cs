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

        public override void Validate()
        {
            if (!_toc.Chapters.Any())
                AddError("TOC file contains no chapters");

            foreach (var file in _toc.Files)
            {
                var source =  _workdir.Combine(file);
                if (!source.IsExisting)
                    AddError("Source file in toc doesn't exist: {0}", file);
            }
        }
    }
}
