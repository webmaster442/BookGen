//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System.Collections.Generic;

namespace BookGen.Contracts
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
        /// <param name="runtimeSettings">Current runtime settings</param>
        /// <param name="log">Logger</param>
        /// <param name="arguments">Arguments</param>
        /// <returns>Markdown string</returns>
        string ScriptMain(IReadonlyRuntimeSettings runtimeSettings,
                          ILog log,
                          IReadOnlyDictionary<string, string> arguments);
    }
}
