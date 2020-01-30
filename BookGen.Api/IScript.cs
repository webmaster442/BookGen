//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api
{
    /// <summary>
    /// Interface for scripts.
    /// Every Script must implement this interface.
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Script name. Later you can reference the script as a shorcode with this name.
        /// </summary>
        string InvokeName { get; }

        /// <summary>
        /// The main entrypoint of the script. It gets executed when parsing the shortcode.
        /// </summary>
        /// <param name="host">Current script host</param>
        /// <param name="arguments">Arguments for the script</param>
        /// <returns>Markdown string</returns>
        string ScriptMain(IScriptHost host, IArguments arguments);
    }
}
