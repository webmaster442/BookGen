//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core
{
    public abstract class Validator
    {
        public List<string> Errors { get; }

        protected Validator()
        {
            Errors = new List<string>();
        }

        protected void AddError(string format, params string[] values)
        {
            Errors.Add(string.Format(format, values));
        }

        public abstract void Validate();

        public bool IsValid => Errors.Count == 0;
    }
}
