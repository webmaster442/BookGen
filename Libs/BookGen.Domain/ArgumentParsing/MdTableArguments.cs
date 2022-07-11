//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------



namespace BookGen.Domain.ArgumentParsing
{
    public sealed class MdTableArguments : ArgumentsBase
    {
        [Switch("d", "delimiter")]
        public char Delimiter { get; set; }

        public MdTableArguments()
        {
            Delimiter = '\t';
        }
    }
}
