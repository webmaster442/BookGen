//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api
{
    /// <summary>
    /// Interface for scripts.
    /// Every Script must implement this interface
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Script name.
        /// </summary>
        string InvokeName { get; }

        /// <summary>
        /// The main entrypoint of the script
        /// </summary>
        /// <param name="host">Current script host</param>
        /// <param name="arguments">Arguments</param>
        /// <returns>Markdown string</returns>
        string ScriptMain(IScriptHost host, IArguments arguments);
    }
}
