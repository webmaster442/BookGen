//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    public abstract class ArgumentsBase
    {
        public string[] Files { get; internal set; }

        protected ArgumentsBase()
        {
            Files = Array.Empty<string>();
        }

        public virtual bool Validate()
        {
            return true;
        }
    }
}
