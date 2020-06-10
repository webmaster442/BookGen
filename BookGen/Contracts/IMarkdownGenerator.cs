//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;

namespace BookGen.Contracts
{
    interface IMarkdownGenerator
    {
        string RunStep(RuntimeSettings settings, ILog log);
    }
}
