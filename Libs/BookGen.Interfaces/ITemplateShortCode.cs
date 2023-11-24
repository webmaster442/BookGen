//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;

namespace BookGen.Interfaces
{
    public interface ITemplateShortCode
    {
        /// <summary>
        /// ShortCode activator tag
        /// </summary>
        string Tag { get; }
       
        /// <summary>
        /// ShortCode main entry point
        /// </summary>
        /// <param name="arguments">Arguments passed to the short code</param>
        /// <returns>Generated string output</returns>
        string Generate(IArguments arguments);

        /// <summary>
        /// If true, then the shortCode result is cached and not run every time
        /// </summary>
        bool CanCacheResult { get; }
    }
}
