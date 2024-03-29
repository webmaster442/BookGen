﻿//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces.Configuration
{
    /// <summary>
    /// Template options.
    /// </summary>
    public interface IReadOnlyTemplateOptions
    {
        /// <summary>
        /// Tries to get an option from the config
        /// </summary>
        /// <typeparam name="T">Option to cast to type</typeparam>
        /// <param name="setting">Setting name</param>
        /// <param name="value">Casted setting value</param>
        /// <returns>true, if setting found and casted succesfully to target type, otherwise false</returns>
        bool TryGetOption<T>(string setting, out T value) where T : struct;

        /// <summary>
        /// Get a key value
        /// </summary>
        /// <param name="key">key to get</param>
        /// <returns>value of given key</returns>
        string this[string key] { get; }
    }
}
